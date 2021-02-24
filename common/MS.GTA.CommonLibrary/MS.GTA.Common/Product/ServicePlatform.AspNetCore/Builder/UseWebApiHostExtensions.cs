//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.AspNetCore.Http;
using MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Hosting;

namespace MS.GTA.ServicePlatform.AspNetCore.Builder
{
    public static class UseWebApiHostExtensions
    {
        public static IApplicationBuilder UseExternalWebApiHost(this IApplicationBuilder builder, IEnvironmentContext environmentContext = null)
        {
            Contract.CheckValue(builder, nameof(builder));
            Contract.CheckValueOrNull(environmentContext, nameof(environmentContext));

            return builder
                .UseMonitoredExceptionHandler()
                // TODO: Use middleware provided by Service Fabric team
                // See: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication-aspnetcore#service-fabric-integration-middleware
                // Package is only available on official NuGet feeds and needs to be mirrored
                .UseServiceFabricRouting()
                .UseWebApiHost(new ExternalWebApiHost(environmentContext))
                .UseCorrelationInfo()
                .UseNoSnoopHeader();
        }

        public static IApplicationBuilder UseInternalWebApiHost(this IApplicationBuilder builder, InternalWebApiHostOptions options, IEnvironmentContext environmentContext = null)
        {
            Contract.CheckValue(builder, nameof(builder));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(
                options.ServiceContextPrincipalType, 
                StringUtil.FormatInvariant("{0} needs to be provided on the options", nameof(options.ServiceContextPrincipalType)));
            Contract.CheckValueOrNull(environmentContext, nameof(environmentContext));

            // Order is important, when processing, execution moves from top-to-bottom while processing the request,
            // then bottom-to-top processing the response.  We want to ensure that Excepion Handling is the last
            // pass before returning a response, so that we can ensure any Exceptions are correctly serialized to the response.
            return builder
                .UseMonitoredExceptionHandler()
                // TODO: Use middleware provided by Service Fabric team
                // See: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication-aspnetcore#service-fabric-integration-middleware
                // Package is only available on official NuGet feeds and needs to be mirrored
                .UseServiceFabricRouting()
                .UseWebApiHost(new InternalWebApiHost(options, environmentContext))
                .UseCorrelationInfo();
        }

        public static IApplicationBuilder UseWebApiHost(this IApplicationBuilder builder, IWebApiHost webApiHost)
        {
            Contract.AssertValue(builder, nameof(builder));
            Contract.AssertValue(webApiHost, nameof(webApiHost));

            object currentServiceContextProvider;
            if (builder.Properties.TryGetValue(typeof(IWebApiHost).FullName, out currentServiceContextProvider))
                throw new WebApiHostAlreadyRegisteredException(currentServiceContextProvider);

            builder.Properties[typeof(IWebApiHost).FullName] = webApiHost;
            builder.UseStatelessHttpMiddleware(webApiHost);
            return builder;
        }
    }
}
