//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="GlobalServiceManagementClient.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.GlobalService
{
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Routing.Constants;
    using MS.GTA.Common.Routing.Contracts;
    using MS.GTA.Common.Routing.Exceptions;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.GlobalService.ClientLibrary;
    using MS.GTA.ServicePlatform.GlobalService.Contracts;
    using MS.GTA.ServicePlatform.GlobalService.Contracts.Client;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary> The global service management client class - used for global service management operations</summary>
    public class GlobalServiceManagementClient : IGlobalServiceManagementClient
    {
        /// <summary>The global service client.</summary>
        private readonly IGlobalServiceClient globalServiceClient;

        /// <summary> Logger </summary>
        private readonly ILogger logger;

        /// <summary> Routing client</summary>
        private readonly IRoutingClient routingClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceManagementClient" /> class.
        /// </summary>
        /// <param name="globalServiceClient">The <see cref="IGlobalServiceClient"/></param>
        /// <param name="routingClient">The <see cref="IRoutingClient"/> </param>
        /// <param name="logger">The <see cref="ILogger"/></param>
        public GlobalServiceManagementClient(
            IGlobalServiceClient globalServiceClient,
            IRoutingClient routingClient,
            ILogger<GlobalServiceManagementClient> logger)
        {
            Contract.CheckValue(globalServiceClient, nameof(globalServiceClient), "globalServiceClient should be provided");
            Contract.CheckValue(routingClient, nameof(routingClient), "routingClient should be provided");
            Contract.CheckValue(logger, nameof(logger), "logger should be provided");

            this.globalServiceClient = globalServiceClient;
            this.routingClient = routingClient;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the BAP environment or AAD tenantId's cluster uri
        /// </summary>
        /// <param name="environmentId">The BAP environment ID or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The tenant's cluster uri <see cref="Task{String}"/></returns>
        public async Task<string> GetClusterUri(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetClusterUri: Trying to retrieve cluster CNAME for tenant: {environmentId}");

            return await this.routingClient.GetClusterUri(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the environment information for a BAP environment or AAD tenant Id
        /// </summary>
        /// <param name="environmentId">The environment id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{Environment}"/></returns>
        public async Task<Environment> GetGlobalServiceEnvironment(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetGlobalServiceEnvironment: Trying to retrieve global service environment for id: {environmentId}");

            return await this.routingClient.GetGlobalServiceEnvironment(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets a BAP environment's / AAD tenant's routing information or creates a new entry global service cluster registration entry for new BAP environments/AAD tenant ids
        /// </summary>
        /// <param name="environmentId">The environmentId to use</param>
        /// <param name="bapLocation">The bap location of the environment</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The routing information </returns>
        public async Task<EnvironmentRoutingInformation> PinEnvironmentToClusterInBapLocation(string environmentId, string bapLocation, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.PinEnvironmentToClusterInBapLocation: Trying to retrieve cluster CNAME for environment: {environmentId} or creating a new one in BAP location: {bapLocation}");

            return await this.routingClient.PinEnvironmentToClusterInBapLocation(environmentId, bapLocation, cancellationToken);
        }

        /// <summary>
        /// Get the global document db storage configuration information
        /// </summary>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public StorageConfiguration GetGlobalDocumentDbStorageConfiguration()
        {
            this.logger.LogInformation($"GlobalServiceManagementClient.GetGlobalDocumentDbStorageConfigurationForTenant: Trying to retrieve global doc db storage configuration");
            return this.routingClient.GetGlobalDocumentDbStorageConfiguration();
        }

        /// <summary>
        /// Gets the regional document db storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional document db <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetRegionalDocumentDbStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetRegionalDocumentDbStorageConfigurationForTenant: Trying to retrieve regional doc db storage configuration for id: {environmentId}");

            return await this.routingClient.GetRegionalDocumentDbStorageConfiguration(environmentId, string.Empty, cancellationToken);
        }

        /// <summary>
        /// Gets the regional sql db storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional sql db <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetRegionalSqlDbStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environment id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetRegionalSqlDbStorageConfigurationForTenant: Trying to retrieve regional sql db storage configuration for id: {environmentId}");

            return await this.routingClient.GetRegionalSqlDbStorageConfiguration(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional redis cache storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The redis cache <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetRegionalRedisCacheStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetRegionalRedisCacheStorageConfigurationForTenant: Trying to regional redis cache storage configuration for id: {environmentId}");

            return await this.routingClient.GetRegionalRedisCacheStorageConfiguration(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional Blob storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Blob Storage <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetPrimaryBlobStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetPrimaryBlobStorageConfiguration: Trying to regional Blob storage configuration for id: {environmentId}");

            return await this.routingClient.GetPrimaryBlobStorageConfiguration(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional Blob storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Blob Storage <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetSecondaryBlobStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetSecondaryBlobStorageConfiguration: Trying to regional Blob storage configuration for id: {environmentId}");

            return await this.routingClient.GetSecondaryBlobStorageConfiguration(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional Storage Account configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Storage Account <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetStorageAccountPrimaryConnectionStringConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetPrimaryStorageAccountConfiguration: Trying to regional Storage Account configuration for id: {environmentId}");

            return await this.routingClient.GetStorageAccountPrimaryConnectionStringConfiguration(environmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional Storage Account configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Storage Account <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetStorageAccountSecondaryConnectionStringConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"GlobalServiceManagementClient.GetSecondaryStorageAccountConfiguration: Trying to regional Storage Account configuration for id: {environmentId}");

            return await this.routingClient.GetStorageAccountSecondaryConnectionStringConfiguration(environmentId, cancellationToken);
        }

        /// <summary>
        /// Deletes the BAP environmentId or tenantId cluster mapping from the global service if it exists
        /// </summary>
        /// <param name="tenantId">The AAD tenant id</param>
        /// <param name="cancellationToken">The instance of <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> to await</returns>
        public async Task DeleteGlobalServiceClusterMapping(string tenantId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - tenantId is empty");
            }
            this.logger.LogInformation($"RoutingClient.RemoveTenantFromCluster: Trying to remove tenant: {tenantId} from global service");

            await this.routingClient.DeleteGlobalServiceClusterMapping(tenantId, cancellationToken);
        }

        /// <summary>
        /// Gets a logical cluster
        /// </summary>
        /// <param name="logicalClusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="LogicalCluster"/></returns>
        public async Task<LogicalCluster> GetLogicalCluster(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(logicalClusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - logical cluster id is empty");
            }

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSGetLogClst",
                 async () =>
                 {
                     this.logger.LogInformation($"GlobalServiceManagementClient.GetLogicalCluster: Attempting to retrieve logical cluster: {logicalClusterId}");
                     var logicalCluster = await this.globalServiceClient.GetLogicalClusterAsync(logicalClusterId, cancellationToken);
                     var configFound = logicalCluster != null;
                     this.logger.LogInformation($"GlobalServiceManagementClient.GetLogicalCluster: Logical cluster: {logicalClusterId} retrieval status: {configFound}");
                     return logicalCluster;
                 });
        }

        /// <summary>
        /// Returns all the clusters in an Azure region
        /// </summary>
        /// <param name="region">The region to use</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{LogicalCluster}"/></returns>
        public async Task<IList<LogicalCluster>> GetLogicalClustersInRegion(string region, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(region))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - cluster region is empty");
            }

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSGetRdnClst",
                 async () =>
                 {
                     this.logger.LogInformation($"GlobalServiceManagementClient.GetLogicalClustersInRegion: Retrieving logical clusters in region: {region}");
                     return await this.globalServiceClient.GetLogicalClustersByDatacenterAsync(region, PhysicalClusterType.Dynamics365, cancellationToken);
                 });
        }

        /// <summary>
        /// Gets a storage configuration
        /// </summary>
        /// <param name="storageConfigurationOwnerId">The id of the storage configuration 'owner' - either a tenant or a cluster</param>
        /// <param name="storageConfigurationName">The storage configuration name</param>
        /// <param name="type">The storage configuration type - cluster or tenant</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public async Task<StorageConfiguration> GetStorageConfiguration(
            string storageConfigurationOwnerId,
            string storageConfigurationName,
            StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(storageConfigurationOwnerId, nameof(storageConfigurationOwnerId), "storage configuration name should be defined");
            Contract.CheckNonEmpty(storageConfigurationName, nameof(storageConfigurationName), "storageConfigurationName should be defined");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSDelStrCnfg",
                 async () =>
                 {
                     switch (type)
                     {
                         case StorageConfigurationType.EnvironmentStorageConfiguration:
                             this.logger.LogInformation($"GlobalServiceManagementClient.GetStorageConfiguration: Attempting to retrieve storage config name: {storageConfigurationName} associated with tenant Id: {storageConfigurationOwnerId}");
                             return await this.globalServiceClient.GetEnvironmentStorageConfigurationAsync(storageConfigurationOwnerId, storageConfigurationName, cancellationToken);
                         case StorageConfigurationType.LogicalClusterStorageConfiguration:
                             this.logger.LogInformation($"GlobalServiceManagementClient.GetStorageConfiguration: Attempting to retrieve storage config name: {storageConfigurationName} associated with logical ClusterId : {storageConfigurationOwnerId}");
                             return await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(storageConfigurationOwnerId, storageConfigurationName, cancellationToken);
                         default:
                             throw new GlobalServiceInvalidOperationException($"GlobalServiceManagementClient.GetStorageConfiguration: Unknown storage configuration type: {type}");
                     }

                 });
        }

        /// <summary>
        /// Adds a list of storage configurations
        /// </summary>
        /// <param name="targetId">The id of the target resource (a cluster id or an environment/tenant id)</param>
        /// <param name="storageConfigurations">The list of storage configurations to add</param>
        /// <param name="type">The type of storage configuration</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>the <see cref="Task{IEnumerable{StorageConfiguration}}"/></returns>
        public async Task<IEnumerable<StorageConfiguration>> AddStorageConfigurations(
            string targetId,
            ICollection<StorageConfiguration> storageConfigurations,
            StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(targetId, nameof(targetId), "targetId should be defined");
            Contract.CheckNonEmpty(storageConfigurations, nameof(storageConfigurations));

            this.logger.LogInformation($"GlobalServiceManagementClient.AddStorageConfigurations: Attempting to set storage configuration type: {type} for target: {targetId}");
            var tasks = new List<Task<StorageConfiguration>>();
            foreach (var storageConfiguration in storageConfigurations)
            {
                tasks.Add(this.AddStorageConfiguration(targetId, storageConfiguration, type, cancellationToken));
            }

            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Adds a storage configuration
        /// </summary>
        /// <param name="targetId">The id of the target resource (a cluster id or an environment/tenant id)</param>
        /// <param name="storageConfiguration">The storage configuration to add</param>
        /// <param name="type">The type of storage configuration</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>the <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddStorageConfiguration(
            string targetId,
            StorageConfiguration storageConfiguration,
            StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(targetId, nameof(targetId), "targetId should be defined");
            Contract.CheckValue(storageConfiguration, nameof(storageConfiguration), "storageConfiguration should be defined");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSAddStorCnfg",
                 async () =>
                 {
                     switch (type)
                     {
                         case StorageConfigurationType.EnvironmentStorageConfiguration:
                             this.logger.LogInformation($"GlobalServiceManagementClient.AddStorageConfiguration(Environment): Attempting to set storage config: {storageConfiguration.Name} for environment: {targetId}");
                             return await this.globalServiceClient.AddEnvironmentStorageConfigurationAsync(targetId, storageConfiguration, cancellationToken);
                         case StorageConfigurationType.LogicalClusterStorageConfiguration:
                             this.logger.LogInformation($"GlobalServiceManagementClient.AddStorageConfiguration(Cluster): Attempting to set storage config: {storageConfiguration.Name} for logical cluster: {targetId}");
                             return await this.globalServiceClient.AddLogicalClusterStorageConfigurationAsync(targetId, storageConfiguration, cancellationToken);
                         default:
                             throw new GlobalServiceInvalidOperationException($"GlobalServiceManagementClient.DeleteStorageConfiguration: Unknown storage configuration type: {type}");
                     }
                 });
        }

        /// <summary>
        /// Deletes a storage configuration
        /// </summary>
        /// <param name="id">The id of the storage configuration 'owner' - either a tenant or a cluster</param>
        /// <param name="configName">The storage configuration name</param>
        /// <param name="type">The storage configuration type - cluster or tenant</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteStorageConfiguration(
            string id,
            string configName,
            StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(id, nameof(id));
            Contract.CheckNonEmpty(configName, nameof(configName));

            await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSDelStrCnfg",
                 async () =>
                 {
                     switch (type)
                     {
                         case StorageConfigurationType.EnvironmentStorageConfiguration:
                             this.logger.LogInformation($"GlobalServiceManagementClient.DeleteStorageConfiguration: Attempting to delete storage config name: {configName} associated with tenant Id : {id}");
                             await this.globalServiceClient.DeleteEnvironmentStorageConfigurationAsync(id, configName, cancellationToken);
                             break;
                         case StorageConfigurationType.LogicalClusterStorageConfiguration:
                             this.logger.LogInformation($"GlobalServiceManagementClient.DeleteStorageConfiguration: Attempting to delete storage config name: {configName} associated with logical ClusterId : {id}");
                             await this.globalServiceClient.DeleteLogicalClusterStorageConfigurationAsync(id, configName, cancellationToken);
                             break;
                         default:
                             throw new GlobalServiceInvalidOperationException($"GlobalServiceManagementClient.DeleteStorageConfiguration: Unknown storage configuration type: {type}");
                     }
                 });
        }

        /// <summary>
        /// Adds a regional document db storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional document db secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalDocDb(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalDocumentDbStorageConfiguration(keyVaultUri);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }


        /// <summary>
        /// Adds a regional sql db storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional sql db secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalSqlDb(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalSqlDbStorageConfiguration(keyVaultUri);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }

        /// <summary>
        /// Adds a regional redis cache configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the redis' cache's secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalRedisCache(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalRedisStorageConfiguration(keyVaultUri);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }

        /// <summary>
        /// Adds a regional Blob configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the Blob storage secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalBlobPrimary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalBlobStorageConfiguration(StorageConfigurationSettings.HCMPrimaryBlobStorageConfigName, keyVaultUri, StorageConfigurationSettings.HCMRegionalBlobPrimarySecretName);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }

        /// <summary>
        /// Adds a regional Blob configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the Blob storage secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalBlobSecondary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalBlobStorageConfiguration(StorageConfigurationSettings.HCMSecondaryBlobStorageConfigName, keyVaultUri, StorageConfigurationSettings.HCMRegionalBlobSecondarySecretName);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }

        /// <summary>
        /// Adds a regional Storage Account configuration to a cluster.
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the Storage Account secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalStorageAccountPrimary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalStorageAccountConfiguration(StorageConfigurationSettings.HCMStorageAccountPrimaryConnectionStringConfigName, keyVaultUri, StorageConfigurationSettings.HCMRegionalStorageAccountPrimarySecretName);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }

        /// <summary>
        /// Adds a regional Storage Account configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the Storage Account secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> AddClusterRegionalStorageAccountSecondary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(clusterId, nameof(clusterId), "clusterId should be defined");
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            var storageConfiguration = this.GetRegionalStorageAccountConfiguration(StorageConfigurationSettings.HCMStorageAccountSecondaryConnectionStringConfigName, keyVaultUri, StorageConfigurationSettings.HCMRegionalStorageAccountSecondarySecretName);
            return await this.AddStorageConfiguration(clusterId, storageConfiguration, StorageConfigurationType.LogicalClusterStorageConfiguration, cancellationToken);
        }

        /// <summary>
        /// Gets the regional document DB storage configuration
        /// </summary>
        /// <param name="keyVaultUri">The country code override</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        private StorageConfiguration GetRegionalDocumentDbStorageConfiguration(string keyVaultUri)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            return new StorageConfiguration()
            {
                Name = StorageConfigurationSettings.HCMRegionalDocumentDbStorageConfigName,
                Type = StorageConfigurationResourceType.DocumentDb,
                ConnectionDetails = new ConnectionDetails()
                {
                    KeyVaultName = keyVaultUri,
                    Type = ConnectionDetailsType.AzureKeyVault,
                    KeyVaultSecretName = StorageConfigurationSettings.HCMRegionalDocumentDbSecretName,
                }
            };
        }

        /// <summary>
        /// Gets the regional sql DB storage configuration
        /// </summary>
        /// <param name="keyVaultUri">The country code override</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        private StorageConfiguration GetRegionalSqlDbStorageConfiguration(string keyVaultUri)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            return new StorageConfiguration()
            {
                Name = StorageConfigurationSettings.HCMRegionalSqlDbStorageConfigName,
                Type = StorageConfigurationResourceType.SqlDb,
                ConnectionDetails = new ConnectionDetails()
                {
                    KeyVaultName = keyVaultUri,
                    Type = ConnectionDetailsType.AzureKeyVault,
                    KeyVaultSecretName = StorageConfigurationSettings.HCMRegionalSqlDbSecretName,
                }
            };
        }

        /// <summary>
        /// Gets the regional REDIS storage configuration
        /// </summary>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        private StorageConfiguration GetRegionalRedisStorageConfiguration(string keyVaultUri)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");

            return new StorageConfiguration()
            {
                Name = StorageConfigurationSettings.HCMRegionalRedisCacheStorageConfigName,
                Type = StorageConfigurationResourceType.RedisCache,
                ConnectionDetails = new ConnectionDetails()
                {
                    KeyVaultName = keyVaultUri,
                    Type = ConnectionDetailsType.AzureKeyVault,
                    KeyVaultSecretName = StorageConfigurationSettings.HCMRegionalRedisSecretName,
                }
            };
        }

        /// <summary>
        /// Gets the regional Blob storage configuration
        /// </summary>
        /// <param name="blobStorage">The Blob Storage</param>
        /// <param name="keyVaultUri">The Keyvault Uri</param>
        /// <param name="keyVaultSecretName">The blob connection string secret name</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        private StorageConfiguration GetRegionalBlobStorageConfiguration(string blobStorage, string keyVaultUri, string keyVaultSecretName)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");
            Contract.CheckNonEmpty(keyVaultSecretName, nameof(keyVaultSecretName), "keyVaultSecretName should be defined");

            return new StorageConfiguration()
            {
                Name = blobStorage,
                Type = StorageConfigurationResourceType.BlobStorage,
                ConnectionDetails = new ConnectionDetails()
                {
                    KeyVaultName = keyVaultUri,
                    Type = ConnectionDetailsType.AzureKeyVault,
                    KeyVaultSecretName = keyVaultSecretName,
                }
            };
        }

        /// <summary>
        /// Gets the regional Storage Account configuration
        /// </summary>
        /// <param name="storageAccount">The Storage Account</param>
        /// <param name="keyVaultUri">The Keyvault Uri</param>
        /// <param name="keyVaultSecretName">The storage account connection string secret name</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        private StorageConfiguration GetRegionalStorageAccountConfiguration(string storageAccount, string keyVaultUri, string keyVaultSecretName)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri), "keyVaultUri should be defined");
            Contract.CheckNonEmpty(keyVaultSecretName, nameof(keyVaultSecretName), "keyVaultSecretName should be defined");

            return new StorageConfiguration()
            {
                Name = storageAccount,
                Type = StorageConfigurationResourceType.StorageAccount,
                ConnectionDetails = new ConnectionDetails()
                {
                    KeyVaultName = keyVaultUri,
                    Type = ConnectionDetailsType.AzureKeyVault,
                    KeyVaultSecretName = keyVaultSecretName,
                }
            };
        }
    }
}
