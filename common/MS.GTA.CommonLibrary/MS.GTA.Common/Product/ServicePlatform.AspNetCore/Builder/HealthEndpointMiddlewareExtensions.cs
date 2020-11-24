//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.AspNetCore.Builder.Abstractions;
using MS.GTA.ServicePlatform.AspNetCore.Builder.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.AspNetCore.Builder
{
    /// <summary>
    /// Middleware extensions for creating a health endpoint.
    /// </summary>
    public static class HealthEndpointMiddlewareExtensions
    {
        /// <summary>
        /// Adds the required endpoints for the health endpoints.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection to use in further registration.</returns>
        public static IServiceCollection AddHealthEndpoints(this IServiceCollection services)
        {
            return services.AddHealthEndpoints<ServiceFabricNodeContext>();
        }

        /// <summary>
        /// Adds the required endpoints for the health endpoints.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection to use in further registration.</returns>
        public static IServiceCollection AddHealthEndpoints<TNodeContext>(this IServiceCollection services) where TNodeContext : INodeContext
        {
            return services
                .AddSingleton(typeof(INodeContext), typeof(TNodeContext))
                .AddRouting();
        }

        /// <summary>
        /// Uses the health endpoint middleware, setting up the following endpoints:
        /// 
        /// health/ping -> returns OK.
        /// 
        /// health/details -> returns details pertaining to the service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The builder to use in further registrations.</returns>
        public static IApplicationBuilder UseHealthEndpoints(this IApplicationBuilder builder)
        {
            return UseHealthEndpoints(builder, NoDetails);
        }

        /// <summary>
        /// Uses the health endpoint middleware, setting up the following endpoints:
        /// 
        /// health/ping -> returns OK.
        /// 
        /// health/details -> returns details pertaining to the service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="retrieveAdditionalHealthDetails">A function that returns additional details to the health contract.</param>
        /// <returns>The builder to use in further registrations.</returns>
        public static IApplicationBuilder UseHealthEndpoints(this IApplicationBuilder builder, Func<HttpContext, Task<IDictionary<string, object>>> retrieveAdditionalHealthDetails)
        {
            Contract.CheckValue(retrieveAdditionalHealthDetails, nameof(retrieveAdditionalHealthDetails));

            var routeBuilder = new RouteBuilder(builder);

            // Basic ping endpoint to ensure application is running.
            routeBuilder.MapGet(
                "health/ping",
                async context =>
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(DateTimeOffset.UtcNow.ToString("o"));
                });

            // Details endpoint, which will indicate the code package and version and node on which the application is running,
            // in addition to any other details the user wishes to provide to the caller.
            routeBuilder.MapGet(
                "health/details",
                async context =>
                {
                    var nodeContext = context.RequestServices.GetService<INodeContext>();
                    var healthContract = new ServiceHealth
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        Application = Context.ServiceContext.Environment.Current.Application,
                        Service = Context.ServiceContext.Environment.Current.Service,
                        Version = Context.ServiceContext.Environment.Current.CodePackageVersion,
                        Node = nodeContext.GetCurrentNodeName(),
                        Details = await retrieveAdditionalHealthDetails.Invoke(context)
                    };

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(
                            healthContract,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            }));
                });

            var routes = routeBuilder.Build();
            builder.UseRouter(routes);

            return builder;
        }

        private static Task<IDictionary<string, object>> NoDetails(HttpContext httpContext)
        {
            return Task.FromResult((IDictionary<string, object>)null);
        }
    }
}
