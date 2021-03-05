//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BapClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Text;

    using Contracts;
    using Contracts.XRM;
    using CommonDataService.Common.Internal;
    using Exceptions;
    using Extensions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using ServicePlatform.Context;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Tracing;

    /// <summary>
    /// Client for performing operations on BAP environments.
    /// </summary>
    public class BapServiceClient : IBapServiceClient
    {
        /// <summary>The relative url.</summary>
        private const string RelativeUrl = "providers/Microsoft.BusinessAppPlatform/scopes";

        /// <summary>The enroll url.</summary>
        private const string EnrollUrl = "service/enroll";

        /// <summary>The environments url.</summary>
        private const string EnvironmentsUrl = "service/environments";

        /// <summary>The environments url.</summary>
        private const string ExpandNamespaces = "service/environments";

        /// <summary> The BAP http retriever. </summary>
        private readonly IBapHttpRetriever bapHttpRetriever;

        /// <summary>The logger</summary>
        private readonly ILogger<BapServiceClient> logger;

        /// <summary>
        /// A cache of provisioned default bap environments.
        /// </summary>
        private readonly ConcurrentDictionary<string, EnvironmentDefinition> provisionedDefaultBapEnvironments = new ConcurrentDictionary<string, EnvironmentDefinition>();

        /// <summary>Initializes a new instance of the <see cref="BapServiceClient"/> class.</summary>
        /// <param name="bapHttpRetriever">The BAP HTTP retriever.</param>
        /// <param name="logger">The logger</param>
        public BapServiceClient(
            IBapHttpRetriever bapHttpRetriever,
            ILogger<BapServiceClient> logger)
        {
            Contract.CheckValue(bapHttpRetriever, nameof(bapHttpRetriever));
            Contract.CheckValue(logger, nameof(logger));

            this.bapHttpRetriever = bapHttpRetriever;
            this.logger = logger;
        }

        /// <summary>Get a bap environment.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="bapEnvironmentId">The bap environment id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<EnvironmentDefinition> GetBapEnvironment(string tenantId, string bapEnvironmentId)
        {
            // Make sure we have a default environment before we try to do any calls.
            await this.EnsureDefaultBapEnvironment(tenantId);

            using (var response = await this.bapHttpRetriever.SendAsServiceAsync(
                                      this.bapHttpRetriever.AttachApiVersion(
                                          $"{RelativeUrl}/{EnvironmentsUrl}/{bapEnvironmentId}"),
                                      tenantId))
            {
                return response?.Content != null ? await response.Content.ReadAsAsync<EnvironmentDefinition>() : null;
            }
        }

        /// <summary>Get a bap environment.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="bapEnvironmentId">The bap environment id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<XRMEnvironmentDefinition> GetXRMEnvironment(string tenantId, string bapEnvironmentId)
        {
            // Make sure we have a default environment before we try to do any calls.
            await this.EnsureDefaultBapEnvironment(tenantId);

            using (var response = await this.bapHttpRetriever.SendAsServiceAsync(
                                      this.bapHttpRetriever.AttachXRMApiVersion(
                                          $"{RelativeUrl}/{EnvironmentsUrl}/{bapEnvironmentId}"),
                                      tenantId))
            {
                return response?.Content != null ? await response.Content.ReadAsAsync<XRMEnvironmentDefinition>() : null;
            }
        }

        /// <summary>Gets bap environments.</summary>
        /// <param name="tenantId">The tenant Id. If not specified the bapHttpRetriever will retrieve it from the token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<List<EnvironmentDefinition>> GetBapEnvironments(string tenantId)
        {
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));

            // Make sure we have a default environment before we try to do any calls.
            await this.EnsureDefaultBapEnvironment(tenantId);

            return await this.logger.ExecuteAsync(
                "HcmBAPV2GetApi",
                async () =>
                {
                    this.logger.LogInformation($"Getting bap environments for tenant {tenantId}");

                    var resultEnvironments = new List<EnvironmentDefinition>();
                    var url = this.bapHttpRetriever.AttachApiVersion($"{RelativeUrl}/{EnvironmentsUrl}");

                    do
                    {
                        using (var responseMessage = await this.bapHttpRetriever.SendAsServiceAsync(url, tenantId))
                        {
                            var readEnvironments = responseMessage?.Content != null ? await responseMessage?.Content?.ReadAsAsync<ResponseWithContinuation<IList<EnvironmentDefinition>>>() : null;

                            if (readEnvironments?.Value != null)
                            {
                                this.logger.LogInformation($"Received page of {readEnvironments.Value.Count} environments for tenant {tenantId}");
                                resultEnvironments.AddRange(readEnvironments.Value);
                            }

                            url = readEnvironments?.NextLink != null ? new Uri(readEnvironments?.NextLink, UriKind.Absolute).PathAndQuery : null;
                        }
                    }
                    while (url != null);

                    this.logger.LogInformation($"Returning {resultEnvironments.Count} environments for tenant {tenantId}: {JsonConvert.SerializeObject(resultEnvironments.Select(e => e.Name))}");

                    return resultEnvironments;
                });
        }

        /// <summary>Create a default bap environment async.</summary>
        /// <param name="tenantId">The tenant Id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<EnvironmentDefinition> EnsureDefaultBapEnvironment(string tenantId)
        {
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));

            return await this.logger.ExecuteAsync(
                "HcmBAPV2CreateDefaultApi",
                async () =>
                {
                    if (this.provisionedDefaultBapEnvironments.TryGetValue(tenantId, out EnvironmentDefinition defaultBapEnvironment))
                    {
                        this.logger.LogInformation($"Got default environment for tenant {tenantId} from cache");
                        return defaultBapEnvironment;
                    }

                    this.logger.LogInformation($"Creating default environment for tenant {tenantId}");

                    using (var responseMessage = await this.bapHttpRetriever.SendAsServiceAsync(
                        this.bapHttpRetriever.AttachApiVersion($"{RelativeUrl}/{EnrollUrl}"),
                        tenantId,
                        null,
                        new StringContent(string.Empty)))
                    {
                        var enrollEnvironmentsDefinition = responseMessage?.Content != null ? await responseMessage.Content.ReadAsAsync<EnrollEnvironmentsDefinition>() : null;

                        defaultBapEnvironment = enrollEnvironmentsDefinition?.DefaultEnvironment;
                        if (defaultBapEnvironment == null)
                        {
                            throw new BapServiceClientException($"Default BAP environment could not be created for tenant {tenantId}");
                        }

                        this.logger.LogInformation($"Finished creation of default environment for tenant {tenantId}: {JsonConvert.SerializeObject(defaultBapEnvironment)}");

                        if (this.provisionedDefaultBapEnvironments.TryAdd(tenantId, defaultBapEnvironment))
                        {
                            this.logger.LogInformation($"Added default environment for tenant {tenantId} to cache");
                        }

                        return defaultBapEnvironment;
                    }
                });
        }

        /// <summary>Check user has permission in given tenant and environment.</summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="bapEnvironmentId">The bap environment id.</param>
        /// <param name="userObjectId">The user object id.</param>
        /// <param name="permissionName">The permission name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<bool> CheckUserPermission(string tenantId, string bapEnvironmentId, string userObjectId, string permissionName)
        {
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));
            Contract.CheckNonEmpty(bapEnvironmentId, nameof(bapEnvironmentId));
            Contract.CheckNonEmpty(userObjectId, nameof(userObjectId));
            Contract.CheckNonEmpty(permissionName, nameof(permissionName));

            return await this.logger.ExecuteAsync(
                "HcmBAPCheckPermission",
                async () =>
                {
                    var permission = new Permission() { Name = permissionName, HasAccess = false };
                    var permissionCollection = new PermissionCollection() { AccessPermissions = new List<Permission>() { permission } };
                    using (var response = await this.bapHttpRetriever.SendAsServiceAsync(
                                        this.bapHttpRetriever.AttachXRMApiVersion(
                                            $"{RelativeUrl}/{EnvironmentsUrl}/{bapEnvironmentId}/checkaccess"),
                                        tenantId,
                                        userObjectId,
                                        new StringContent(JsonConvert.SerializeObject(permissionCollection), Encoding.UTF8, "application/json"),
                                        HttpMethod.Post,
                                        retryOnTransientErrors: true))
                    {
                        if (response?.StatusCode == HttpStatusCode.NotFound)
                        {
                            this.logger.LogWarning($"Environment {bapEnvironmentId} was not found. Will default to NOT admin.");

                            return false;
                        }
                        
                        if (response == null || (response != null && !response.IsSuccessStatusCode))
                        {
                            throw new BapServiceClientException($"Failed to check user permission {permissionName} for tenant {tenantId}, bap environment {bapEnvironmentId}");
                        }

                        if (response.Content != null)
                        {
                            permissionCollection = await response.Content.ReadAsAsync<PermissionCollection>();
                        }
                    }
                    
                    return permissionCollection.AccessPermissions.FirstOrDefault().HasAccess;
                });


        }
    }
}
