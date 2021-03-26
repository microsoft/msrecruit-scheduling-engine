//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using TA.CommonLibrary.Common.Base.Configuration;
    using TA.CommonLibrary.Common.Base.Security.V2;
    using TA.CommonLibrary.Common.Base.ServiceContext;
    using TA.CommonLibrary.ServicePlatform.Azure.AAD;
    using TA.CommonLibrary.ServicePlatform.Azure.Security;
    using TA.CommonLibrary.ServicePlatform.Configuration;

    /// <summary>
    /// XRM HTTP Client generator used for storing the Client across a single request.
    /// </summary>
    public class XrmHttpClientGenerator : IXrmHttpClientGenerator
    {
        private const string ApiBasePath = "/api/data/v9.0/";

        private readonly IHCMPrincipalRetriever principalRetriever;
        private readonly IHCMServiceContext serviceContext;
        private readonly IAzureActiveDirectoryClient azureActiveDirectoryClient;
        private readonly ILogger<XrmHttpClientGenerator> logger;
        private readonly IConfigurationManager configurationManager;

        /// <summary>The XRM Http Client so we can cache it across queries.</summary>
        private IXrmHttpClient xrmHttpClient;
        private IXrmHttpClient adminXrmHttpClient;
        private ConcurrentDictionary<Guid, IXrmHttpClient> userImpersonationXrmHttpClients = new ConcurrentDictionary<Guid, IXrmHttpClient>();

        /// <summary>Initializes a new instance of the <see cref="XrmHttpClientGenerator"/> class.</summary>
        /// <param name="principalRetriever">The principal retriever.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="azureActiveDirectoryClient">The azure active directory client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationManager">The instnce for <see cref="IConfigurationManager"/>.</param>
        public XrmHttpClientGenerator(
            IHCMPrincipalRetriever principalRetriever,
            IHCMServiceContext serviceContext,
            IAzureActiveDirectoryClient azureActiveDirectoryClient,
            ILogger<XrmHttpClientGenerator> logger,
            IConfigurationManager configurationManager)
        {
            this.principalRetriever = principalRetriever;
            this.serviceContext = serviceContext;
            this.azureActiveDirectoryClient = azureActiveDirectoryClient;
            this.logger = logger;
            this.configurationManager = configurationManager;
        }

        /// <summary>Get an XRM http client for the app itself.</summary>
        /// <returns>The client.</returns>
        public async Task<IXrmHttpClient> GetAdminXrmHttpClient()
        {
            if (this.adminXrmHttpClient == null)
            {
                this.adminXrmHttpClient = this.BuildXrmHttpClient(this.serviceContext.XRMInstanceApiUri, this.serviceContext.TenantId, useUserToken: false);
                await this.adminXrmHttpClient.RefreshToken();
            }

            return this.adminXrmHttpClient;
        }

        public async Task<IXrmHttpClient> GetXrmHttpClient()
        {
            if (this.xrmHttpClient == null)
            {
                this.xrmHttpClient = this.BuildXrmHttpClient(this.serviceContext.XRMInstanceApiUri, this.serviceContext.TenantId, useUserToken: true);
                await this.xrmHttpClient.RefreshToken();
            }

            return this.xrmHttpClient;
        }

        /// <summary>Get an XRM http client to impersonate the specified AAD user.</summary>
        /// <returns>The client.</returns>
        public async Task<IXrmHttpClient> GetUserImpersonationXrmHttpClient(Guid userObjectId)
        {
            var userImpersonationXrmHttpClient = this.userImpersonationXrmHttpClients.GetOrAdd(
                userObjectId,
                _ =>
                {
                    var client = this.BuildXrmHttpClient(this.serviceContext.XRMInstanceApiUri, this.serviceContext.TenantId, useUserToken: false);
                    client.ImpersonatedUserObjectId = userObjectId;
                    return client;
                });

            await userImpersonationXrmHttpClient.RefreshToken();
            return userImpersonationXrmHttpClient;
        }

        //TODO: RootActivityID
        private IXrmHttpClient BuildXrmHttpClient(string xrmRestEndpoint, string tenantId, bool useUserToken)
        {
            var baseAddress = new Uri(xrmRestEndpoint.TrimEnd('/') + ApiBasePath);
            return new XrmHttpClient(
                logger: this.logger,
                baseAddress: baseAddress,
                getToken: () => this.GetAccessToken(baseAddress, tenantId, useUserToken),
                getRootActivityId: () => Task.FromResult(Guid.NewGuid().ToString()),
                getActivityVector: () => Task.FromResult(Guid.NewGuid().ToString()),
                tracePotentialPII: false);
        }

        private async Task<Tuple<string, DateTime?>> GetAccessToken(Uri baseAddress, string tenantId, bool useUserToken)
        {
            var aadClientConfiguration = this.configurationManager.Get<AADClientConfiguration>();
            var secretManager = new SecretManager(this.azureActiveDirectoryClient, this.configurationManager);
            var resource = aadClientConfiguration.XRMResource;
            var clientId = aadClientConfiguration.ClientId;
            var clientSId = await secretManager.GetSecretAsync(aadClientConfiguration.ClientCredential, this.logger);
            var authority = string.Format(aadClientConfiguration.AADInstance, aadClientConfiguration.TenantID);
            AuthenticationContext ctx = new AuthenticationContext(authority);

            AuthenticationResult resultToken = await ctx.AcquireTokenAsync(resource, new ClientCredential(clientId, clientSId));
            DateTime? expiration = resultToken.ExpiresOn.UtcDateTime
                    .Subtract(TimeSpan.FromMinutes(10));

            return Tuple.Create(resultToken?.AccessToken, expiration);
        }
    }
}
