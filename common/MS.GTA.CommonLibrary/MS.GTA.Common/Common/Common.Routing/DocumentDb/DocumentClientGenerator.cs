//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Routing.DocumentDb
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Base.KeyVault;
    using Base.ServiceContext;
    using CommonDataService.Common.Internal;
    using DocumentDB;
    using DocumentDB.Configuration;
    using DocumentDB.CustomSerializers;
    using DocumentDB.Exceptions;
    using DocumentDB.Extensions;
    using Exceptions;
    using Extensions;

    using MS.GTA.Common.Routing.Constants;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Azure.Security;

    /// <summary>The HCM document client provider.</summary>
    public class DocumentClientGenerator : IDocumentClientGenerator
    {
        /// <summary>The cache hours.</summary>
        private const int CacheHours = 4;

        /// <summary>The routing client.</summary>
        private readonly IRoutingClient routingClient;

        /// <summary>The HCM service context.</summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary> Secret manager provider </summary>
        private readonly ISecretManager secretManager;

        /// <summary>Logger</summary>
        private readonly ILogger logger;

        /// <summary>The document client store.</summary>
        private readonly IDocumentClientStore documentClientStore;

        /// <summary>The configuration manager.</summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentClientGenerator" /> class.
        /// </summary>
        /// <param name="routingClient">The routing Client.</param>
        /// <param name="hcmServiceContext">The HCM Service Context.</param>
        /// <param name="secretManager">The secret manager.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="documentClientStore">The document Client Store.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        public DocumentClientGenerator(
            IRoutingClient routingClient,
            IHCMServiceContext hcmServiceContext,
            ISecretManager secretManager,
            ILogger<DocumentClientGenerator> logger,
            IDocumentClientStore documentClientStore,
            IConfigurationManager configurationManager)
        {
            this.routingClient = routingClient;
            this.secretManager = secretManager;
            this.logger = logger;
            this.documentClientStore = documentClientStore;
            this.hcmServiceContext = hcmServiceContext;
            this.configurationManager = configurationManager;
        }

        /// <summary>The get HCM global HCM document client.</summary>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IHcmDocumentClient> GetHcmGlobalHcmDocumentClient(string collectionName, string databaseName = StorageConfigurationSettings.HCMDatabaseId)
        {
            var documentClient = await this.GetHcmGlobalDocumentClient();
            return new HcmDocumentClient(documentClient, databaseName, collectionName);
        }

        /// <summary>The get global HCM document client.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IDocumentClient> GetHcmGlobalDocumentClient()
        {
            var globalDocumentDbStorageConfig = this.routingClient.GetGlobalDocumentDbStorageConfiguration();
            var globalRoutingDocDbConfig = globalDocumentDbStorageConfig.ToGlobalRoutingDocumentDBStorageConfiguration();

            return await this.GetDocumentClient(globalRoutingDocDbConfig);
        }

        /// <summary>The get HCM regional document client. Auto-detects which region to use based on user token.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IDocumentClient> GetHcmRegionalDocumentClient()
        {
            if (this.hcmServiceContext.TenantId != null && this.hcmServiceContext.EnvironmentId != null)
            {
                return await this.GetHcmRegionalDocumentClient(
                           tenantId: this.hcmServiceContext.TenantId,
                           environmentId: this.hcmServiceContext.EnvironmentId,
                           userObjectId: this.hcmServiceContext.ObjectId,
                           resourceName: this.hcmServiceContext.FalconResourceName);
            }

            return null;
        }

        /// <summary>The get HCM regional document client.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="environmentId">The environment id.</param>
        /// <param name="userObjectId">The user Object Id.</param>
        /// <param name="resourceName">The resource Name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IDocumentClient> GetHcmRegionalDocumentClient(string tenantId, string environmentId, string userObjectId = null, string resourceName = null)
        {
            var memCacheKey = environmentId;
            if (!string.IsNullOrEmpty(resourceName))
            {
                memCacheKey = $"{environmentId}_{resourceName}";
            }

            if (!this.documentClientStore.MemoryCache.TryGetValue(memCacheKey, out DocumentDBStorageConfiguration configuration))
            {
                var storageConfiguration = await this.routingClient.GetRegionalDocumentDbStorageConfiguration(environmentId, resourceName);
                if (storageConfiguration == null)
                {
                    throw new GlobalServiceInvalidOperationException($"Could not retrieve cluster for environment: {environmentId}; please verify this environment has been correctly pinned");
                }

                configuration = storageConfiguration.ToDocumentDBStorageConfiguration(tenantId);

                this.documentClientStore.MemoryCache.Set(memCacheKey, configuration, TimeSpan.FromHours(CacheHours));
            }

            return await this.GetDocumentClient(configuration, userObjectId);
        }

        /// <summary>The get HCM document client for the active users environment.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IHcmDocumentClient> GetHcmDocumentClient()
        {
            if (this.hcmServiceContext.TenantId != null && this.hcmServiceContext.EnvironmentId != null)
            {
                var client = await this.GetHcmRegionalDocumentClient();
                if (string.IsNullOrEmpty(this.hcmServiceContext.FalconDatabaseId))
                {
                    return new HcmDocumentClient(client, this.hcmServiceContext.TenantId, this.hcmServiceContext.EnvironmentId);
                }

                return new HcmDocumentClient(client, this.hcmServiceContext.FalconDatabaseId, this.hcmServiceContext.EnvironmentId);
            }

            this.logger.LogWarning($"TenantId and/or EnvironmentId is null");

            return null;
        }

        /// <summary>The get HCM document client for the MS GTA environment.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IHcmDocumentClient> GetGTAHcmDocumentClient()
        {
            var storageConfiguration = this.configurationManager.Get<DocumentDBStorageConfiguration>();
            var client = await this.GetDocumentClient(storageConfiguration);
            return new HcmDocumentClient(client, this.hcmServiceContext.FalconDatabaseId, this.hcmServiceContext.FalconOfferContainerId);
        }

        /// <summary>The get HCM document client for the MS GTA environment.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IHcmDocumentClient> GetGTAHcmDocumentClient(string containerName, string databaseName)
        {
            var storageConfiguration = this.configurationManager.Get<DocumentDBStorageConfiguration>();
            var client = await this.GetDocumentClient(storageConfiguration);
            return new HcmDocumentClient(client, databaseName, containerName);
        }

        /// <summary>
        /// Get document client based on configuration
        /// </summary>
        /// <param name="documentDbStorageConfiguration">DB configurations</param>
        /// <param name="docDbEntityObjectId">The document DB Entity Object Id. This value is used to set the createdBy and updatedBy whenever a document is saved.</param>
        /// <returns>Document Client</returns>
        public async Task<IDocumentClient> GetDocumentClient(DocumentDBStorageConfiguration documentDbStorageConfiguration, string docDbEntityObjectId = null)
        {
            Contract.CheckValue(documentDbStorageConfiguration, nameof(documentDbStorageConfiguration));

            var documentKey = $"{documentDbStorageConfiguration.KeyVaultUri}_{documentDbStorageConfiguration.ConnectionStringSecretName}";
            if (!this.documentClientStore.DocumentDBConnections.ContainsKey(documentKey))
            {
                var secretString = await this.GetDatabaseSecret(
                                       documentDbStorageConfiguration.ConnectionStringSecretName);
                var resourceAccessKey = DocumentDbConnectionStringParser.GetResourceAccessKey(secretString);
                var resourceEndpoint = DocumentDbConnectionStringParser.GetResourceEndpoint(secretString);

                var jsonSettings = new JsonSerializerSettings
                {
                    Converters =
                                            new List<JsonConverter>
                                                {
                                                    new DocDbEntitySerializer(docDbEntityObjectId)
                                                }
                };

                var connectionPolicy = new ConnectionPolicy() { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp };

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    this.logger.LogWarning($"Program has debugger attached and will use https for docdb calls, this will reduce performance but allow for tracing.");

                    connectionPolicy.ConnectionProtocol = Protocol.Https;
                }

                this.documentClientStore.DocumentDBConnections[documentKey] = new DocumentClient(
                    new Uri(resourceEndpoint),
                    resourceAccessKey,
                    jsonSettings,
                    connectionPolicy);
            }

            return this.documentClientStore.DocumentDBConnections[documentKey];
        }

        public async Task<IHcmDocumentClient> GetHcmRegionalHcmDocumentClient(
            string tenantId,
            string environmentId,
            string databaseId,
            string collectionId,
            string partitionKey = null,
            string userObjectId = null,
            string resourceName = null)
        {
            var client = await this.GetHcmRegionalDocumentClient(tenantId, environmentId, userObjectId, resourceName);
            return new HcmDocumentClient(client, databaseId, collectionId, partitionKey);
        }

        /// <summary>
        /// Gets the database secret from the key vault.
        /// </summary>
        /// <param name="keyVaultUri">The key vault URI</param>
        /// <param name="secretName">Name of key vault secret.</param>
        /// <returns>The database secret task.</returns>
        private async Task<string> GetDatabaseSecret(string secretName)
        {
            return await this.secretManager.GetSecretAsync(secretName, this.logger);
        }
    }
}
