//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Caching;
using CommonLibrary.ServicePlatform.Caching.Abstractions;
using CommonLibrary.ServicePlatform.Communication.Http;
using CommonLibrary.ServicePlatform.Communication.Http.Routers;
using CommonLibrary.ServicePlatform.GlobalService.Contracts.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GSContracts = CommonLibrary.ServicePlatform.GlobalService.Contracts.Client;

namespace CommonLibrary.ServicePlatform.GlobalService.ClientLibrary
{
    /// <summary>
    /// Implementation of global service client.
    /// </summary>
    /// <seealso cref="CommonLibrary.ServicePlatform.GlobalService.ClientLibrary.IGlobalServiceClient" />
    public abstract class GlobalServiceClient : IGlobalServiceClient
    {
        protected const string DefaultApplicationName = "GlobalService";
        protected const string ServiceName = "GlobalService";
        private const string IsPermanentFlag = "isPermanent";
        private static readonly TimeSpan VolatileExpiration = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan StableExpiration = TimeSpan.FromDays(1);

        protected readonly Func<Task<AuthenticationHeaderValue>> getAuthorizationHeaderAsync;
        protected readonly IHttpCommunicationClientFactory httpClientFactory;
        protected readonly ILogger Logger;

        private readonly ICache<GSContracts.Environment> environmentCache = new ExtendedMemoryCache<GSContracts.Environment>(StableExpiration);
        private readonly ICache<string> cnameCache = new ExtendedMemoryCache<string>(StableExpiration);
        private readonly ICache<PhysicalCluster> physicalClusterCache = new ExtendedMemoryCache<PhysicalCluster>(StableExpiration);
        private readonly ICache<PhysicalCluster> physicalClusterByClusterIdCache = new ExtendedMemoryCache<PhysicalCluster>(StableExpiration);

        private readonly ICache<Mapping> mappingCache = new ExtendedMemoryCache<Mapping>(VolatileExpiration);
        private readonly ICache<NamespaceMapping> nsMappingCache = new ExtendedMemoryCache<NamespaceMapping>(VolatileExpiration);
        private readonly ICache<StorageConfiguration> storageConfigurationCache = new ExtendedMemoryCache<StorageConfiguration>(VolatileExpiration);
        private readonly ICache<IList<PhysicalCluster>> physicalClustersByDataCenterCache = new ExtendedMemoryCache<IList<PhysicalCluster>>(VolatileExpiration);
        private readonly ICache<LogicalCluster> logicalClusterCache = new ExtendedMemoryCache<LogicalCluster>(VolatileExpiration);
        private readonly ICache<IList<LogicalCluster>> logicalClustersByDataCenterCache = new ExtendedMemoryCache<IList<LogicalCluster>>(VolatileExpiration);
        private readonly ICache<IList<LogicalCluster>> logicalClustersByClusterIdCache = new ExtendedMemoryCache<IList<LogicalCluster>>(VolatileExpiration);
        private readonly ICache<IList<GSContracts.Environment>> environmentsByLogicalClusterCache = new ExtendedMemoryCache<IList<GSContracts.Environment>>(VolatileExpiration);

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceClient"/> class.
        /// </summary>
        /// <param name="factory">The http communication client factory.</param>
        /// <param name="getAuthorizationHeaderAsync">The function to get an authorization header asynchronously.</param>
        /// <param name="logger">The logger</param>
        public GlobalServiceClient(IHttpCommunicationClientFactory factory, Func<Task<AuthenticationHeaderValue>> getAuthorizationHeaderAsync, ILogger logger)
        {
            Contract.CheckValue(factory, nameof(factory));
            Contract.CheckValue(getAuthorizationHeaderAsync, nameof(getAuthorizationHeaderAsync));
            Contract.CheckValue(logger, nameof(logger));

            this.httpClientFactory = factory;
            this.getAuthorizationHeaderAsync = getAuthorizationHeaderAsync;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the router.
        /// </summary>
        protected abstract IHttpRouter Router { get; }

        /// <summary>
        /// Adds the environment asynchronously.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the Environment.
        /// </returns>
        public async Task<GSContracts.Environment> AddEnvironmentAsync(GSContracts.Environment environment, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri("environments", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(environment), Encoding.UTF8,
                        "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newEnvironment = await response.Content.ReadAsAsync<GSContracts.Environment>();
                    await environmentCache.SetAsync(newEnvironment.Id, newEnvironment);
                    return newEnvironment;
                }
            }
        }

        /// <summary>
        /// Adds the environment asynchronously.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the Environment.
        /// </returns>
        public async Task<GSContracts.Environment> AddEnvironmentAsync(GSContracts.Environment environment, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"authority/{authorityId}/environments", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(environment), Encoding.UTF8,
                        "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newEnvironment = await response.Content.ReadAsAsync<GSContracts.Environment>();
                    await environmentCache.SetAsync(newEnvironment.Id, newEnvironment);
                    return newEnvironment;
                }
            }
        }

        /// <summary>
        /// Gets the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the Environment.
        /// </returns>
        public async Task<GSContracts.Environment> GetEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await environmentCache.GetOrAddValueAsync(environmentId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"environments/{environmentId}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<GSContracts.Environment>();
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the Environment.
        /// </returns>
        public async Task<GSContracts.Environment> GetEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await environmentCache.GetOrAddValueAsync(environmentId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"authority/{authorityId}/environments/{environmentId}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<GSContracts.Environment>();
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environment cname asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the cname.
        /// </returns>
        public async Task<string> GetEnvironmentCnameAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await cnameCache.GetOrAddValueAsync(environmentId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"environments/{environmentId}/cname", cancellationToken))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environment cname asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the cname.
        /// </returns>
        public async Task<string> GetEnvironmentCnameAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await cnameCache.GetOrAddValueAsync(environmentId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"authority/{authorityId}/environments/{environmentId}/cname", cancellationToken))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            });
        }

        /// <summary>
        /// Deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"environments/{environmentId}?{IsPermanentFlag}=true", cancellationToken))
                {
                    await EvictEnvironmentDataFromCaches(environmentId);
                }
            }
        }

        /// <summary>
        /// Soft deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task SoftDeleteEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"environments/{environmentId}?{IsPermanentFlag}=false", cancellationToken))
                {
                    await EvictEnvironmentDataFromCaches(environmentId);
                }
            }
        }

        /// <summary>
        /// Restores the environment from a soft delete asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task RestoreEnvironmentAsync(string environmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.PostAsync($"environments/{environmentId}/restore", new StringContent(string.Empty), cancellationToken))
                { }
            }
        }

        /// <summary>
        /// Deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"authority/{authorityId}/environments/{environmentId}?{IsPermanentFlag}=true", cancellationToken))
                {
                    await environmentCache.RemoveAsync(environmentId);
                }
            }
        }

        /// <summary>
        /// Soft deletes the environment asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task SoftDeleteEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"authority/{authorityId}/environments/{environmentId}?{IsPermanentFlag}=false", cancellationToken))
                {
                    await environmentCache.RemoveAsync(environmentId);
                }
            }
        }

        /// <summary>
        /// Restores the environment from a soft delete asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task RestoreEnvironmentAsync(string environmentId, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.PostAsync($"authority/{authorityId}/environments/{environmentId}/restore", new StringContent(string.Empty), cancellationToken))
                { }
            }
        }

        /// <summary>
        /// Adds the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<StorageConfiguration> AddEnvironmentStorageConfigurationAsync(string environmentId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"environments/{environmentId}/storage", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(configuration), Encoding.UTF8,"application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newConfiguration = await response.Content.ReadAsAsync<StorageConfiguration>();
                    await storageConfigurationCache.SetAsync($"{environmentId}_{newConfiguration.Name}", newConfiguration);
                    return newConfiguration;
                }
            }
        }

        /// <summary>
        /// Adds the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<StorageConfiguration> AddEnvironmentStorageConfigurationAsync(string environmentId, string authorityId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"authority/{authorityId}/environments/{environmentId}/storage", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(configuration), Encoding.UTF8, "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newConfiguration = await response.Content.ReadAsAsync<StorageConfiguration>();
                    await storageConfigurationCache.SetAsync($"{environmentId}_{newConfiguration.Name}", newConfiguration);
                    return newConfiguration;
                }
            }
        }

        /// <summary>
        /// Deletes the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteEnvironmentStorageConfigurationAsync(string environmentId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"environments/{environmentId}/storage/{configName}", cancellationToken))
                {
                    await storageConfigurationCache.RemoveAsync($"{environmentId}_{configName}");
                }
            }
        }

        /// <summary>
        /// Deletes the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteEnvironmentStorageConfigurationAsync(string environmentId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (var response = await httpClient.DeleteAsync($"authority/{authorityId}/environments/{environmentId}/storage/{configName}", cancellationToken))
                {
                    await storageConfigurationCache.RemoveAsync($"{environmentId}_{configName}");
                }
            }
        }

        /// <summary>
        /// Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        public async Task<StorageConfiguration> GetEnvironmentStorageConfigurationAsync(string environmentId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await storageConfigurationCache.GetOrAddValueAsync($"{environmentId}_{configName}", async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"environments/{environmentId}/storage/{configName}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<StorageConfiguration>();
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="environmentId">The environment identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        public async Task<StorageConfiguration> GetEnvironmentStorageConfigurationAsync(string environmentId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await storageConfigurationCache.GetOrAddValueAsync($"{environmentId}_{configName}", async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"authority/{authorityId}/environments/{environmentId}/storage/{configName}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<StorageConfiguration>();
                    }
                }
            });
        }

        /// <summary>
        /// Adds the physical cluster asynchronously.
        /// </summary>
        /// <param name="physicalCluster">The physical cluster.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the physical cluster.
        /// </returns>
        public async Task<PhysicalCluster> AddPhysicalClusterAsync(PhysicalCluster physicalCluster, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"physicalclusters", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(physicalCluster), Encoding.UTF8, "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newCluster = await response.Content.ReadAsAsync<PhysicalCluster>();
                    await physicalClusterCache.SetAsync(newCluster.Id, newCluster);
                    return newCluster;
                }
            }
        }

        /// <summary>
        /// Adds the logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalCluster">The logical cluster.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the logical cluster.
        /// </returns>
        public async Task<LogicalCluster> AddLogicalClusterAsync(LogicalCluster logicalCluster, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"logicalclusters", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(logicalCluster), Encoding.UTF8, "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newCluster = await response.Content.ReadAsAsync<LogicalCluster>();
                    await logicalClusterCache.SetAsync(newCluster.Id, newCluster);
                    return newCluster;
                }
            }
        }

        /// <summary>
        /// Gets the physical cluster asynchronously.
        /// </summary>
        /// <param name="physicalClusterId">The physical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the physical cluster.
        /// </returns>
        public async Task<PhysicalCluster> GetPhysicalClusterAsync(string physicalClusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await physicalClusterCache.GetOrAddValueAsync(physicalClusterId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"physicalclusters/{physicalClusterId}", cancellationToken))
                    {
                        var physicalCluster = await response.Content.ReadAsAsync<PhysicalCluster>();
                        if(!string.IsNullOrEmpty(physicalCluster.ClusterId))
                        {
                            await physicalClusterByClusterIdCache.SetAsync(physicalCluster.ClusterId, physicalCluster);
                        }

                        return physicalCluster;
                    }
                }
            });
        }

        /// <summary>
        /// Retrieves the physical cluster by cluster Id.
        /// </summary>
        /// <param name="clusterId">The unique cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the physical cluster.</returns>
        public async Task<PhysicalCluster> GetPhysicalClusterByClusterIdAsync(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await physicalClusterByClusterIdCache.GetOrAddValueAsync(clusterId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"physicalclusters?clusterId={clusterId}", cancellationToken))
                    {
                        var physicalCluster = await response.Content.ReadAsAsync<PhysicalCluster>();
                        await physicalClusterCache.SetAsync(physicalCluster.Id, physicalCluster);
                        return physicalCluster;
                    }
                }
            });
        }

        /// <summary>
        /// Gets the logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the logical cluster.
        /// </returns>
        public async Task<LogicalCluster> GetLogicalClusterAsync(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await logicalClusterCache.GetOrAddValueAsync(logicalClusterId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"logicalclusters/{logicalClusterId}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<LogicalCluster>();
                    }
                }
            });
        }

        /// <summary>
        /// Gets the physical clusters by datacenter asynchronously.
        /// </summary>
        /// <param name="dataCenter">The data center.</param>
        /// <param name="clusterType">The type.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the list of physical clusters.
        /// </returns>
        public async Task<IList<PhysicalCluster>> GetPhysicalClustersByDatacenterAsync(string dataCenter, Contracts.PhysicalClusterType clusterType, CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: Do we need to handle the type similar to how we do for logical clusters below?

            return await physicalClustersByDataCenterCache.GetOrAddValueAsync($"{dataCenter}_{clusterType}", async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"datacenters/{dataCenter}/physicalclusters?clusterType={clusterType.ToString()}", cancellationToken))
                    {
                        var result = await response.Content.ReadAsAsync<IList<PhysicalCluster>>();

                        foreach (var cluster in result)
                        {
                            await physicalClusterCache.SetAsync(cluster.Id, cluster);
                        }

                        return result;
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environments corresponding to a logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of environment in logical cluster.</returns>
        public async Task<IList<GSContracts.Environment>> GetEnvironmentsByLogicalClusterAsync(string logicalClusterId, string authorityId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await environmentsByLogicalClusterCache.GetOrAddValueAsync(logicalClusterId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"logicalclusters/{logicalClusterId}/authority/{authorityId}/environments", cancellationToken))
                    {
                        var result = await response.Content.ReadAsAsync<PagedList<GSContracts.Environment>>();

                        foreach (var environment in result.Items)
                        {
                            await environmentCache.SetAsync(environment.Id, environment);
                        }

                        return result.Items;
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environments corresponding to a logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of environment in logical cluster.</returns>
        public async Task<IList<GSContracts.Environment>> GetEnvironmentsByLogicalClusterAsync(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await environmentsByLogicalClusterCache.GetOrAddValueAsync(logicalClusterId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"logicalclusters/{logicalClusterId}/environments", cancellationToken))
                    {
                        var result = await response.Content.ReadAsAsync<PagedList<GSContracts.Environment>>();

                        foreach (var environment in result.Items)
                        {
                            await environmentCache.SetAsync(environment.Id, environment);
                        }

                        return result.Items;
                    }
                }
            });
        }

        /// <summary>
        /// Retrieves the list of logical cluster by unique cluster id.
        /// </summary>
        /// <param name="clusterId">The unique cluster Id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the list of logical clusters.</returns>
        public async Task<IList<LogicalCluster>> GetLogicalClustersByClusterIdAsync(string clusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await logicalClustersByClusterIdCache.GetOrAddValueAsync(clusterId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"logicalclusters?clusterId={clusterId}", cancellationToken))
                    {
                        var result = await response.Content.ReadAsAsync<PagedList<GSContracts.LogicalCluster>>();

                        foreach (var cluster in result.Items)
                        {
                            await logicalClusterCache.SetAsync(cluster.Id, cluster);
                        }

                        return result.Items;
                    }
                }
            });
        }

        /// <summary>
        /// Gets the logical clusters by datacenter asynchronously.
        /// </summary>
        /// <param name="dataCenter">The data center.</param>
        /// <param name="clusterType">The type. (App teams are not allowed to request all clusters)</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the list of logical clusters.
        /// </returns>
        public async Task<IList<LogicalCluster>> GetLogicalClustersByDatacenterAsync(string dataCenter, Contracts.PhysicalClusterType clusterType = Contracts.PhysicalClusterType.All, CancellationToken cancellationToken = default(CancellationToken))
        {
            var relativeRequestUri = $"datacenters/{dataCenter}/logicalclusters";
            if (clusterType != Contracts.PhysicalClusterType.All)
            {
                relativeRequestUri += $"?clusterType={clusterType.ToString()}";
            }

            return await logicalClustersByDataCenterCache.GetOrAddValueAsync($"{dataCenter}_{clusterType}", async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync(relativeRequestUri, cancellationToken))
                    {
                        var result = await response.Content.ReadAsAsync<IList<LogicalCluster>>();

                        foreach (var cluster in result)
                        {
                            await logicalClusterCache.SetAsync(cluster.Id, cluster);
                        }

                        return result;
                    }
                }
            });
        }

        /// <summary>
        /// Adds the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        public async Task<StorageConfiguration> AddLogicalClusterStorageConfigurationAsync(string logicalClusterId, string authorityId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"logicalclusters/{logicalClusterId}/authority/{authorityId}/storage", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(configuration), Encoding.UTF8, "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newConfiguration = await response.Content.ReadAsAsync<StorageConfiguration>();
                    await storageConfigurationCache.SetAsync($"{logicalClusterId}_{newConfiguration.Name}", newConfiguration);
                    return newConfiguration;
                }
            }
        }

        /// <summary>
        /// Adds the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        public async Task<StorageConfiguration> AddLogicalClusterStorageConfigurationAsync(string logicalClusterId, StorageConfiguration configuration, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"logicalclusters/{logicalClusterId}/storage", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(configuration), Encoding.UTF8, "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    var newConfiguration = await response.Content.ReadAsAsync<StorageConfiguration>();
                    await storageConfigurationCache.SetAsync($"{logicalClusterId}_{newConfiguration.Name}", newConfiguration);
                    return newConfiguration;
                }
            }
        }

        /// <summary>
        /// Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        public async Task<StorageConfiguration> GetLogicalClusterStorageConfigurationAsync(string logicalClusterId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await storageConfigurationCache.GetOrAddValueAsync($"{logicalClusterId}_{configName}", async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"logicalclusters/{logicalClusterId}/authority/{authorityId}/storage/{configName}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<StorageConfiguration>();
                    }
                }
            });
        }

        /// <summary>
        /// Gets the environment storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task for which the result is the storage configuration.
        /// </returns>
        public async Task<StorageConfiguration> GetLogicalClusterStorageConfigurationAsync(string logicalClusterId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await storageConfigurationCache.GetOrAddValueAsync($"{logicalClusterId}_{configName}", async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"logicalclusters/{logicalClusterId}/storage/{configName}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<StorageConfiguration>();
                    }
                }
            });
        }

        /// <summary>
        /// Deletes the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="authorityId">The authority identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteLogicalClusterStorageConfigurationAsync(string logicalClusterId, string authorityId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"logicalclusters/{logicalClusterId}/authority/{authorityId}/storage/{configName}", cancellationToken))
                {
                    await storageConfigurationCache.RemoveAsync($"{logicalClusterId}_{configName}");
                }
            }
        }

        /// <summary>
        /// Deletes the logical cluster storage configuration asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteLogicalClusterStorageConfigurationAsync(string logicalClusterId, string configName, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"logicalclusters/{logicalClusterId}/storage/{configName}", cancellationToken))
                {
                    await storageConfigurationCache.RemoveAsync($"{logicalClusterId}_{configName}");
                }
            }
        }

        /// <summary>
        /// Deletes the logical cluster asynchronously.
        /// </summary>
        /// <param name="logicalClusterId">The logical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteLogicalClusterAsync(string logicalClusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"logicalclusters/{logicalClusterId}", cancellationToken))
                {
                    await logicalClusterCache.RemoveAsync(logicalClusterId);
                }
            }
        }

        /// <summary>
        /// Deletes the physical cluster asynchronously.
        /// </summary>
        /// <param name="physicalClusterId">The physical cluster identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeletePhysicalClusterAsync(string physicalClusterId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                using (await httpClient.DeleteAsync($"physicalclusters/{physicalClusterId}", cancellationToken))
                {
                    await physicalClusterCache.RemoveAsync(physicalClusterId);
                }
            }
        }

        /// <summary>
        /// MANAGEMENT API: Gets a namespace mapping based on the namespace id
        /// </summary>
        /// <param name="namespaceId">The namespace id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the namespace mapping.</returns>
        public async Task<NamespaceMapping> GetNamespaceMappingByNamespaceAsync(string namespaceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await nsMappingCache.GetOrAddValueAsync(namespaceId, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"api/domains/lookup/{namespaceId}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<NamespaceMapping>();
                    }
                }
            });
        }

        /// <summary>
        /// MANAGEMENT API: Gets a mapping by the domain.
        /// </summary>
        /// <param name="domain">The domain or logical cluster endpoint.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for which the result is the mapping.</returns>
        public async Task<Mapping> GetMappingByDomainAsync(string domain, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await mappingCache.GetOrAddValueAsync(domain, async () =>
            {
                using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
                {
                    using (var response = await httpClient.GetAsync($"api/domains/{domain}", cancellationToken))
                    {
                        return await response.Content.ReadAsAsync<Mapping>();
                    }
                }
            });
        }

        /// <summary>
        /// MANAGEMENT API: Sets a mapping
        /// </summary>
        /// <param name="domain">The domain or logical cluster endpoint.</param>
        /// <param name="mapping">The mapping to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task the completes after adding the mapping.</returns>
        public async Task AddNamespaceMapping(string domain, Mapping mapping, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var httpClient = httpClientFactory.CreateGTA(this.Router, GetOptions(), Logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"api/domains/{domain}", UriKind.Relative))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(mapping), Encoding.UTF8, "application/json")
                };

                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    await mappingCache.SetAsync(domain, mapping);
                }
            }
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <returns>The options.</returns>
        protected HttpCommunicationClientOptions GetOptions()
        {
            return new HttpCommunicationClientOptions
            {
                CustomHandlers = new List<DelegatingHandler>
                {
                    new AuthorizationHandler(getAuthorizationHeaderAsync)
                }
            };
        }

        private async Task EvictEnvironmentDataFromCaches(string environmentId)
        {
            await environmentCache.RemoveAsync(environmentId);
            await cnameCache.RemoveAsync(environmentId);

            // TODO: Should we evict storage configs? We'd need to know all the keys associated with this environment.
        }
    }
}
