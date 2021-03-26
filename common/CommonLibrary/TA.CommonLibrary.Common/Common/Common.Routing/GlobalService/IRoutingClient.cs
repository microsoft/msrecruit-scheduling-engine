//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Routing
{
    using System.Threading;
    using System.Threading.Tasks;
    using TA.CommonLibrary.Common.Routing.Contracts;
    using ServicePlatform.GlobalService.Contracts.Client;

    /// <summary>
    /// Routing Client Interface
    /// </summary>
    public interface IRoutingClient
    {
        /// <summary>
        /// Gets the BAP environment or AAD tenantId's cluster uri
        /// </summary>
        /// <param name="environmentId">The BAP environment ID or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The cluster uri for the BAP environment or AAD tenant<see cref="Task{String}"/></returns>
        Task<string> GetClusterUri(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a BAP environment's / AAD tenant's routing information or creates a new entry global service cluster registration entry for new BAP environments/AAD tenant ids
        /// </summary>
        /// <param name="environmentId">The environmentId to use</param>
        /// <param name="bapLocation">The bap location of the environment</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The routing information </returns>
        Task<EnvironmentRoutingInformation> PinEnvironmentToClusterInBapLocation(string environmentId, string bapLocation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that environments are pinned to their BAP locations or migrated to tenant clusters. If an environmentId is not passed in, we fall back to the default BAP environment lookup. The steps are highlighted below:
        /// 1. If environmentId is already pinned to a global service cluster, just return the cluster Uri.
        /// 2. If environmentId is new or pinToBAPLocation is true or the tenant is not pinned to any cluster, then look up the environment's bapLocation and pin that environmentId to that cluster location
        /// 3. Otherwise; look up the tenant's global service cluster and migrate the environment id to that cluster
        /// </summary>
        /// <param name="tenantId">The tenant id</param>
        /// <param name="environmentId">The enviromentId to use if specified</param>
        /// <param name="forcePinToBAPLocation">If true, the pinning will always be based off the BAP location and the tenant migration is not done</param>
        /// <param name="languageCode">The language code for localizing errors</param>
        /// <returns>The environment routing information</returns>
        Task<EnvironmentRoutingInformation> EnsureEnvironmentIsPinned(string environmentId, string tenantId, bool forcePinToBAPLocation = false, string languageCode = null);

        /// <summary>
        /// Creates a global service environment.
        /// </summary>
        /// <param name="globalServiceEnvironmentPayload">The global service environment payload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The global service environment</returns>
        Task<Environment> CreateGlobalServiceEnvironment(Environment globalServiceEnvironmentPayload, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the environment information for a BAP environment or AAD tenant Id
        /// </summary>
        /// <param name="environmentId">The environment id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{Environment}"/></returns>
        Task<Environment> GetGlobalServiceEnvironment(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the BAP environmentId or tenantId cluster mapping from the global service if it exists
        /// </summary>
        /// <param name="environmentId">The BAP environment ID or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>A <see cref="Task"/> to await</returns>
        Task DeleteGlobalServiceClusterMapping(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the global document db storage configuration
        /// </summary>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        StorageConfiguration GetGlobalDocumentDbStorageConfiguration();

        /// <summary>Gets the regional document db storage configuration for a BAP environmentId or AAD tenant id</summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="resourceName">The resource Name.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional document db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        Task<StorageConfiguration> GetRegionalDocumentDbStorageConfiguration(string environmentId, string resourceName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Gets the regional sql db storage configuration for a BAP environmentId or AAD tenant id</summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional document db <see cref="Task{StorageConfiguration}"/></returns>
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
        /// <returns>The redis cache <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetPrimaryBlobStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Blob storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The redis cache <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetSecondaryBlobStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        // <summary>
        /// Gets the regional Storage Account configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The storage account <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetStorageAccountPrimaryConnectionStringConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Storage Account configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The storage account <see cref="Task{StorageConfiguration}"/></returns>
        Task<StorageConfiguration> GetStorageAccountSecondaryConnectionStringConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Get the regional document db storage configuration information for the specified cluster Id.</summary>
        /// <param name="clusterId">The cluster Id to use</param>
        /// <param name="resourceName">The resource Name.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetRegionalDocumentDbStorageConfigurationForCluster(string clusterId, string resourceName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Get the regional sql db storage configuration information for the specified cluster Id.</summary>
        /// <param name="clusterId">The cluster Id to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetRegionalSqlDbStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the regional redis cache storage configuration information for the specified cluster id.
        /// </summary>
        /// <param name="clusterId">The cluster id to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetRegionalRedisCacheStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the regional Blob storage configuration information for the specified cluster id.
        /// </summary>
        /// <param name="clusterId">The cluster id to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetPrimaryBlobStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the regional Blob storage configuration information for the specified cluster id.
        /// </summary>
        /// <param name="clusterId">The cluster id to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetSecondaryBlobStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the regional Storage Account configuration information for the specified cluster id.
        /// </summary>
        /// <param name="clusterId">The cluster id to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetStorageAccountPrimaryConnectionStringConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the regional Storage Account configuration information for the specified cluster id.
        /// </summary>
        /// <param name="clusterId">The cluster id to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetStorageAccountSecondaryConnectionStringConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
