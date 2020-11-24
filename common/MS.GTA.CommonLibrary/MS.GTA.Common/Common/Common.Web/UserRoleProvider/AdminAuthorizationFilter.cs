//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AdminAuthorizationFilter.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Web.UserRoleProvider
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.ServiceFabric.Services.Client;
    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Web.Configuration;
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.Common.Web.Exceptions;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ServicePlatform.Communication.Http;
    using MS.GTA.ServicePlatform.Communication.Http.Routers;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using Newtonsoft.Json;

    /// <summary>
    /// The admin authorization handler.
    /// </summary>
    public class AdminAuthorizationFilter : ActionFilterAttribute
    {
        private int applicationRole;

        public AdminAuthorizationFilter(int applicationRole)
        {
            this.applicationRole = applicationRole;
        }
        /// <summary>The on action execution async.</summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await this.RunAuthorization(context.HttpContext);
            await next();
        }

        /// <summary>The run authorization.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task RunAuthorization(HttpContext httpContext)
        {
            Contract.CheckValue(httpContext, "Context cannot be null");
            var trace = httpContext.RequestServices.GetService(typeof(ITraceSource)) as ITraceSource;
            var configurationManager = httpContext.RequestServices.GetService(typeof(IConfigurationManager)) as IConfigurationManager;
            var configuration = configurationManager.Get<RoleProviderConfiguration>();
            Contract.CheckValue(configuration, "Configuraiton  cannot be null");
            Contract.CheckValueOrNull(configuration.Service, "Service in role provider configuraiton can not be null");
            Contract.CheckValueOrNull(configuration.EndPoint, "EndPoint in role provider configuraiton can not be null");
            await ExternalCall(httpContext, trace, configuration.Service, string.Format(configuration.EndPoint, this.applicationRole), "HcmCMMCheckAppAdmin");
        }



        /// <summary>
        /// Validate Application Admin
        /// </summary>
        /// <param name="httpContext">Http Context</param>
        /// <param name="trace">The Trace</param>
        /// <param name="url">Base url</param>
        /// <param name="path">Relative path</param>
        /// <param name="activityName">Activity Name</param>
        private async Task ExternalCall(HttpContext httpContext, ITraceSource trace, string url, string path, string activityName)
        {
            var hcmServiceContext = httpContext.RequestServices.GetService(typeof(IHCMServiceContext)) as IHCMServiceContext;
            var principalRetriever = httpContext.RequestServices.GetService(typeof(IHCMPrincipalRetriever)) as IHCMPrincipalRetriever;
            var clientFactory = httpContext.RequestServices.GetService(typeof(IHttpCommunicationClientFactory)) as IHttpCommunicationClientFactory;
            var userPrincipal = principalRetriever.Principal as IHCMUserPrincipal;
            var environmentId = hcmServiceContext.EnvironmentId;
            var token = userPrincipal.Token;
            bool result = false;

            try
            {
                using (var serviceClient =
                        clientFactory.CreateGTA(new SingleUriRouter(new Uri(url)), new HttpCommunicationClientOptions() { ThrowOnNonSuccessResponse = false }))
                {
                    trace.TraceInformation($"Role provider service{url + path} for environment {environmentId}");
                    var request = new HttpRequestMessage(HttpMethod.Get, path);
                    request.Headers.Authorization = new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, token);
                    if (!string.IsNullOrWhiteSpace(environmentId))
                    {
                        trace.TraceInformation($"Adding environment header {environmentId}");
                        request.Headers.Add("x-ms-environment-id", environmentId);
                    }

                    request.Headers.Add("x-ms-environment-mode", ((int)hcmServiceContext.EnvironmentMode).ToString(CultureInfo.InvariantCulture));

                    await CommonLogger.Logger.ExecuteAsync(
                       activityName,
                       async () =>
                       {
                           using (var response = await serviceClient.SendAsync(request, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false))
                           {
                               trace.TraceInformation($"Role provider response: {response?.StatusCode}");
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

                                       throw new RoleProviderRemoteServiceException(request.RequestUri.OriginalString, response.StatusCode.ToString(), stringResult).EnsureTraced(trace);
                                   }
                                   else
                                   {
                                       throw new RoleProviderRemoteServiceException(request.RequestUri.OriginalString).EnsureTraced(trace);
                                   }
                               }
                           }
                       });
                }
            }
            catch (NonSuccessHttpResponseException ex)
            {
                throw new Base.Exceptions.TransientException($"Exception during {path} call for admin", ex).EnsureTraced(trace);
            }
            catch (TimeoutException e)
            {
                throw new Base.Exceptions.TransientException($"Exception during {path} call for admin request timed out", e).EnsureTraced(trace);
            }

            if (!result)
            {
                throw new RoleProviderException("Current user is not Admin: Opeartion not allowed").EnsureTraced(trace);
            };
        }
    }
}
