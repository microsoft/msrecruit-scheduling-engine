//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IGlobalServiceManagementClient.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.GlobalService
{
    using MS.GTA.Common.Routing.Contracts;
    using MS.GTA.ServicePlatform.GlobalService.Contracts.Client;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary> The global service management client interface</summary>
    public interface IGlobalServiceManagementClient
    {
        /// <summary>
        /// Gets the BAP environment or AAD tenantId's cluster uri
        /// </summary>
        /// <param name="tenantId">The AAD tenant id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The routing information </returns>
        Task<string> GetClusterUri(string tenantId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environment information for a BAP environment or AAD tenant Id
        /// </summary>
        /// <param name="tenantId">The tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{Environment}"/></returns>
        Task<Environment> GetGlobalServiceEnvironment(string tenantId, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Gets a BAP environment's / AAD tenant's routing information or creates a new entry global service cluster registration entry for new BAP environments/AAD tenant ids
        /// </summary>
        /// <param name="environmentId">The environmentId to use</param>
        /// <param name="bapLocation">The bap location of the environment</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The routing information </returns>
        Task<EnvironmentRoutingInformation> PinEnvironmentToClusterInBapLocation(string environmentId, string bapLocation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the global document db storage configuration information
        /// </summary>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        StorageConfiguration GetGlobalDocumentDbStorageConfiguration();

        /// <summary>
        /// Gets the regional document db storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional document db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        Task<StorageConfiguration> GetRegionalDocumentDbStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional sql db storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional sql db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        Task<StorageConfiguration> GetRegionalSqlDbStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional redis cache storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The redis cache <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetRegionalRedisCacheStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Blob storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Blob Storage <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetPrimaryBlobStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Blob storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Blob Storage <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetSecondaryBlobStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Storage Account configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Storage Account <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetStorageAccountPrimaryConnectionStringConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Storage Account configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Storage Account <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetStorageAccountSecondaryConnectionStringConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the BAP environmentId or tenantId cluster mapping from the global service if it exists
        /// </summary>
        /// <param name="tenantId">The AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>A <see cref="Task"/> to await</returns>
        Task DeleteGlobalServiceClusterMapping(string tenantId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a logical cluster
        /// </summary>
        /// <param name="id">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="LogicalCluster"/></returns>
        Task<LogicalCluster> GetLogicalCluster(string id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the logical clusters in an azure region
        /// </summary>
        /// <param name="region">The region to use</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{IList{LogicalCluster}}"/></returns>
        Task<IList<LogicalCluster>> GetLogicalClustersInRegion(string region, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a storage configuration
        /// </summary>
        /// <param name="storageConfigurationOwnerId">The id of the storage configuration 'owner' - either a tenant or a cluster</param>
        /// <param name="storageConfigurationName">The storage configuration name</param>
        /// <param name="type">The storage configuration type - cluster or tenant</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetStorageConfiguration(string storageConfigurationOwnerId, string storageConfigurationName, StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a list of storage configurations
        /// </summary>
        /// <param name="targetId">The id of the target resource (a cluster id or an environment/tenant id)</param>
        /// <param name="storageConfigurations">The list of storage configurations to add</param>
        /// <param name="type">The type of storage configuration</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>the <see cref="Task{IEnumerable{StorageConfiguration}}"/></returns>
        Task<IEnumerable<StorageConfiguration>> AddStorageConfigurations(string targetId, ICollection<StorageConfiguration> storageConfigurations, StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a storage configuration
        /// </summary>
        /// <param name="targetId">The id of the target resource (a cluster id or an environment/tenant id)</param>
        /// <param name="storageConfiguration">The storage configuration to add</param>
        /// <param name="type">The type of storage configuration</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>the <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddStorageConfiguration(string targetId, StorageConfiguration storageConfiguration, StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a storage configuration
        /// </summary>
        /// <param name="id">The id of the storage configuration 'owner' - either a tenant or a cluster</param>
        /// <param name="configName">The storage configuration name</param>
        /// <param name="type">The storage configuration type - cluster or tenant</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteStorageConfiguration(string id, string configName, StorageConfigurationType type = StorageConfigurationType.EnvironmentStorageConfiguration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional document db storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional document db secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalDocDb(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional sql db storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional document db secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalSqlDb(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional redis cache storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional redis cache secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalRedisCache(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional Blob storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional Blob storage secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalBlobPrimary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional Blob storage configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional Blob storage secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalBlobSecondary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional Storage Account configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional Storage Account secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalStorageAccountPrimary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a regional Storage Account configuration to a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="keyVaultUri">The key vault uri containing the regional Storage Account secrets</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> AddClusterRegionalStorageAccountSecondary(string clusterId, string keyVaultUri, CancellationToken cancellationToken = default(CancellationToken));
    }
}
