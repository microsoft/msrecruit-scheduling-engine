//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="UseServiceFabricReverseProxy404HeaderExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.MiddleWare
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using CommonDataService.Common.Internal;

    /// <summary>
    /// Extension methods for IApplicationBuilder.
    /// </summary>
    public static class UseServiceFabricReverseProxy404HeaderExtensions
    {
        /// <summary>
        /// Use middleware that ensures 404 error responses have the header required when using the Service Fabric reverse proxy.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseServiceFabricReverseProxy404Header(this IApplicationBuilder builder)
        {
            Contract.CheckValue(builder, nameof(builder));

            builder.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.StatusCode == 404)
                    {
                        // When using the service fabric reverse proxy,
                        // normal HTTP 404 error responses must add this header so that 
                        // the proxy does not try to re-resolve the service address.
                        // See https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reverseproxy#special-handling-for-port-sharing-services
                        // for more information
                        context.Response.Headers.Add("X-ServiceFabric", "ResourceNotFound");
                    }

                    return Task.FromResult(0);
                });
                await next();
            });

            return builder;
        }
    }
}
