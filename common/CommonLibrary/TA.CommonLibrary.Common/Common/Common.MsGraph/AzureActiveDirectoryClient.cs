//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.Extensions.Caching.Memory;

namespace TA.CommonLibrary.Common.MSGraph
{
    using System;
    using System.Collections.Specialized;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Base.Configuration;
    using Base.Security;
    using Base.Utilities;
    using Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using ServicePlatform.Context;
    using ServicePlatform.Security;
    using ServicePlatform.Tracing;
    using AD = Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AAD client class
    /// </summary>
    public sealed class AzureActiveDirectoryClient : IAzureActiveDirectoryClient, IDisposable
    {
        /// <summary>
        /// token cache expiry in minutes
        /// </summary>
        private const int ExpiryMinutes = -5;

        /// <summary>
        /// The AAD constant for identity provider;
        /// </summary>
        private const string IdentityProvider = "idp";

        /// <summary>
        /// Trace source instance
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// Certificate Manager
        /// </summary>
        private CertificateManager certificateManager;

        /// <summary>
        /// Memory cache instance
        /// </summary>
        private readonly MemoryCache accessTokenCache;

        /// <summary>
        /// AAD Client configuration
        /// </summary>
        private readonly AADClientConfiguration aadClientConfig;

        /// <summary>
        /// Graph setting
        /// </summary>
        private readonly MsGraphSetting graphConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureActiveDirectoryClient" /> class.
        /// </summary>
        /// <param name="logger">The instnce for <see cref="ILogger"/>.</param>
        /// <param name="aadClientConfig">AAD client config</param>
        /// <param name="graphConfig">Graph setting object</param>
        public AzureActiveDirectoryClient(ILogger logger, AADClientConfiguration aadClientConfig, MsGraphSetting graphConfig)
        {
            AzureActiveDirectoryClient.logger = logger;
            this.aadClientConfig = aadClientConfig;
            this.graphConfig = graphConfig;
            this.accessTokenCache = new MemoryCache(new MemoryCacheOptions());
            this.certificateManager = new CertificateManager();
        }

        /// <summary>Gets or sets HCM user principal.</summary>    
        public IHCMApplicationPrincipal Principal { get; set; }

        /// <summary>
        /// Dispose memory cache
        /// </summary>
        public void Dispose()
        {
            this.accessTokenCache.Dispose();
        }

        /// <summary>
        /// Gets an Access token for the given resource on behalf of the user in the provided access token
        /// </summary>
        /// <param name="userAccessToken">The access token</param>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="resource">Resource ID</param>
        /// <param name="tokenCachingOptions">Token caching options</param>
        /// <returns>Authentication result which contains on behalf of token</returns>
        public async Task<AD.AuthenticationResult> GetAccessTokenForResourceFromUserTokenAsync(
            string userAccessToken,
            string tenantId,
            string resource,
            TokenCachingOptions tokenCachingOptions)
        {
            logger.LogInformation("Start GetAccessTokenForResourceFromUserTokenAsync");
            var aadInstance = GetAADInstance(this.aadClientConfig.AADInstance, tenantId);
            var exceptions = new List<Exception>();
            var thumbprints = this.aadClientConfig.ClientCertificateThumbprintList;
            var userName = this.GetUserName();
            AD.AuthenticationResult result = null;

            try
            {
                if (tokenCachingOptions == TokenCachingOptions.PreferCache &&
                    this.TryGetAccessToken(resource, tenantId, userName, out result))
                {
                    logger.LogInformation("Retrieved access token from cache.");
                    return result;
                }

                foreach (var thumbprint in thumbprints)
                {
                    try
                    {
                        // Construct context
                        var authority = this.aadClientConfig.AADInstance.FormatWithInvariantCulture(tenantId);
                        var context = new AuthenticationContext(authority, false);
                        context.CorrelationId = ServiceContext.Activity.Current.RootActivityId;

                        // Construct client assertion certificate
                        var certificate = this.certificateManager.FindByThumbprint(thumbprint, StoreName.My, StoreLocation.LocalMachine);
                        var clientAssertionCertificate = new ClientAssertionCertificate(this.aadClientConfig.ClientId, certificate);

                        // User Assertion
                        if (string.IsNullOrEmpty(userAccessToken))
                        {
                            logger.LogInformation("Calling AcquireTokenAsync without User Assertion.");
                            result = await context.AcquireTokenAsync(resource, clientAssertionCertificate);
                        }
                        else
                        {
                            logger.LogInformation("Calling AcquireTokenAsync with User Assertion.");
                            var userAssertion = new UserAssertion(TrimBearerToken(userAccessToken));

                            result = await context.AcquireTokenAsync(resource, clientAssertionCertificate, userAssertion);
                        }

                        logger.LogInformation($"Requesting access token for Resource: '{resource}', AADInstance: '{aadInstance}', ClientID: '{this.aadClientConfig.ClientId}, CorrelationId: '{context.CorrelationId}'");

                        if (!userName.IsNullOrEmpty())
                        {
                            // Set Cache
                            this.SetAccessTokenCache(this.graphConfig.GraphResourceId, this.graphConfig.GraphTenant, userName, result);
                        }

                        return result;
                    }
                    catch (AdalServiceException ex)
                    {
                        logger.LogWarning($"AdalServiceException: error code- {ex.ErrorCode}, error message- {ex.Message}");
                        exceptions.Add(ex);
                    }
                    catch (CertificateNotFoundException ex)
                    {
                        exceptions.Add(ex);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        break;
                    }
                }
            }
            catch (AdalException exception)
            {
                HandleAzureActiveDirectoryClientException(exception);
                return null;
            }

            throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", aadClientConfig.ClientCertificateThumbprintList)}", exceptions);
        }

        /// <summary>
        /// Get on-behalf-of user access token
        /// </summary>
        /// <param name="userAccessToken">Authenticated client access token taken from request header</param>
        /// <param name="tokenCachingOptions">Token caching options</param>
        /// <returns>Authentication result which contains on behalf of token</returns>
        public async Task<AD.AuthenticationResult> GetAccessTokenFromUserTokenAsync(
            string userAccessToken,
            TokenCachingOptions tokenCachingOptions)
        {
            /* Note: In most cases this won't apply as we don't need this for anything but cases where we recieved a hybrid token.
             * Most of the time we will use "common".
             * We need to read the token and check to see if it has an identity provider claim or IDP.
             * This is needed for instances where apps may use a "hybrid" token where it has app claims and user claims mixed together.
             * This will only happen in instances where a token is generated when the issuer is tenant x but we are requesting a token for an app in tenant y
             * and specifically ask that the authority be tenant y. We do this so that the issuer is listed as tenant y instead of x. When this happens the issuer
             * and tenant in the token can no longer be tenant x so it's moved into IDP. This is needed for validation on the server in instances where
             * we only want another app calling and/or we want to see the user token claims with the app token claims.
             * See here for more details: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-token-and-claims#access-tokens
             */

            var tenant = this.graphConfig.GraphTenant;
            if (!string.IsNullOrEmpty(userAccessToken))
            {
                var readToken = (new JwtSecurityTokenHandler()).ReadJwtToken(userAccessToken);
                var identityProviderClaim = readToken.Claims.FirstOrDefault(c => c.Type == IdentityProvider);

                if (identityProviderClaim != null)
                {
                    var tenantMatch = (new Regex(@"https:\/\/sts\.windows\.net\/([0-9A-Za-z-.]*)\/?")).Match(identityProviderClaim.Value);
                    if (tenantMatch.Success)
                    {
                        tenant = tenantMatch.Groups[1].Value;
                        logger.LogInformation($"Tenant {tenant} was matched from the identity provider claim and will be used to generate a token instead of common");
                    }
                    else
                    {
                        logger.LogWarning($"Identity provider claim {identityProviderClaim} was provided but was not matched");
                    }
                }
            }

            return await this.GetAccessTokenForResourceFromUserTokenAsync(userAccessToken, tenant, this.graphConfig.GraphResourceId, tokenCachingOptions);
        }

        /// <summary>Get a token in the app token format for a given resource.</summary>
        /// <param name="tenant">The tenant which the token will be for. Note, the app needs to be inside of the given tenant for this to work.</param>
        /// <param name="resource">The resource the token have access to.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<AD.AuthenticationResult> GetAppToken(string tenant, string resource = null)
        {
            var exceptions = new List<Exception>();
            var thumbprints = this.aadClientConfig.ClientCertificateThumbprintList;
            AD.AuthenticationResult result = null;

            foreach (var thumbprint in thumbprints)
            {
                try
                {
                    var authority = this.aadClientConfig.AADInstance.FormatWithInvariantCulture(tenant);
                    var authenticationContext = new AuthenticationContext(authority);

                    var certificate = this.certificateManager.FindByThumbprint(thumbprint, StoreName.My, StoreLocation.LocalMachine);
                    var clientAssertionCertificate = new ClientAssertionCertificate(this.aadClientConfig.ClientId, certificate);

                    resource = resource ?? this.graphConfig.GraphResourceId;

                    result = await authenticationContext.AcquireTokenAsync(resource, clientAssertionCertificate);
                    return result;
                }
                catch (AdalServiceException ex)
                {
                    exceptions.Add(ex);
                }
                catch (CertificateNotFoundException ex)
                {
                    exceptions.Add(ex);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    break;
                }
            }

            throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", aadClientConfig.ClientCertificateThumbprintList)}", exceptions);
        }

        /// <summary>
        /// Get on-behalf-of user access token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="tokenCachingOptions">Token caching options</param>
        /// <returns>Authentication result which contains on behalf of token</returns>
        public async Task<AD.AuthenticationResult> GetAccessTokenFromRefreshTokenAsync(
            string refreshToken,
            TokenCachingOptions tokenCachingOptions)
        {
            throw new NotImplementedException("This method has been deprecated because refresh tokens can no longer be used to get new user tokens without user interaction");
            var aadInstance = GetAADInstance(this.aadClientConfig.AADInstance, this.graphConfig.GraphTenant);
            var userName = this.GetUserName();
            var exceptions = new List<Exception>();
            var thumbprints = this.aadClientConfig.ClientCertificateThumbprintList;
            AD.AuthenticationResult result = null;

            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            try
            {
                if (tokenCachingOptions == TokenCachingOptions.PreferCache &&
                    this.TryGetAccessToken(this.graphConfig.GraphResourceId, this.graphConfig.GraphTenant, userName, out result))
                {
                    return result;
                }

                foreach (var thumbprint in thumbprints)
                {
                    try
                    {
                        // Construct context
                        var authority = this.aadClientConfig.AADInstance.FormatWithInvariantCulture(this.graphConfig.GraphTenant);
                        var context = new AuthenticationContext(authority, false);
                        context.CorrelationId = ServiceContext.Activity.Current.RootActivityId;

                        // Construct client assertion certificate
                        var certificate = this.certificateManager.FindByThumbprint(thumbprint, StoreName.My, StoreLocation.LocalMachine);
                        var clientAssertionCertificate = new ClientAssertionCertificate(this.aadClientConfig.ClientId, certificate);
                        var userIdentifier = new UserIdentifier(refreshToken, UserIdentifierType.UniqueId);
                        result = await context.AcquireTokenSilentAsync(this.graphConfig.GraphResourceId, clientAssertionCertificate, userIdentifier);
                        
                        /////AcquireTokenByRefreshTokenAsync(refreshToken, clientAssertionCertificate, this.graphConfig.GraphResourceId);

                        logger.LogInformation($"Requesting access token for Resource: '{this.graphConfig.GraphResourceId}', AADInstance: '{aadInstance}', ClientID: '{this.aadClientConfig.ClientId}, CorrelationId: '{context.CorrelationId}'");

                        // Set Cache
                        if (tokenCachingOptions == TokenCachingOptions.PreferCache && !string.IsNullOrEmpty(userName))
                        {
                            this.SetAccessTokenCache(this.graphConfig.GraphResourceId, this.graphConfig.GraphTenant, userName, result);
                        }

                        return result;
                    }
                    catch (AdalServiceException ex)
                    {
                        exceptions.Add(ex);
                    }
                    catch (CertificateNotFoundException ex)
                    {
                        exceptions.Add(ex);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        break;
                    }
                }
            }
            catch (AdalException exception)
            {
                HandleAzureActiveDirectoryClientException(exception);
                return result;
            }

            throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", aadClientConfig.ClientCertificateThumbprintList)}", exceptions);
        }

        /// <summary>
        /// Creates custom AAD Client exception
        /// </summary>
        /// <param name="adalException">ADAL Exception object</param>
        private static void HandleAzureActiveDirectoryClientException(AdalException adalException)
        {
            if (!adalException.ErrorCode.IsNullOrEmpty())
            {
                logger.LogWarning($"AzureActiveDirectoryClientException;ErrorCode={adalException.ErrorCode}, AdalException={adalException}");
            }
            else
            {
                logger.LogWarning($"AzureActiveDirectoryClientException;ErrorMessage={adalException.Message}, AdalException={adalException}");
            }
        }

        /// <summary>
        /// Construct cache key
        /// </summary>
        /// <param name="resourceId">Graph resource ID</param>
        /// <param name="tenant">Graph tenant name (example: common)</param>
        /// <param name="userName">User principal name</param>
        /// <returns>Cache key for on-behalf-of token</returns>
        private static string GetAccessTokenCacheKey(string resourceId, string tenant, string userName)
        {
            logger.LogInformation($"Retrieve AccessTokenCacheKey: {resourceId}-{tenant}-username");
            return FormattableString.Invariant($"{resourceId}-{tenant}-{userName}");
        }

        /// <summary>
        /// Add tenant to AAD instance URL
        /// </summary>
        /// <param name="aadInstanceUrl">AAD instance URL</param>
        /// <param name="tenant">Graph tenant name</param>
        /// <returns>Formatted URL</returns>
        private static string GetAADInstance(string aadInstanceUrl, string tenant)
        {
            logger.LogInformation($"GetAADInstance for {tenant}");
            return aadInstanceUrl.FormatWithInvariantCulture(tenant);
        }

        /// <summary>
        /// Trims bearer token
        /// </summary>
        /// <param name="bearerToken">Bearer token taken from request header</param>
        /// <returns>Trimmed token</returns>
        private static string TrimBearerToken(string bearerToken)
        {
            if (bearerToken.StartsWith(Base.Constants.BearerAuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                bearerToken = bearerToken.Substring(Base.Constants.BearerAuthenticationScheme.Length).Trim();
            }

            return bearerToken;
        }

        /// <summary>
        /// Get user identifier 
        /// </summary>
        /// <returns>email address or user name string</returns>
        private string GetUserName()
        {
            var userName = string.Empty;

            //// Case where service context is not set try to use the current principal property set by the caller
            //// This happens when this is called from service fabric middleware like HCMAuthorizer

            var currentPrincipal = ServiceContext.Principal.TryGetCurrent<IHCMApplicationPrincipal>();
            if (currentPrincipal == null)
            {
                currentPrincipal = this.Principal;
                logger.LogInformation("Using HCM user principle");
            }

            if (currentPrincipal != null)
            {
                userName = string.IsNullOrEmpty(currentPrincipal.UserPrincipalName) ? currentPrincipal.EmailAddress : currentPrincipal.UserPrincipalName;
            }

            logger.LogInformation($"Get userName from principle.");
            return userName;
        }

        /// <summary>
        /// Check MemoryCache to see if token is cached
        /// </summary>
        /// <param name="resourceId">Graph resource ID</param>
        /// <param name="tenant">Graph tenant name</param>
        /// <param name="userName">User identifier</param>
        /// <param name="authenticationResult">Authentication result object</param>
        /// <returns>True if token is cached</returns>
        private bool TryGetAccessToken(string resourceId, string tenant, string userName, out AD.AuthenticationResult authenticationResult)
        {
            logger.LogInformation($"Getting cached access token for resource '{resourceId}'");
            authenticationResult = (AD.AuthenticationResult)this.accessTokenCache.Get(GetAccessTokenCacheKey(resourceId, tenant, userName));
            logger.LogInformation($"Token retrieved: {authenticationResult?.GetHashCode()}");
            return (authenticationResult != null) && (authenticationResult.ExpiresOn > DateTime.UtcNow);
        }

        /// <summary>
        /// Cache on-behalf-of user access token
        /// </summary>
        /// <param name="resourceId">Graph resource ID</param>
        /// <param name="tenant">Graph tenant name</param>
        /// <param name="userName">User identifier</param>
        /// <param name="authenticationResult">Authentication result object</param>
        private void SetAccessTokenCache(string resourceId, string tenant, string userName, AD.AuthenticationResult authenticationResult)
        {
            logger.LogInformation($"Setting cached access token for resource '{resourceId}'");
            var cacheExpirationOptions =
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(ExpiryMinutes),
                    Priority = CacheItemPriority.Normal
                };

            var res = (GetAccessTokenCacheKey(resourceId, tenant, userName), authenticationResult);
            this.accessTokenCache.Set(res, DateTime.Now.ToString(), cacheExpirationOptions);

            logger.LogInformation($"Token set: {authenticationResult?.GetHashCode()}");
        }
    }
}
