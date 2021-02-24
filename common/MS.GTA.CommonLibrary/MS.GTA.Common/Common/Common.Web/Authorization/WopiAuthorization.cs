//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Web.Authorization
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using Microsoft.Extensions.Logging;

    /// <summary>The WOPI authorization.</summary>
    public class WopiAuthorization
    {
        /// <summary>The logger.</summary>
        private static ILogger Logger => CommonLogger.Logger;

        /// <summary>The run authorization.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task RunAuthorization(HttpContext httpContext)
        {
            var trace = httpContext.RequestServices.GetService(typeof(ITraceSource)) as ITraceSource;

            Logger.Execute(
                "WopiAuthorization",
                () =>
                    {
                        if (httpContext.Request?.Query != null && httpContext.Request.Query.ContainsKey("access_token"))
                        {
                            var hcmServiceContext = httpContext.RequestServices.GetService(typeof(IHCMServiceContext)) as IHCMServiceContext;

                            var accessToken = httpContext.Request.Query["access_token"];

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var readToken = tokenHandler.ReadJwtToken(accessToken);

                            hcmServiceContext.TenantId = readToken.Claims?.FirstOrDefault(c => c.Type == "tid")?.Value;
                            hcmServiceContext.EnvironmentId = readToken.Claims?.FirstOrDefault(c => c.Type == "environmentid")?.Value;
                            hcmServiceContext.ObjectId = readToken.Claims?.FirstOrDefault(c => c.Type == "oid")?.Value;

                            trace.TraceInformation($"Finished wopi authorization, request setup with tenant {hcmServiceContext.TenantId} environment {hcmServiceContext.EnvironmentId}");
                        }
                        else
                        {
                            trace.TraceWarning("Unable to run WOPI authorization, missing access token");
                        }
                    });
        }

        /// <summary>The WOPI authorization middleware.</summary>
        public class WopiAuthorizationMiddleware
        {
            /// <summary>The configure.</summary>
            /// <param name="applicationBuilder">The application builder.</param>
            public void Configure(IApplicationBuilder applicationBuilder)
            {
                applicationBuilder.Use(
                    async (httpContext, func) =>
                        {
                            await RunAuthorization(httpContext);
                            await func();
                        });
            }
        }

        /// <summary>The WOPI authorization filter.</summary>
        public class WopiAuthorizationFilter : ActionFilterAttribute
        {
            /// <summary>The on action execution async.</summary>
            /// <param name="context">The context.</param>
            /// <param name="next">The next.</param>
            /// <returns>The <see cref="Task"/>.</returns>
            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await RunAuthorization(context.HttpContext);
                await next();
            }
        }
    }
}
