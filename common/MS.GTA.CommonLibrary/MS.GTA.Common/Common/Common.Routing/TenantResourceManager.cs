//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Routing
{
    using Microsoft.AspNetCore.Http;
    using CommonDataService.Common.Internal;
    using Common.Base.Security;
    using Common.Base.ServiceContext;
    using Common.DocumentDB.Configuration;
    using Common.Routing.Contracts;
    using Common.Routing.Exceptions;
    using Common.Routing.Extensions;
    using ServicePlatform.GlobalService.Contracts.Client;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary> The tenant resource manager </summary>
    public class TenantResourceManager : ITenantResourceManager
    {

        /// <summary> The tenantId of the principal on the scoped request</summary>
        private readonly string tenantId;

        /// <summary> The environmentId of the principal on the scoped request</summary>
        private readonly string environmentId;

        /// <summary> The global document db storage configuration for the tenant</summary>
        private StorageConfiguration globalDocDbStorageConfiguration;

        /// <summary>The routing client</summary>
        private readonly IRoutingClient routingClient;

        /// <summary> Logger </summary>
        private readonly ILogger logger;

        /// <summary>Initializes a new instance of the <see cref="TenantResourceManager"/> class.</summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="routingClient"></param>
        /// <param name="bapServiceClient">The bap service client.</param>
        /// <param name="hcmServiceContext">The HCM Service Context.</param>
        /// <param name="logger">The <see cref="ILogger"/></param>
        public TenantResourceManager(
            IHttpContextAccessor httpContextAccessor,
            IRoutingClient routingClient,
            IHCMServiceContext hcmServiceContext,
            ILogger<TenantResourceManager> logger)
        {
            Contract.CheckValue(httpContextAccessor, nameof(httpContextAccessor), "httpContextAccessor should be provided");
            Contract.CheckValue(routingClient, nameof(routingClient), "routingClient should be provided");
            Contract.CheckValue(logger, nameof(logger), "logger should be provided");

            var principal = new HCMApplicationPrincipal(httpContextAccessor.HttpContext);
            this.tenantId = this.GetPrincipalTenantId(principal);
            this.environmentId = hcmServiceContext.EnvironmentId;
            this.routingClient = routingClient;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the cluster uri for the current user principal.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The cluster uri string</returns>
        public async Task<string> GetClusterCname(CancellationToken cancellationToken = default(CancellationToken))
        {
            var environmentRoutingInformation = await this.EnsureEnvironmentIsPinned();
            this.logger.LogInformation($"TenantResourceManager.GetClusterCname: Retrieved cluster cname: {environmentRoutingInformation.ClusterUri} for environmentId: {environmentRoutingInformation.EnvironmentId}");
            return environmentRoutingInformation.ClusterUri;
        }

        /// <summary>
        /// Ensures that environments are pinned to their BAP locations or migrated to tenant clusters. If an environmentId is not passed in, we fall back to the default BAP environment lookup. The steps are highlighted below:
        /// 1. If environmentId is already pinned to a global service cluster, just return the cluster Uri.
        /// 2. If environmentId is new or pinToBAPLocation is true or the tenant is not pinned to any cluster, then look up the environment's bapLocation and pin that environmentId to that cluster location
        /// 3. Otherwise; look up the tenant's global service cluster and migrate the environment id to that cluster
        /// </summary>
        /// <param name="environmentId">The enviromentId to use if specified</param>
        /// <param name="forcePinToBAPLocation">If true, the pinning will always be based off the BAP location and the tenant migration is not done</param>
        /// <returns>The environment routing information</returns>
        public async Task<EnvironmentRoutingInformation> EnsureEnvironmentIsPinned(string environmentId = null, bool forcePinToBAPLocation = false)
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                this.logger.LogInformation("TenantResourceManager.ensureEnvironmentIsPinned: EnvironmentId not specified, using environment from HCMContext");
                environmentId = this.environmentId;
            }

            return await this.routingClient.EnsureEnvironmentIsPinned(environmentId, this.tenantId, forcePinToBAPLocation);
        }

        /// <summary>
        /// Gets the global document db storage configuration
        /// </summary>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public StorageConfiguration GetGlobalDocumentDbStorageConfiguration()
        {
            this.logger.LogInformation($"TenantResourceManager.GetGlobalDocumentDbStorageConfiguration: Retrieving global document db storage configuration for tenantId: {this.tenantId}");
            if (this.globalDocDbStorageConfiguration == null)
            {
                this.globalDocDbStorageConfiguration = this.routingClient.GetGlobalDocumentDbStorageConfiguration();
            }

            return this.globalDocDbStorageConfiguration;
        }

        /// <summary>
        /// Get the global document db storage configuration
        /// </summary>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        public DocumentDBStorageConfiguration GetGlobalRoutingDocumentDbConfiguration()
        {
            this.logger.LogInformation($"TenantResourceManager.GetGlobalRoutingDocumentDbConfiguration: Getting global routing document db configuration for tenantId: {this.tenantId}");
            var storageConfig = this.GetGlobalDocumentDbStorageConfiguration();
            return storageConfig.ToGlobalRoutingDocumentDBStorageConfiguration();
        }

        /// <summary>
        /// Gets the regional document db storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public async Task<StorageConfiguration> GetRegionalDocumentDbStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"TenantResourceManager.GetRegionalDocumentDbStorageConfiguration: MigrateToEnvironmentsPath active so getting regional document db storage configuration for environmentId: {this.environmentId}");
            var environmentRoutingInformation = await this.EnsureEnvironmentIsPinned();
            return await this.routingClient.GetRegionalDocumentDbStorageConfiguration(environmentRoutingInformation.EnvironmentId, string.Empty, cancellationToken);
        }

        /// <summary>
        /// Gets the regional redis cache storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public async Task<StorageConfiguration> GetRegionalRedisCacheStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"TenantResourceManager.GetRegionalRedisCacheStorageConfiguration: MigrateToEnvironmentsPath active so getting regional redis cache storage configuration for environmentId: {this.environmentId}");
            var environmentRoutingInformation = await this.EnsureEnvironmentIsPinned();
            return await this.routingClient.GetRegionalRedisCacheStorageConfiguration(environmentRoutingInformation.EnvironmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional Blob storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public async Task<StorageConfiguration> GetPrimaryBlobStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"TenantResourceManager.GetPrimaryBlobStorageConfiguration: MigrateToEnvironmentsPath active so getting regional blob storage configuration for environmentId: {this.environmentId}");
            var environmentRoutingInformation = await this.EnsureEnvironmentIsPinned();
            return await this.routingClient.GetPrimaryBlobStorageConfiguration(environmentRoutingInformation.EnvironmentId, cancellationToken);
        }

        /// <summary>
        /// Gets the regional Blob storage configuration for the current user principal
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public async Task<StorageConfiguration> GetSecondaryBlobStorageConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"TenantResourceManager.GetSecondaryBlobStorageConfiguration: MigrateToEnvironmentsPath active so getting regional blob storage configuration for environmentId: {this.environmentId}");
            var environmentRoutingInformation = await this.EnsureEnvironmentIsPinned();
            return await this.routingClient.GetSecondaryBlobStorageConfiguration(environmentRoutingInformation.EnvironmentId, cancellationToken);
        }

        /// <summary>
        /// Get the regional HCM document db storage configuration information for the current user principal.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        public async Task<DocumentDBStorageConfiguration> GetHCMDocumentDbConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"TenantResourceManager.GetHCMDocumentDbConfiguration: Getting regional HCM database configuration for tenantId: {this.tenantId}");
            var storageConfig = await this.GetRegionalDocumentDbStorageConfiguration();
            if (storageConfig == null)
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - storage configuration is null for tenant: {this.tenantId}");
            }
            return storageConfig.ToHCMDocumentDBStorageConfiguration();
        }

        /// <summary>
        /// Get the regional user settings document db storage configuration information for the current principal.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        public async Task<DocumentDBStorageConfiguration> GetUserSettingsDocumentDbConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation($"TenantResourceManager.GetUserSettingsDocumentDbConfiguration: Getting regional UserSettings database configuration for tenantId: {this.tenantId}");
            var storageConfig = await this.GetRegionalDocumentDbStorageConfiguration();
            if (storageConfig == null)
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - storage configuration is null for tenant: {this.tenantId}");
            }
            return storageConfig.ToUserSettingsDocumentDBStorageConfiguration();
        }

        /// <summary>
        /// Gets the current HCM principal tenant id
        /// </summary>
        /// <returns>The HCM Principal tenant id</returns>
        private string GetPrincipalTenantId(IHCMApplicationPrincipal principal)
        {
            Contract.CheckValue(principal, nameof(principal));

            var tenantId = !string.IsNullOrEmpty(principal.TenantObjectId) ?
                principal.TenantObjectId : principal.TenantId;
            return tenantId;
        }
    }
}
