//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ITenantResourceManager.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing
{
    using MS.GTA.Common.DocumentDB.Configuration;
    using MS.GTA.Common.Routing.Contracts;
    using MS.GTA.ServicePlatform.GlobalService.Contracts.Client;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary> The tenant resource manager interface </summary>
    public interface ITenantResourceManager
    {
        /// <summary>
        /// Gets the cluster uri for the current user principal.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The cluster uri string</returns>
        Task<string> GetClusterCname(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that environments are pinned to their BAP locations or migrated to tenant clusters. If an environmentId is not passed in, we fall back to the default BAP environment lookup. The steps are highlighted below:
        /// 1. If environmentId is already pinned to a global service cluster, just return the cluster Uri.
        /// 2. If environmentId is new or pinToBAPLocation is true or the tenant is not pinned to any cluster, then look up the environment's bapLocation and pin that environmentId to that cluster location
        /// 3. Otherwise; look up the tenant's global service cluster and migrate the environment id to that cluster
        /// </summary>
        /// <param name="environmentId">The enviromentId to use if specified</param>
        /// <param name="forcePinToBAPLocation">If true, the pinning will always be based off the BAP location and the tenant migration is not done</param>
        /// <returns>The environment routing information</returns>
        Task<EnvironmentRoutingInformation> EnsureEnvironmentIsPinned(string environmentId = null, bool forcePinToBAPLocation = false);

        /// <summary>
        /// Gets the global document db storage configuration
        /// </summary>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        StorageConfiguration GetGlobalDocumentDbStorageConfiguration();

        /// <summary>
        /// Gets the regional document db storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetRegionalDocumentDbStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional redis cache storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetRegionalRedisCacheStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the regional Blob storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetPrimaryBlobStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Gets the regional Blob storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        Task<StorageConfiguration> GetSecondaryBlobStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the global document db storage configuration
        /// </summary>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        DocumentDBStorageConfiguration GetGlobalRoutingDocumentDbConfiguration();

        /// <summary>
        /// Get the regional HCM document db storage configuration information for the current user principal.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        Task<DocumentDBStorageConfiguration> GetHCMDocumentDbConfiguration(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the regional user settings document db storage configuration information for the current principal.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        Task<DocumentDBStorageConfiguration> GetUserSettingsDocumentDbConfiguration(CancellationToken cancellationToken = default(CancellationToken));
    }
}
