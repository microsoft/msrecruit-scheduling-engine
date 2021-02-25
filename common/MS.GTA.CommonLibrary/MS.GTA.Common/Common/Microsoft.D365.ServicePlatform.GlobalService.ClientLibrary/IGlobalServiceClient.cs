//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MS.GTA.ServicePlatform.GlobalService.Contracts;
using MS.GTA.ServicePlatform.GlobalService.Contracts.Client;

namespace MS.GTA.ServicePlatform.GlobalService.ClientLibrary
{
    /// <summary>
    /// A global service client.
    /// </summary>
    public interface IGlobalServiceClient
    {
        /// <summary>
        /// Adds the environment asynchronously.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the Environment.</returns>
        Task<Environment> AddEnvironmentAsync(Environment environment, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Adds the environment asynchronously.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the Environment.</returns>
        Task<Environment> AddEnvironmentAsync(Environment environment, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task<StorageConfiguration> AddEnvironmentStorageConfigurationAsync(string environmentId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Adds the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task<StorageConfiguration> AddEnvironmentStorageConfigurationAsync(string environmentId, string authorityId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalCluster">The logical cluster.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the logical cluster.</returns>
        Task<LogicalCluster> AddLogicalClusterAsync(LogicalCluster logicalCluster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Adds the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the storage configuration.</returns>
        Task<StorageConfiguration> AddLogicalClusterStorageConfigurationAsync(string logicalClusterId, string authorityId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the storage configuration.</returns>
        Task<StorageConfiguration> AddLogicalClusterStorageConfigurationAsync(string logicalClusterId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the physical cluster asynchronously.
        /// </summary>
        /// <param name="physicalCluster">The physical cluster.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the physical cluster.</returns>
        Task<PhysicalCluster> AddPhysicalClusterAsync(PhysicalCluster physicalCluster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Soft deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SoftDeleteEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Restores the environment from a soft delete asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RestoreEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Soft deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SoftDeleteEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Restores the environment from a soft delete asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RestoreEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteEnvironmentStorageConfigurationAsync(string environmentId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Deletes the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteEnvironmentStorageConfigurationAsync(string environmentId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteLogicalClusterAsync(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Deletes the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteLogicalClusterStorageConfigurationAsync(string logicalClusterId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeleteLogicalClusterStorageConfigurationAsync(string logicalClusterId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Deletes the physical cluster asynchronously.
        /// </summary>
        /// <param name="physicalClusterId">The physical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task DeletePhysicalClusterAsync(string physicalClusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the Environment.</returns>
        Task<Environment> GetEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the Environment.</returns>
        Task<Environment> GetEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environment cname asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the cname.</returns>
        Task<string> GetEnvironmentCnameAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets the environment cname asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the cname.</returns>
        Task<string> GetEnvironmentCnameAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the storage configuration.</returns>
        Task<StorageConfiguration> GetEnvironmentStorageConfigurationAsync(string environmentId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the storage configuration.</returns>
        Task<StorageConfiguration> GetEnvironmentStorageConfigurationAsync(string environmentId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the logical cluster.</returns>
        Task<LogicalCluster> GetLogicalClusterAsync(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieves the list of logical cluster by unique cluster id.
        /// </summary>
        /// <param name="clusterId">The unique cluster Id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of logical clusters.</returns>
        Task<IList<LogicalCluster>> GetLogicalClustersByClusterIdAsync(string clusterId, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Gets the logical clusters by datacenter asynchronously.
        /// </summary>
        /// <param name="dataCenter">The data center.</param>
        /// <param name="type">The type.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of logical clusters.</returns>
        Task<IList<LogicalCluster>> GetLogicalClustersByDatacenterAsync(string dataCenter, PhysicalClusterType type = PhysicalClusterType.All, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environments corresponding to a logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of environment in logical cluster.</returns>
        Task<IList<Environment>> GetEnvironmentsByLogicalClusterAsync(string logicalClusterId, string authorityId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environments corresponding to a logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of environment in logical cluster.</returns>
        Task<IList<Environment>> GetEnvironmentsByLogicalClusterAsync(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        Task<StorageConfiguration> GetLogicalClusterStorageConfigurationAsync(string logicalClusterId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        Task<StorageConfiguration> GetLogicalClusterStorageConfigurationAsync(string logicalClusterId, string configName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets the physical cluster asynchronously.
        /// </summary>
        /// <param name="physicalClusterId">The physical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the physical cluster.</returns>
        Task<PhysicalCluster> GetPhysicalClusterAsync(string physicalClusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieves the physical cluster by cluster Id.
        /// </summary>
        /// <param name="clusterId">The unique cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the physical cluster.</returns>
        Task<PhysicalCluster> GetPhysicalClusterByClusterIdAsync(string clusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets the physical clusters by datacenter asynchronously.
        /// </summary>
        /// <param name="dataCenter">The data center.</param>
        /// <param name="type">The type.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of physical clusters.</returns>
        Task<IList<PhysicalCluster>> GetPhysicalClustersByDatacenterAsync(string dataCenter, PhysicalClusterType type, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets a namespace mapping based on the namespace id
        /// </summary>
        /// <param name="namespaceId">The namespace id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the namespace mapping.</returns>
        Task<NamespaceMapping> GetNamespaceMappingByNamespaceAsync(string namespaceId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Gets a mapping by the domain.
        /// </summary>
        /// <param name="domain">The domain or logical cluster endpoint.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the mapping.</returns>
        Task<Mapping> GetMappingByDomainAsync(string domain, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// MANAGEMENT API: Sets a mapping
        /// </summary>
        /// <param name="domain">The domain or logical cluster endpoint.</param>
        /// <param name="mapping">The mapping to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task the completes after adding the mapping.</returns>
        Task AddNamespaceMapping(string domain, Mapping mapping, CancellationToken cancellationToken = default(CancellationToken));
    }
}