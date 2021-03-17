//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using CommonLibrary.Common.BapClient;
using CommonLibrary.Common.BapClient.Contracts;
using CommonLibrary.Common.BapClient.Exceptions;
using CommonLibrary.Common.Base.Configuration;
using CommonLibrary.Common.Base.Utilities;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.Common.Routing.Constants;
using CommonLibrary.Common.Routing.Contracts;
using CommonLibrary.Common.DocumentDB.V2;
using CommonLibrary.Common.Routing.Exceptions;
using CommonLibrary.Common.Routing.Extensions;
using CommonLibrary.Common.Contracts;
using Microsoft.Extensions.Logging;
using CommonLibrary.ServicePlatform.Communication.Http;
using CommonLibrary.ServicePlatform.Context;
using CommonLibrary.ServicePlatform.GlobalService.ClientLibrary;
using CommonLibrary.ServicePlatform.GlobalService.Contracts;
using CommonLibrary.ServicePlatform.GlobalService.Contracts.Client;
using GlobalServiceEnvironment = CommonLibrary.ServicePlatform.GlobalService.Contracts.Client.Environment;
using CommonLibrary.Common.Common.Common.Resources;

namespace CommonLibrary.Common.Routing.GlobalService
{
    /// <summary>
    /// Routing client used for making requests to the global service for cluster uris and associated resources
    /// </summary>
    public class RoutingClient : IRoutingClient
    {
        /// <summary> The HCM Authority ID. </summary>
        private readonly string authorityId;

        /// <summary>The global service client.</summary>
        private readonly IGlobalServiceClient globalServiceClient;

        /// <summary> Logger </summary>
       private readonly ILogger logger;

        /// <summary> The global document db storage configuration</summary>
        private readonly StorageConfiguration globalDocumentDbStorageConfiguration;

        /// <summary>The document database repository for the HCM Cluster to BAP location collection information.</summary>
        private readonly IDocumentDBProvider<HCMClusterToBAPLocationMapCollection> clusterToBAPLocationMappings;

        /// <summary>The document database repository for the environments information.</summary>
        private readonly IDocumentDBProvider<EnvironmentDocument> environmentRepository;

        /// <summary>The bap service client.</summary>
        private readonly IBapServiceClient bapServiceClient;

        /// <summary>Initializes a new instance of the <see cref="RoutingClient"/> class.</summary>
        /// <param name="globalServiceClient">The global service client</param>
        /// <param name="authorityId">The HCM Global service authority id</param>
        /// <param name="logger">The <see cref="ILogger"/></param>
        /// <param name="clusterToBAPLocationMappings"> The HCM cluster to BAP location mappings repository</param>
        /// <param name="bapServiceClient">The bap service client.</param>
        /// <param name="environmentRepository">The environments repository</param>
        /// <param name="globalDocumentDbStorageConfiguration">GlobalDocument db storage configuration</param>
        public RoutingClient(
            IGlobalServiceClient globalServiceClient,
            string authorityId,
            ILogger<RoutingClient> logger,
            IDocumentDBProvider<HCMClusterToBAPLocationMapCollection> clusterToBAPLocationMappings,
            IDocumentDBProvider<EnvironmentDocument> environmentRepository,
            IBapServiceClient bapServiceClient,
            StorageConfiguration globalDocumentDbStorageConfiguration)
        {
            Contract.CheckValue(globalServiceClient, nameof(globalServiceClient), "globalServiceClient should be provided");
            Contract.CheckNonEmpty(authorityId, nameof(authorityId), "HCM Authority id should be provided");
            Contract.CheckValue(logger, nameof(logger), "logger should be provided");
            Contract.CheckValue(clusterToBAPLocationMappings, nameof(clusterToBAPLocationMappings), "clusterToBAPLocationMappings should be provided");
            Contract.CheckValue(environmentRepository, nameof(environmentRepository), "environmentRepository should be provided");
            Contract.CheckValue(globalDocumentDbStorageConfiguration, nameof(globalDocumentDbStorageConfiguration), "globalDocumentDbStorageConfiguration should be provided");

            this.globalServiceClient = globalServiceClient;
            this.logger = logger;
            this.authorityId = authorityId;
            this.globalDocumentDbStorageConfiguration = globalDocumentDbStorageConfiguration;
            var docDbClientConfig = globalDocumentDbStorageConfiguration.ToGlobalRoutingDocumentDBStorageConfiguration();
            clusterToBAPLocationMappings.Build(docDbClientConfig);
            this.clusterToBAPLocationMappings = clusterToBAPLocationMappings;
            environmentRepository.Build(globalDocumentDbStorageConfiguration.ToHCMDocumentDBStorageConfiguration(), Base.Constants.EnvironmentCollectionId);
            this.environmentRepository = environmentRepository;
            this.bapServiceClient = bapServiceClient;
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
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId or tenantId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetClusterUri: Trying to retrieve cluster CNAME for environment/tenant: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmCmnRCGetCName",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                         await this.globalServiceClient.GetEnvironmentCnameAsync(environmentId, cancellationToken),
                         nameof(GetClusterUri));
                 });
        }

        /// <summary>
        /// Ensures that environments are pinned to their BAP locations or migrated to tenant clusters. If an environmentId is not passed in, we fall back to the default BAP environment lookup. The steps are highlighted below:
        /// 1. If environmentId is already pinned to a global service cluster, just return the cluster Uri.
        /// 2. If environmentId is new or pinToBAPLocation is true or the tenant is not pinned to any cluster, then look up the environment's bapLocation and pin that environmentId to that cluster location
        /// 3. Otherwise; look up the tenant's global service cluster and migrate the environment id to that cluster
        /// </summary>
        /// <param name="tenantId">The tenant id</param>
        /// <param name="environmentId">The enviromentId to use if specified</param>
        /// <param name="forcePinToBAPLocation">If true, the pinning will always be based off the BAP location and the tenant migration is not done</param>
        /// <param name="languageCode">The language code to use for localized error messages</param>
        /// <returns>The environment routing information</returns>
        public async Task<EnvironmentRoutingInformation> EnsureEnvironmentIsPinned(string environmentId, string tenantId, bool forcePinToBAPLocation = false, string languageCode = null)
        {
            var ensuredBapEnvironment = await this.EnsureBAPEnvironmentInformation(environmentId, tenantId, languageCode);
            var ensuredBapEnvironmentId = ensuredBapEnvironment.EnvironmentId;

            this.logger.LogInformation($"RoutingClient.ensureEnvironmentIsPinned: Retrieving cluster cname for environment: {ensuredBapEnvironmentId} if it already exists");
            var environmentClusterUri = await this.GetClusterUri(ensuredBapEnvironmentId);
            if (!string.IsNullOrEmpty(environmentClusterUri))
            {
                this.logger.LogInformation($"RoutingClient.ensureEnvironmentIsPinned: ClusterURI: {environmentClusterUri} found for environmentId: {ensuredBapEnvironmentId}");
                ensuredBapEnvironment.ClusterUri = environmentClusterUri;
                var environment = await this.GetGlobalServiceEnvironment(ensuredBapEnvironmentId);
                ensuredBapEnvironment.PartitionId = environment?.PartitionId;
                return ensuredBapEnvironment;
            }

            this.logger.LogInformation($"RoutingClient.ensureEnvironmentIsPinned: No registered clusters for {ensuredBapEnvironmentId}; trying to determine if it is a brand new or an old environment");
            var environmentDocument = await this.environmentRepository.GetItemAsync(ensuredBapEnvironmentId); // Check if old environment
            var tenantGlobalServiceEnvironment = await this.GetGlobalServiceEnvironment(tenantId); // Check if tenant has been pinned
            if (forcePinToBAPLocation || environmentDocument == null || tenantGlobalServiceEnvironment == null)
            {
                this.logger.LogInformation($"RoutingClient.ensureEnvironmentIsPinned: environmentId: {ensuredBapEnvironmentId} is new; will create entry based off bapLocation: {ensuredBapEnvironment.BapLocation}");
                var environmentRoutingInformation = await this.PinEnvironmentToClusterInBapLocation(ensuredBapEnvironmentId, ensuredBapEnvironment.BapLocation);
                if (tenantGlobalServiceEnvironment == null)
                {
                    // first time tenant, pin to same region as environment to avoid mismatches
                    this.logger.LogInformation($"RoutingClient.ensureEnvironmentIsPinned: tenant id: {tenantId} is not pinned to any cluster; will create entry based off bapLocation: {environmentRoutingInformation.BapLocation}");
                    await this.PinEnvironmentToClusterInBapLocation(tenantId, environmentRoutingInformation.BapLocation);
                }
                return environmentRoutingInformation;
            }
            else
            {
                this.logger.LogInformation($"RoutingClient.ensureEnvironmentIsPinned: Pinning {ensuredBapEnvironmentId} to existing tenantId: {tenantId} global service environment with id:{tenantGlobalServiceEnvironment.PartitionId} and partition url: {tenantGlobalServiceEnvironment.PartitionUrl}");
                var clusterId = tenantGlobalServiceEnvironment.PartitionId;
                var globalServiceEnvironmentPayload = new GlobalServiceEnvironment
                {
                    Id = ensuredBapEnvironmentId,
                    PartitionId = clusterId,
                    Rights = new EnvironmentRights
                    {
                        Owner = new Authority
                        {
                            Id = this.authorityId
                        }
                    }
                };

                var globalServiceEnvironment = await this.CreateGlobalServiceEnvironment(globalServiceEnvironmentPayload);
                ensuredBapEnvironment.ClusterUri = globalServiceEnvironment.PartitionUrl;
                ensuredBapEnvironment.PartitionId = globalServiceEnvironment.PartitionId;
                return ensuredBapEnvironment;
            }
        }

        /// <summary>
        /// Creates a global service environment with the associated payload
        /// </summary>
        /// <param name="globalServiceEnvironmentPayload">The global service environment to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The <see cref="Task{GlobalServiceEnvironment}"/></returns>
        public async Task<GlobalServiceEnvironment> CreateGlobalServiceEnvironment(GlobalServiceEnvironment globalServiceEnvironmentPayload, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSCreateGSEnv",
                 async () =>
                 {
                     this.logger.LogInformation($"RoutingClient.PinEnvironmentToClusterRegion: Setting up environment: {globalServiceEnvironmentPayload.Id}");
                     return await this.globalServiceClient.AddEnvironmentAsync(globalServiceEnvironmentPayload, cancellationToken);
                 });
        }

        /// <summary>
        /// Gets a BAP environment's routing information or creates a new entry global service cluster registration entry for new users
        /// </summary>
        /// <param name="environmentId">The environmentId to pin to a cluster in the BAP location</param>
        /// <param name="bapLocation">The BAP location to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The routing information </returns>
        public async Task<EnvironmentRoutingInformation> PinEnvironmentToClusterInBapLocation(string environmentId, string bapLocation, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }

            var regionInfo = await this.GetClusterToBapLocationInformation(bapLocation);
            var region = regionInfo.Regions.Random<string>();
            var bapLocationName = regionInfo.BapLocation;
            var logicalCluster = await this.PinEnvironmentToClusterRegion(environmentId, region, cancellationToken);
            var clusterUri = logicalCluster?.PublicUri?.ToString();

            this.logger.LogInformation($"RoutingClient.PinEnvironmentToClusterInBapLocation: Successfully retrieved cluster: {clusterUri} for environmentId: {environmentId} in BAP location: {bapLocationName}");
            return new EnvironmentRoutingInformation
            {
                ClusterUri = clusterUri,
                BapLocation = bapLocationName,
                EnvironmentId = environmentId,
                PartitionId = logicalCluster?.Id
            };
        }

        /// <summary>
        /// Gets the environment information for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment ID or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{GlobalServiceEnvironment}"/></returns>
        public async Task<GlobalServiceEnvironment> GetGlobalServiceEnvironment(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetGlobalServiceEnvironment: Trying to get global service environment information for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmCmnRCGetGSEnv",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetEnvironmentAsync(environmentId, cancellationToken),
                        nameof(GetGlobalServiceEnvironment));
                 });
        }

        /// <summary>
        /// Deletes the BAP environmentId or tenantId cluster mapping from the global service if it exists
        /// </summary>
        /// <param name="environmentId">The BAP environment ID or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>A <see cref="Task"/> to await</returns>
        public async Task DeleteGlobalServiceClusterMapping(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - id is empty");
            }
            this.logger.LogInformation($"RoutingClient.DeleteGlobalServiceClusterMapping: Trying to remove id: {environmentId} from global service");

            await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSDelTenClstr",
                 async () =>
                 {
                     var clientCluster = await this.GetClusterUri(environmentId, cancellationToken);
                     if (clientCluster != null)
                     {
                         this.logger.LogInformation($"RoutingClient.DeleteGlobalServiceClusterMapping: Client cluster record found; proceeding to remove id: {environmentId} from global service");
                         await this.globalServiceClient.DeleteEnvironmentAsync(environmentId, cancellationToken);
                     }
                 });
        }

        /// <summary>
        /// Gets the global document db storage configuration
        /// </summary>
        /// <returns>The global document db <see cref="StorageConfiguration"/></returns>
        public StorageConfiguration GetGlobalDocumentDbStorageConfiguration()
        {
            this.logger.LogInformation($"RoutingClient.GetGlobalDocumentDbStorageConfigurationForTenant: Returning global document db storage configuration");
            return this.globalDocumentDbStorageConfiguration;
        }

        /// <summary>
        /// Gets the regional document db storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional document db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        public async Task<StorageConfiguration> GetRegionalDocumentDbStorageConfiguration(string environmentId, string resourceName, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId/tenantId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetRegionalDocumentDbStorageConfiguration: Trying to regional document db storage configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRDocDbCnfg",
                 async () =>
                 {
                     var globalServiceEnvironment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (globalServiceEnvironment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetRegionalDocumentDbStorageConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetRegionalDocumentDbStorageConfigurationForCluster(globalServiceEnvironment.PartitionId, resourceName, cancellationToken);
                 });
        }

        /// <summary>
        /// Gets the regional sql db storage configuration for a BAP environmentId or AAD tenantid
        /// </summary>
        /// <param name="environmentId">The BAP environment or AAD tenant id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional sql db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        public async Task<StorageConfiguration> GetRegionalSqlDbStorageConfiguration(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(environmentId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId/tenantId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetRegionalSqlDbStorageConfiguration: Trying to regional sql db storage configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRSqlDbEnvCnfg",
                 async () =>
                 {
                     var globalServiceEnvironment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (globalServiceEnvironment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetRegionalSqlDbStorageConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetRegionalSqlDbStorageConfigurationForCluster(globalServiceEnvironment.PartitionId, cancellationToken);
                 });
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
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetRegionalRedisCacheStorageConfiguration: Trying to regional redis cache storage configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRRedisCnfg",
                 async () =>
                 {
                     var environment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (environment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetRegionalRedisCacheStorageConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetRegionalRedisCacheStorageConfigurationForCluster(environment.PartitionId, cancellationToken);
                 });
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
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetPrimaryBlobStorageConfiguration: Trying to regional Blob storage configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRBlobCnfg",
                 async () =>
                 {
                     var environment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (environment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetPrimaryBlobStorageConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetPrimaryBlobStorageConfigurationForCluster(environment.PartitionId, cancellationToken);
                 });
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
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetSecondaryBlobStorageConfiguration: Trying to regional Blob storage configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRBlobCnfgClus",
                 async () =>
                 {
                     var environment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (environment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetSecondaryBlobStorageConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetSecondaryBlobStorageConfigurationForCluster(environment.PartitionId, cancellationToken);
                 });
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
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetPrimaryStorageAccountConfiguration: Trying to regional Storage Account configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRStorageAcntPrimaryCnfg",
                 async () =>
                 {
                     var environment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (environment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetPrimaryStorageAccountConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetStorageAccountPrimaryConnectionStringConfigurationForCluster(environment.PartitionId, cancellationToken);
                 });
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
                throw new GlobalServiceInvalidOperationException($"Invalid operation - environmentId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetSecondaryStorageAccountConfiguration: Trying to regional Storage Account configuration for id: {environmentId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRStorageAcntSecondaryCnfg",
                 async () =>
                 {
                     var environment = await this.GetGlobalServiceEnvironment(environmentId, cancellationToken);
                     if (environment == null)
                     {
                         this.logger.LogInformation($"RoutingClient.GetSecondaryStorageAccountConfiguration: No global service environment found for id: {environmentId}; returning null storage configuration");
                         return null;
                     }
                     return await this.GetStorageAccountSecondaryConnectionStringConfigurationForCluster(environment.PartitionId, cancellationToken);
                 });
        }

        /// <summary>
        /// Gets the regional document db storage configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="resourceName">The resource name</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional document db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        public async Task<StorageConfiguration> GetRegionalDocumentDbStorageConfigurationForCluster(string clusterId, string resourceName, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetRegionalDocumentDbStorageConfigurationForCluster: Trying to regional document db storage configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRDocDbStorCnfg",
                 async () =>
                     {
                         var configName = StorageConfigurationSettings.HCMRegionalDocumentDbStorageConfigName;
                         if (!string.IsNullOrEmpty(resourceName))
                         {
                             configName = resourceName;
                         }

                         this.logger.LogInformation($"RoutingClient.GetRegionalDocumentDbStorageConfigurationForCluster: Trying to get regional document db storage configuration for cluster: {clusterId}, resource {resourceName}");

                         return await this.MakeSafeCall(async () =>
                            await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, configName, cancellationToken),
                            nameof(GetRegionalDocumentDbStorageConfigurationForCluster));
                 });
        }

        /// <summary>
        /// Gets the regional sql db storage configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The regional sql db <see cref="Task{StorageConfiguration}"/></returns>
        /// <returns></returns>
        public async Task<StorageConfiguration> GetRegionalSqlDbStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetRegionalSqlDbStorageConfigurationForCluster: Trying to regional sql db storage configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRSqlDbClusterCnfg",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, StorageConfigurationSettings.HCMRegionalSqlDbStorageConfigName, cancellationToken),
                        nameof(GetRegionalSqlDbStorageConfigurationForCluster));
                 });
        }

        /// <summary>
        /// Gets the regional redis cache storage configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The redis cache <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetRegionalRedisCacheStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetRegionalRedisCacheStorageConfigurationForCluster: Trying to regional redis cache storage configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRRedisCnfg",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, StorageConfigurationSettings.HCMRegionalRedisCacheStorageConfigName, cancellationToken),
                        nameof(GetRegionalRedisCacheStorageConfigurationForCluster));
                 });
        }

        /// <summary>
        /// Gets the regional Blob storage configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Blob Storage <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetPrimaryBlobStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetPrimaryBlobStorageConfigurationForCluster: Trying to regional Blob storage configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRStorageCnfg",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, StorageConfigurationSettings.HCMPrimaryBlobStorageConfigName, cancellationToken),
                        nameof(GetPrimaryBlobStorageConfigurationForCluster));
                 });
        }

        /// <summary>
        /// Gets the regional Blob storage configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Blob Storage <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetSecondaryBlobStorageConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetSecondaryBlobStorageConfigurationForCluster: Trying to regional Blob storage configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRStrCnfg",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, StorageConfigurationSettings.HCMSecondaryBlobStorageConfigName, cancellationToken),
                        nameof(GetSecondaryBlobStorageConfigurationForCluster));
                 });
        }

        /// <summary>
        /// Gets the regional Storage Account configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Storage Account <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetStorageAccountPrimaryConnectionStringConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetPrimaryStorageAccountConfigurationForCluster: Trying to regional Storage Account configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRStorageAcntPrimaryCnfgForCluster",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, StorageConfigurationSettings.HCMStorageAccountPrimaryConnectionStringConfigName, cancellationToken),
                        nameof(GetStorageAccountPrimaryConnectionStringConfigurationForCluster));
                 });
        }

        /// <summary>
        /// Gets the regional Storage Account configuration for a cluster
        /// </summary>
        /// <param name="clusterId">The cluster id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The Storage Account <see cref="Task{StorageConfiguration}"/></returns>
        public async Task<StorageConfiguration> GetStorageAccountSecondaryConnectionStringConfigurationForCluster(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clusterId))
            {
                throw new GlobalServiceInvalidOperationException($"Invalid operation - clusterId is empty");
            }
            this.logger.LogInformation($"RoutingClient.GetSecondaryStorageAccountConfigurationForCluster: Trying to regional Storage Account configuration for cluster: {clusterId}");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmRCRStorageAcntSecondaryCnfgForCluster",
                 async () =>
                 {
                     return await this.MakeSafeCall(async () =>
                        await this.globalServiceClient.GetLogicalClusterStorageConfigurationAsync(clusterId, StorageConfigurationSettings.HCMStorageAccountSecondaryConnectionStringConfigName, cancellationToken),
                        nameof(GetStorageAccountSecondaryConnectionStringConfigurationForCluster));
                 });
        }


        /// <summary>
        /// Ensures a BAP environment exists. Either retrieves the BAP environment for a given tenant/environmentId or creates a default one for the tenant
        /// </summary>
        /// <param name="environmentId">The environment id</param>
        /// <param name="tenantId">The tenant Id</param>
        /// <param name="languageCode">The language code for localization</param>
        /// <returns></returns>
        private async Task<EnvironmentRoutingInformation> EnsureBAPEnvironmentInformation(string environmentId, string tenantId, string languageCode = null)
        {
            EnvironmentDefinition resolvedBapEnvironment = null;
            if (string.IsNullOrEmpty(environmentId))
            {
                this.logger.LogInformation($"RoutingClient.ResolveBAPEnvironmentInformation: EnvironmentId is null, trying to retrieve the default BAP environment for tenant: {tenantId}");
                var defaultBapEnvironment = await this.bapServiceClient.EnsureDefaultBapEnvironment(tenantId);
                var errorMessageTemplate = this.GetLocalizedErrorMessageTemplate("BapClient_InvalidOperation_FailedToRetrieveDefaultEnvironmentForTenant", languageCode);
                resolvedBapEnvironment = defaultBapEnvironment ?? throw new BapClientInvalidOperationException(string.Format(errorMessageTemplate, tenantId)); ;
            }
            else
            {
                this.logger.LogInformation($"RoutingClient.ResolveBAPEnvironmentInformation: EnvironmentId : {environmentId} is set, trying to retrieve BAP environment details: {tenantId}");
                var bapEnvironment = await this.bapServiceClient.GetBapEnvironment(tenantId, environmentId);
                var errorMessageTemplate = this.GetLocalizedErrorMessageTemplate("BapClient_InvalidOperation_FailedToRetrieveEnvironment", languageCode);
                resolvedBapEnvironment = bapEnvironment ?? throw new BapClientInvalidOperationException(string.Format(errorMessageTemplate, environmentId));
            }

            return new EnvironmentRoutingInformation()
            {
                BapLocation = resolvedBapEnvironment.Location,
                EnvironmentId = resolvedBapEnvironment.Name
            };
        }

        /// <summary>
        /// Gets the BAP location cluster mapping information
        /// </summary>
        /// <param name="bapLocation">The BAP location</param>
        /// <returns>The country information</returns>
        private async Task<HCMClusterToBAPLocationMap> GetClusterToBapLocationInformation(string bapLocation)
        {
            var clusterToBAPlocationMappings = await this.clusterToBAPLocationMappings.GetPartitionedItemAsync(StorageConfigurationSettings.HCMClusterToBAPLocationMappingsDocumentDbKey);
            if (clusterToBAPlocationMappings == null)
            {
                throw new GlobalServiceMappingsRetrievalException("RoutingClient.GetClusterToBapLocationInformation: Could not find the cluster-BAPLocation mapping information entry document in document db.");
            }

            var clusterToBAPLocationInformation = clusterToBAPlocationMappings.Mappings.FirstOrDefault(clusterToBapLocationMap =>
            string.Equals(clusterToBapLocationMap.BapLocation, bapLocation, StringComparison.InvariantCultureIgnoreCase));
            if (clusterToBAPLocationInformation != null && clusterToBAPLocationInformation.Enabled)
            {
                this.logger.LogInformation($"RoutingClient.GetClusterToBapLocationInformation: Successfully retrieved cluster mapping information for bap location: {bapLocation}");
                return clusterToBAPLocationInformation;
            }

            this.logger.LogInformation($"RoutingClient.GetClusterToBapLocationInformation: Bap Location: {bapLocation} is not enabled; will default to fall back bap location");
            var fallBackBAPLocation = StorageConfigurationSettings.HCMDefaultBAPLocation;
            this.logger.LogInformation($"RoutingClient.GetClusterToBapLocationInformation: BAP location: {bapLocation} is either not in the document db or not yet enabled; falling back to BAP Location: {fallBackBAPLocation}");
            return await this.GetClusterToBapLocationInformation(fallBackBAPLocation);
        }

        /// <summary>
        /// Pins an environment id to a cluster in the specified region
        /// </summary>
        /// <param name="environmentId">The environment's id</param>
        /// <param name="region">The region to register the environment in</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{GlobalServiceEnvironment}"/></returns>
        private async Task<LogicalCluster> PinEnvironmentToClusterRegion(
            string environmentId,
            string region,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(environmentId, nameof(environmentId), "environmentId should be defined");
            Contract.CheckNonEmpty(region, nameof(region), "region should be defined");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSPinEnvToClstr",
                 async () =>
                 {
                     this.logger.LogInformation($"RoutingClient.PinEnvironmentToClusterRegion: Setting up environment: {environmentId} in region: {region}");
                     var selectedCluster = await this.SelectRandomLogicalClusterFromRegion(region);
                     var environment = new GlobalServiceEnvironment
                     {
                         Id = environmentId,
                         PartitionId = selectedCluster.Id,
                         Rights = new EnvironmentRights
                         {
                             Owner = new Authority
                             {
                                 Id = this.authorityId
                             }
                         }
                     };

                     var registeredEnvironment = await this.CreateGlobalServiceEnvironment(environment, cancellationToken);
                     this.logger.LogInformation($"RoutingClient.PinEnvironmentToClusterRegion: Successfully created environment for user with id {registeredEnvironment.Id}");

                     return selectedCluster;
                 });
        }

        /// <summary>
        /// Selects a random cluster from an Azure region
        /// </summary>
        /// <param name="region">The region to use</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{LogicalCluster}"/></returns>
        private async Task<LogicalCluster> SelectRandomLogicalClusterFromRegion(string region, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(region, nameof(region), "region should be defined");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSGetRdnClst",
                 async () =>
                 {
                     this.logger.LogInformation($"RoutingClient.SelectRandomLogicalClusterFromRegion: Selecting random logical cluster in region: {region}");
                     var logicalClusters = await this.GetLogicalClustersInRegion(region, cancellationToken);
                     return logicalClusters.Random();
                 });
        }

        /// <summary>
        /// Selects all the clusters in an Azure region
        /// </summary>
        /// <param name="region">The region to use</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{LogicalCluster}"/></returns>
        private async Task<IList<LogicalCluster>> GetLogicalClustersInRegion(string region, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.CheckNonEmpty(region, nameof(region), "region should be defined");

            return await CommonLogger.Logger.ExecuteAsync(
                 "HcmGSGetRdnClst",
                 async () =>
                 {
                     this.logger.LogInformation($"RoutingClient.GetLogicalClustersInRegion: Retrieving logical clusters in region: {region}");
                     return await this.globalServiceClient.GetLogicalClustersByDatacenterAsync(region, PhysicalClusterType.Dynamics365, cancellationToken);
                 });
        }

        /// <summary>
        /// Ensures that the benign errors from the global service client are safely handled.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="globalServiceCall">The global service function to call</param>
        /// <param name="callingFunction">The name of the calling function for logging purposes</param>
        /// <returns>The <see cref="Task{T}"/> </returns>
        private async Task<T> MakeSafeCall<T>(Func<Task<T>> globalServiceCall, string callingFunction)
        {
            Contract.CheckValue(globalServiceCall, nameof(globalServiceCall));
            Contract.CheckNonEmpty(callingFunction, nameof(callingFunction));

            try
            {
                return await globalServiceCall();
            }
            catch (NonSuccessHttpResponseException ex)
            {
                if (ex.RemoteStatusCode == HttpStatusCode.NotFound)
                {
                    logger.LogInformation($"GlobalServiceClient.{callingFunction}: got NotFound response code; returning null");
                    return default(T);
                }
                throw new GlobalServiceException($"GlobalServiceClient.{callingFunction}: Non-success response from Global Service {ex.RemoteStatusCode} ({ex.RemoteReasonPhrase}) {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the localized error message template
        /// </summary>
        /// <param name="resourceString">The error key message</param>
        /// <param name="languageCode">The language code</param>
        /// <returns>The error message template</returns>
        private string GetLocalizedErrorMessageTemplate(string resourceString, string languageCode = null)
        {
            languageCode = languageCode ?? SupportedLanguagesConfiguration.DefaultLanguage;
            var cultureInfo = SupportedLanguagesConfiguration.GetCultureInfo((string)languageCode);
            return CommonStrings.ResourceManager.GetString(resourceString, cultureInfo);
        }
    }
}
