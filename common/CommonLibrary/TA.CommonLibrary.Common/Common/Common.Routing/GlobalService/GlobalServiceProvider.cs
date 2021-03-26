//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Routing.GlobalService
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using BapClient;
    using Base.Configuration;
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using DocumentDB.V2;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using TA.CommonLibrary.Common.Contracts;
    using Microsoft.Extensions.Logging;
    using Routing.Configuration;
    using Routing.Constants;
    using Routing.Contracts;
    using ServicePlatform.Communication.Http;
    using ServicePlatform.Configuration;
    using ServicePlatform.Context;
    using ServicePlatform.GlobalService.ClientLibrary;
    using ServicePlatform.GlobalService.Contracts.Client;
    using ServicePlatform.Security;

    /// <summary>
    /// The Global Service client provider
    /// </summary>
    public class GlobalServiceClientProvider : IGlobalServiceClientProvider
    {
        /// <summary>Gets or sets the AAD configuration.</summary
        private readonly AADClientConfiguration aadClientConfiguration;

        /// <summary> Routing client Configuration </summary>
        private readonly GlobalServiceConfiguration globalServiceConfiguration;

        /// <summary> Route Configuration </summary>
        private readonly IHttpCommunicationClientFactory clientFactory;

        /// <summary> Logger factory </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary> Logger </summary>
        private readonly ILogger logger;

        /// <summary> The global document db storage configuration</summary>
        private readonly StorageConfiguration globalDocumentDbStorageConfiguration;

        /// <summary>The HCM cluster to BAP location mapping information.</summary>
        private readonly IDocumentDBProvider<HCMClusterToBAPLocationMapCollection> hcmClusterToBapLocationMappings;

        /// <summary>The document database repository for the environments information.</summary>
        private readonly IDocumentDBProvider<EnvironmentDocument> environmentRepository;

        /// <summary>The bap service client.</summary>
        private readonly IBapServiceClient bapServiceClient;

        /// <summary> The global service client </summary>
        private IGlobalServiceClient globalServiceClient;

        /// <summary> The routing client </summary>
        private RoutingClient routingClient;

        /// <summary> The global service management client </summary>
        private GlobalServiceManagementClient globalServiceManagementClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceClientProvider" /> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager</param>
        /// <param name="clientFactory">The http client factory</param>
        /// <param name="bapServiceClient">The bap service client.</param>
        /// <param name="hcmClusterToBapLocationMappings">The HCM cluster to BAP location mapping information.</param>
        /// <param name="environmentRepository">The environments repository</param>
        /// <param name="loggerFactory">The logger factory</param>
        public GlobalServiceClientProvider(
            IConfigurationManager configurationManager,
            IHttpCommunicationClientFactory clientFactory,
            IBapServiceClient bapServiceClient,
            IDocumentDBProvider<HCMClusterToBAPLocationMapCollection> hcmClusterToBapLocationMappings,
            IDocumentDBProvider<EnvironmentDocument> environmentRepository,
            ILoggerFactory loggerFactory)
        {
            Contract.AssertValue(configurationManager, nameof(configurationManager));
            Contract.AssertValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(bapServiceClient, nameof(bapServiceClient));
            Contract.AssertValue(hcmClusterToBapLocationMappings, nameof(hcmClusterToBapLocationMappings));
            Contract.CheckValue(environmentRepository, nameof(environmentRepository), "environmentRepository should be provided");
            Contract.AssertValue(loggerFactory, nameof(loggerFactory));

            this.clientFactory = clientFactory;
            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger<GlobalServiceClientProvider>();
            var environmentName = configurationManager.Get<EnvironmentNameConfiguration>().EnvironmentName;
            this.globalServiceConfiguration = GlobalServiceEnvironmentSettings.GetGlobalServiceConfiguration(environmentName);
            this.globalDocumentDbStorageConfiguration = GlobalServiceEnvironmentSettings.GetGlobalDocumentDbStorageConfiguration(environmentName);
            this.aadClientConfiguration = configurationManager.Get<AADClientConfiguration>();
            this.hcmClusterToBapLocationMappings = hcmClusterToBapLocationMappings;
            this.environmentRepository = environmentRepository;
            this.bapServiceClient = bapServiceClient;
        }

        /// <summary> Gets the global service client </summary>
        private IGlobalServiceClient GlobalServiceClient
        {
            get
            {
                if (this.globalServiceClient == null)
                {
                    this.globalServiceClient = this.CreateGlobalServiceClient();
                }

                return this.globalServiceClient;
            }
        }

        /// <summary> Gets the routing client </summary>
        private RoutingClient RoutingClient
        {
            get
            {
                if (this.routingClient == null)
                {
                    this.routingClient = new RoutingClient(this.GlobalServiceClient,
                        this.aadClientConfiguration.TenantID,
                        this.loggerFactory.CreateLogger<RoutingClient>(),
                        this.hcmClusterToBapLocationMappings,
                        this.environmentRepository,
                        this.bapServiceClient,
                        this.globalDocumentDbStorageConfiguration);
                }

                return this.routingClient;
            }
        }

        /// <summary> Gets the global service management client </summary>
        private GlobalServiceManagementClient GlobalServiceManagementClient
        {
            get
            {
                if (this.globalServiceManagementClient == null)
                {
                    this.globalServiceManagementClient = new GlobalServiceManagementClient(
                        this.GlobalServiceClient,
                        this.RoutingClient,
                        this.loggerFactory.CreateLogger<GlobalServiceManagementClient>());
                }

                return this.globalServiceManagementClient;
            }
        }

        /// <summary> Gets the singleton routing client </summary>
        /// <returns>The singleton <see cref="RoutingClient"/> instance </returns>
        public RoutingClient GetSingletonRoutingClient() => this.RoutingClient;

        /// <summary> Gets the singleton global service management client </summary>
        /// <returns>The singleton <see cref="GlobalServiceManagementClient"/> instance </returns>
        public GlobalServiceManagementClient GetSingletonManagementClient() => this.GlobalServiceManagementClient;

        /// <summary> Gets the singleton global service management client </summary>
        /// <returns>The singleton <see cref="GlobalServiceManagementClient"/> instance </returns>
        public IGlobalServiceClient GetGlobalServiceClient() => this.globalServiceClient;

        /// <summary>
        /// Creates the singleton external global service client
        /// </summary>
        /// <returns>The <see cref="IGlobalServiceClient"/></returns>
        private IGlobalServiceClient CreateGlobalServiceClient()
        {
            var uri = new Uri(this.globalServiceConfiguration.ClusterUri);
            return new ExternalGlobalServiceClient(
                this.clientFactory,
                () => this.GetAuthToken(),
                uri,
                this.loggerFactory.CreateLogger<ExternalGlobalServiceClient>());
        }

        /// <summary>
        /// Generates a token to use for the Global Service
        /// </summary>
        /// <returns>The <see cref="Task{AuthenticationHeaderValue}"/></returns>
        private async Task<AuthenticationHeaderValue> GetAuthToken()
        {
            var exceptions = new List<Exception>();
            var thumprints = this.aadClientConfiguration.ClientCertificateThumbprintList;
            foreach (var thumprint in thumprints)
            {
                try
                {
                    var authority = this.aadClientConfiguration.AADInstance.FormatWithInvariantCulture(this.aadClientConfiguration.TenantID);
                    var authContext = new AuthenticationContext(authority);

                    var clientCertificate = this.FindCertificateFromStoreByThumbprint(thumprint, StoreLocation.LocalMachine)
                                         ?? this.FindCertificateFromStoreByThumbprint(thumprint, StoreLocation.CurrentUser);

                    var clientAssertion = new ClientAssertionCertificate(this.aadClientConfiguration.ClientId, clientCertificate);

                    return await this.logger.ExecuteAsync(
                        "HcmGSAcquireTkn",
                        async () =>
                        {
                            this.logger.LogInformation("Retrieving Global service app token");
                            var authenticationResult = await authContext.AcquireTokenAsync(this.globalServiceConfiguration.Resource, clientAssertion);
                            return new AuthenticationHeaderValue(authenticationResult.AccessTokenType, authenticationResult.AccessToken);
                        });
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", this.aadClientConfiguration.ClientCertificateThumbprintList)}", exceptions);
        }

        /// <summary>
        /// Attempts to retrieve a certificate by thumbprint from the specified store location.
        /// </summary>
        /// <param name="thumbprint">The certificate thumbprint.</param>
        /// <param name="storeLocation">The certificate store location to search within.</param>
        /// <returns>The <see cref="Task{X509Certificate2}"/></returns>
        private X509Certificate2 FindCertificateFromStoreByThumbprint(string thumbprint, StoreLocation storeLocation)
        {
            Contract.CheckNonEmpty(thumbprint, nameof(thumbprint), "Certificate thumbprint must not be empty.");

            var certificateManager = new CertificateManager();
            X509Certificate2 clientCertificate = null;
            var storeName = StoreName.My;

            this.logger.LogWarning($"Attempting to search for certificate by thumbprint {thumbprint} from store name {storeName} at store location {storeLocation}.");

            try
            {
                clientCertificate = certificateManager.FindByThumbprint(thumbprint, storeName, storeLocation);
            }
            catch (Exception e)
            {
                this.logger.LogError($"Failed to retrieve certificate by thumbprint {thumbprint} from store name {storeName} at store location {storeLocation}. Exception: {e.Message}.");
            }

            if (clientCertificate != null)
            {
                this.logger.LogInformation($"Successfully retrieved certificate by thumbprint {thumbprint} from store name {storeName} at store location {storeLocation}.");
            }
            else
            {
                this.logger.LogWarning($"Successfully searched for (but did not find) certificate by thumbprint {thumbprint} from store name {storeName} at store location {storeLocation}.");
            }

            return clientCertificate;
        }
    }
}
