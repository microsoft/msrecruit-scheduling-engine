//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.Web.UserRoleProvider
{
    using Microsoft.ServiceFabric.Services.Client;
    using CommonLibrary.Common.BapClient;
    using CommonLibrary.Common.BapClient.Exceptions;
    using CommonLibrary.Common.Base.Security.V2;
    using CommonLibrary.Common.Base.ServiceContext;
    using CommonLibrary.Common.Base.Utilities;
    using CommonLibrary.Common.Web.Configuration;
    using CommonLibrary.Common.Web.Exceptions;
    using CommonLibrary.CommonDataService.Common;
    using CommonLibrary.ServicePlatform.Communication.Http;
    using CommonLibrary.ServicePlatform.Communication.Http.Routers;
    using CommonLibrary.ServicePlatform.Configuration;
    using CommonLibrary.ServicePlatform.Context;
    using CommonLibrary.ServicePlatform.Exceptions;
    using CommonLibrary.ServicePlatform.Tracing;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for user role
    /// </summary>
    public class RoleProvider : IRoleProvider
    {
        /// <summary>
        /// Talent admin permission name in BAP
        /// </summary>
        public const string AdminPermissionName = "ManageTalentEnvironmentSettings";

        /// <summary>The admin role definition name in BAP.</summary>
        public const string AdminRoleDefinitionName = "EnvironmentAdmin";

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Trace source instance
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Role provider configuration. 
        /// </summary>
        private readonly RoleProviderConfiguration roleProviderConfiguration;

        /// <summary>
        /// The hcm service context.
        /// </summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary>
        /// The client factory.
        /// </summary>
        private readonly IHttpCommunicationClientFactory clientFactory;

        /// <summary>
        /// BAP service client.
        /// </summary>
        private readonly IBapServiceClient bapServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleProvider" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager object</param>
        /// <param name="trace">Trace source object</param>
        /// <param name="hcmServiceContext">The hcm Service Context.</param>
        /// <param name="clientFactory">Client factory</param>
        /// <param name="bapServiceClient">The instnce for <see cref="IBapServiceClient"/>.</param>
        public RoleProvider(
            IConfigurationManager configurationManager,
            ITraceSource trace,
            IHCMServiceContext hcmServiceContext,
            IHttpCommunicationClientFactory clientFactory,
            IBapServiceClient bapServiceClient)
        {
            this.configurationManager = configurationManager;
            this.roleProviderConfiguration = this.configurationManager.Get<RoleProviderConfiguration>();
            this.trace = trace;
            this.hcmServiceContext = hcmServiceContext;
            this.clientFactory = clientFactory;
            this.bapServiceClient = bapServiceClient;
        }

        /// <summary>Method to identify if user has Admin role.</summary>
        /// <param name="principal">Current user principal</param>
        /// <param name="environmentId">The environment Id.</param>
        /// <returns>Flag to identify whether current user is Admin.</returns>
        [Obsolete("This IsAdmin implementation is deprecated, please use the new implementation HasAdminPermission instead", error: false)]
        public async Task<bool> IsAdmin(IHCMUserPrincipal principal, string environmentId)
        {
            Contract.CheckValue(principal, nameof(principal));
            bool isAdmin = false;

            //////if (this.roleProviderConfiguration?.EnableRoleProviderCache == true)
            //////{
            //////    var cacheRole = await this.GetCacheRoleItem(principal.TenantId, principal.ObjectId);
            //////    this.trace.TraceInformation($"UserRoleProvider: Found cached role - {cacheRole}");

            //////    if (cacheRole != null)
            //////    {
            //////        this.trace.TraceInformation($"UserRoleProvider: Enable cache is true - {cacheRole}");
            //////        isAdmin = cacheRole.Role == Role.Admin;
            //////        this.trace.TraceInformation($"UserRoleProvider: Set is admin to {isAdmin}");
            //////        return isAdmin;
            //////    }
            //////}

            isAdmin = await this.ExternalCallForAdmin(principal, environmentId);
            this.trace.TraceInformation($"UserRoleProvider: Set is admin to {isAdmin}");
            ////if (this.roleProviderConfiguration?.EnableRoleProviderCache == true)
            ////{
            ////    var donotWait = Task.Run(() => this.SetCacheRoleItem(principal.TenantId, principal.ObjectId, isAdmin ? new UserRole() { Role = Role.Admin } : new UserRole() { Role = Role.User }));
            ////}

            return isAdmin;
        }

        /// <summary>Method checks whether the user has admin permission.</summary>
        /// <param name="principal">Current user principal</param>
        /// <param name="environmentId">The environment Id.</param>
        /// <returns>Flag to identify whether current user has admin permission.</returns>
        public async Task<bool> HasAdminPermission(IHCMUserPrincipal principal, string environmentId)
        {
            Contract.CheckValue(principal, nameof(principal));
            Contract.CheckNonEmpty(environmentId, nameof(environmentId));

            bool hasAdminPermission = false;
            try
            {
                hasAdminPermission = await this.bapServiceClient.CheckUserPermission(principal.TenantId, environmentId, principal.ObjectId, AdminPermissionName);

                this.trace.TraceInformation($"UserRoleProvider: Does user has admin permission: {hasAdminPermission}");
            }
            catch (BapServiceClientException bapException)
            {
                this.trace.TraceWarning($"BapServiceClientException, RemotesServiceError: {bapException.RemoteServiceError}");
                return false;
            }

            return hasAdminPermission;
        }

        /// <summary>External call for admin check. </summary>
        /// <param name="principal">Current user principal</param>
        /// <param name="environmentId">The environment Id.</param>
        /// <returns>Flag to identify whether current user is Admin</returns>
        private async Task<bool> ExternalCallForAdmin(IHCMUserPrincipal principal, string environmentId)
        {
            bool result = false;

            try
            {
                using (var serviceClient =
                        this.clientFactory.CreateGTA(
                           new FabricServiceRouter(
                               ServicePartitionResolver.GetDefault(),
                               new FabricServiceEndpoint(new Uri(this.roleProviderConfiguration.ClusterUrl)))))
                {
                    this.trace.TraceInformation($"Role provider service{this.roleProviderConfiguration.ClusterUrl + this.roleProviderConfiguration.Path} for environment {environmentId}");
                    var request = new HttpRequestMessage(HttpMethod.Get, this.roleProviderConfiguration.Path);
                    request.Headers.Authorization = new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, principal.Token);
                    if (!string.IsNullOrWhiteSpace(environmentId))
                    {
                        this.trace.TraceInformation($"Adding environment header {environmentId}");
                        request.Headers.Add("x-ms-environment-id", environmentId);
                    }

                    request.Headers.Add("x-ms-environment-mode", ((int)this.hcmServiceContext.EnvironmentMode).ToString(CultureInfo.InvariantCulture));

                    await CommonLogger.Logger.ExecuteAsync(
                       "HcmCMMGetAdminRole",
                       async () =>
                       {
                           using (var response = await serviceClient.SendAsync(request, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false))
                           {
                               this.trace.TraceInformation($"Role provider response: {response?.StatusCode}");
                               if (response != null && response.IsSuccessStatusCode)
                               {

                                   var stringResult = await response.Content.ReadAsStringAsync();
                                   result = JsonConvert.DeserializeObject<bool>(stringResult);
                               }
                               else
                               {
                                   if (response != null)
                                   {
                                       var stringResult = string.Empty;
                                       if (response.Content != null)
                                       {
                                           stringResult = await response.Content.ReadAsStringAsync();
                                       }

                                       throw new RoleProviderRemoteServiceException(request.RequestUri.AbsoluteUri, response.StatusCode.ToString(), stringResult).EnsureTraced(this.trace);
                                   }
                                   else
                                   {
                                       throw new RoleProviderRemoteServiceException(request.RequestUri.AbsoluteUri).EnsureTraced(this.trace);
                                   }
                               }
                           }
                       });

                    return result;
                }
            }
            catch (NonSuccessHttpResponseException ex)
            {
                throw new Base.Exceptions.TransientException($"Exception during {this.roleProviderConfiguration.Path} call for admin", ex).EnsureTraced(this.trace);
            }
            catch (TimeoutException e)
            {
                throw new Base.Exceptions.TransientException($"Exception during {this.roleProviderConfiguration.Path} call for admin request timed out", e).EnsureTraced(this.trace);
            }
            catch (Exception e)
            {
                throw new Base.Exceptions.TransientException($"Exception during {this.roleProviderConfiguration.Path} call for admin request failed", e).EnsureTraced(this.trace);
            }
        }
    }
}
