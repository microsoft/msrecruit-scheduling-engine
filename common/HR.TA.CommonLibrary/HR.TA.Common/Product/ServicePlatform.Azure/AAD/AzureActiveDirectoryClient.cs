//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HR.TA.CommonDataService.Common.Internal;
using Microsoft.Extensions.Logging;
using HR.TA.ServicePlatform.Configuration;
using HR.TA.ServicePlatform.Context;
using HR.TA.ServicePlatform.Exceptions;
using HR.TA.ServicePlatform.Security;
using HR.TA.ServicePlatform.Tracing;

namespace HR.TA.ServicePlatform.Azure.AAD
{
    //// Using statement inside namespace to avoid ambigutity with AuthenticationResult
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// A class to return the access token to authenticate access to azure resource using application id and certificate
    /// </summary>
    public sealed class AzureActiveDirectoryClient : IAzureActiveDirectoryClient
    {
        private readonly AzureActiveDirectoryClientConfiguration azureActiveDirectoryConfiguration;
        private readonly ICertificateManager certificateManager;
        private readonly ILogger logger;
        private readonly TokenCache tokenCache;
        private readonly Func<string, TokenCache, IAuthenticationContextWrapper> authenticationContextFactory;
        private volatile int lastGoodIndex;

        public AzureActiveDirectoryClient(IConfigurationManager configurationManager, ICertificateManager certificateManager)
            : this(configurationManager.Get<AzureActiveDirectoryClientConfiguration>(), certificateManager)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
        }

        public AzureActiveDirectoryClient(IConfigurationManager configurationManager, ICertificateManager certificateManager, TokenCache tokenCache)
            : this(configurationManager.Get<AzureActiveDirectoryClientConfiguration>(), certificateManager, tokenCache)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
        }

        public AzureActiveDirectoryClient(AzureActiveDirectoryClientConfiguration azureActiveDirectoryConfiguration, ICertificateManager certificateManager)
            : this(azureActiveDirectoryConfiguration, certificateManager, TokenCache.DefaultShared)
        {
        }

        public AzureActiveDirectoryClient(AzureActiveDirectoryClientConfiguration azureActiveDirectoryConfiguration, ICertificateManager certificateManager, TokenCache tokenCache)
            : this(azureActiveDirectoryConfiguration, certificateManager, tokenCache, (authority, aadTokenCache) => new AuthenticationContextWrapper(authority, aadTokenCache))
        {
        }

        internal AzureActiveDirectoryClient(AzureActiveDirectoryClientConfiguration azureActiveDirectoryConfiguration, ICertificateManager certificateManager, TokenCache tokenCache, Func<string, TokenCache, IAuthenticationContextWrapper> authenticationContextFactory)
        {
            Contract.CheckValue(azureActiveDirectoryConfiguration, nameof(azureActiveDirectoryConfiguration));
            Contract.CheckRange(azureActiveDirectoryConfiguration.ClientCertificateThumbprintList.Count >= 1, nameof(azureActiveDirectoryConfiguration.ClientCertificateThumbprintList));
            Contract.CheckAllNonEmpty(azureActiveDirectoryConfiguration.ClientCertificateThumbprintList, nameof(azureActiveDirectoryConfiguration.ClientCertificateThumbprintList));
            Contract.CheckValue(certificateManager, nameof(certificateManager));
            Contract.CheckValue(authenticationContextFactory, nameof(authenticationContextFactory));
            Contract.CheckValue(tokenCache, nameof(tokenCache));

            this.azureActiveDirectoryConfiguration = azureActiveDirectoryConfiguration;
            this.certificateManager = certificateManager;
            this.authenticationContextFactory = authenticationContextFactory;
            this.tokenCache = tokenCache;
            this.logger = TraceSourceMeta.LoggerFactory.CreateLogger(nameof(AzureActiveDirectoryClient));
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> GetAppOnlyAccessTokenAsync(string authority, string resource)
        {
            return GetAppOnlyAccessTokenAsync(azureActiveDirectoryConfiguration.Authority, resource);
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> GetAppOnlyAccessTokenAsync(string resource)
        {
            string authority = azureActiveDirectoryConfiguration.Authority;
            Contract.CheckNonEmpty(authority, nameof(authority));
            Contract.CheckNonEmpty(resource, nameof(resource));
            
            return await logger.ExecuteAsync(
                        "SP.AadClient.AcquireToken",
                        async () =>
                        {
                            return await AcquireAccessTokenAsync(
                                authority,
                                (context, clientAssertionCertificate) => context.AcquireTokenAsync(resource, clientAssertionCertificate));
                        });
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> GetUserAccessTokenAsync(string resource, string clientId, UserCredential userCredential)
        {
            Contract.CheckNonEmpty(resource, nameof(resource));
            Contract.CheckNonEmpty(clientId, nameof(clientId));
          
            return await logger.ExecuteAsync(
                        "SP.AadClient.AcquireToken",
                        async () =>
                        {
                            return await AcquireAccessTokenAsync(
                                azureActiveDirectoryConfiguration.Authority,
                                (context, clientAssertionCertificate) => context.AcquireTokenAsync(resource,clientId,userCredential));
                        });
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> GetAccessTokenByAuthorizationCode(string authorizationCode, Uri redirectUrl)
        {
            Contract.CheckNonEmpty(authorizationCode, nameof(authorizationCode));
            Contract.CheckValue(redirectUrl, nameof(redirectUrl));

            return await logger.ExecuteAsync(
                        "SP.AadClient.AcquireTokenByAuthCode",
                        async () =>
                        {
                            return await AcquireAccessTokenAsync(
                                azureActiveDirectoryConfiguration.Authority,
                                (context, clientAssertionCertificate) => context.AcquireTokenByAuthorizationCodeAsync(authorizationCode, redirectUrl, clientAssertionCertificate));
                        });
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> GetOnBehalfOfAccessTokenAsync(string resource, string userAccessToken)
        {
            return GetOnBehalfOfAccessTokenAsync(azureActiveDirectoryConfiguration.Authority, resource, userAccessToken);
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> GetOnBehalfOfAccessTokenAsync(string authority, string resource, string userAccessToken)
        {
            Contract.CheckNonEmpty(authority, nameof(authority));
            Contract.CheckNonEmpty(authority, nameof(resource));
            Contract.CheckNonEmpty(userAccessToken, nameof(userAccessToken));

            return await logger.ExecuteAsync(
                        "SP.AadClient.AcquireTokenOBO",
                        async () =>
                        {
                            // Note, ADAL 3 uses:  UserAssertion userAssertion = new UserAssertion(bootstrapContext.Token, "urn:ietf:params:oauth:grant-type:jwt-bearer", userName);
                            UserAssertion userAssertion = new UserAssertion(userAccessToken);

                            return await AcquireAccessTokenAsync(
                                authority,
                                (context, clientAssertionCertificate) => context.AcquireTokenAsync(resource, clientAssertionCertificate, userAssertion));
                        });
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> GetAccessTokenSilentAsync(string resource, UserIdentifier userIdentifier)
        {
            return GetAccessTokenSilentAsync(azureActiveDirectoryConfiguration.Authority, resource, userIdentifier);
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> GetAccessTokenSilentAsync(string authority, string resource, UserIdentifier userIdentifier)
        {
            Contract.CheckNonEmpty(authority, nameof(authority));
            Contract.CheckNonEmpty(authority, nameof(resource));
            Contract.CheckValue(userIdentifier, nameof(userIdentifier));

            return await logger.ExecuteAsync(
                        "SP.AadClient.AcquireTokenSilent",
                        async () =>
                        {
                            return await AcquireAccessTokenAsync(
                                authority,
                                (context, clientAssertionCertificate) => context.AcquireTokenSilentAsync(resource, clientAssertionCertificate, userIdentifier));
                        });
        }

        /// <summary>
        /// Used to perform the common portion of algorithm for acquiring the access token.
        /// </summary>
        /// <param name="authority">Address of the authority to issue token.</param>
        /// <param name="acquireTokenAsync">Function that acquires the access token.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        private async Task<AuthenticationResult> AcquireAccessTokenAsync(string authority, Func<IAuthenticationContextWrapper, ClientAssertionCertificate, Task<AuthenticationResult>> acquireTokenAsync)
        {
            var exceptions = new List<Exception>();
            var thumbprints = this.azureActiveDirectoryConfiguration.ClientCertificateThumbprintList;

            var start = lastGoodIndex;
            for (var i = start; i < start + thumbprints.Count; i++)
            {
                var thumbprint = thumbprints[i % thumbprints.Count];

                try
                {
                    var certificate = await certificateManager.FindByThumbprintAsync(
                        thumbprint,
                        this.azureActiveDirectoryConfiguration.ClientCertificateStoreName,
                        this.azureActiveDirectoryConfiguration.ClientCertificateStoreLocation);

                    var clientAssertionCertificate = new ClientAssertionCertificate(azureActiveDirectoryConfiguration.ClientId, certificate);
                    var context = authenticationContextFactory(authority, tokenCache);
                    var result = await acquireTokenAsync(context, clientAssertionCertificate);

                    if (i != start)
                    {
                        lastGoodIndex = i % thumbprints.Count;
                    }

                    return result;
                }
                catch (AdalServiceException ex) when (ex.StatusCode == 401 && ex.ErrorCode == "invalid_client")
                {
                    exceptions.Add(new ClientCertificateUnauthorizedException(thumbprint, ex).EnsureTraced());
                }
                catch (CertificateNotFoundException ex)
                {
                    exceptions.Add(ex);
                }
                catch (Exception ex)
                {
                    // Exceptions other than certificate not found and invalid client are non-recoverable
                    // so we break out of loop to avoid continued failures
                    // plus lastGoodIndex is not modified
                    exceptions.Add(ex);
                    break;
                }
            }

            throw new AggregateException($"Could not successfully acquire access token using thumbprints: {string.Join(", ", azureActiveDirectoryConfiguration.ClientCertificateThumbprintList)}", exceptions);
        }

        /// <summary>
        /// Exception thrown when AAD authentication rejects the client certificate.
        /// </summary>
        [MonitoredExceptionMetadata(
            HttpStatusCode.Unauthorized,
            ErrorNamespaces.ServicePlatform,
            "ClientCertificateUnauthorizedException",
            MonitoredExceptionKind.Service)]
        internal sealed class ClientCertificateUnauthorizedException : MonitoredException
        {
            public ClientCertificateUnauthorizedException(string thumbprint, Exception innerException)
                : base($"Azure Active Directory rejected client authentication certificate with thumbprint:'{thumbprint}'", innerException)
            {
                Contract.AssertValue(thumbprint, nameof(thumbprint));

                Thumbprint = thumbprint;
            }

            [ExceptionCustomData(
                Name = "thumbprint",
                Serialize = true)]
            public string Thumbprint { get; }
        }
    }
}
