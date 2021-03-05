//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using CommonDataService.Common.Internal;
using ServicePlatform.AspNetCore.Http;

namespace ServicePlatform.AspNetCore.Builder
{
    public static class UseServiceFabricRoutingExtensions
    {
        public static IApplicationBuilder UseServiceFabricRouting(this IApplicationBuilder builder)
        {
            Contract.CheckValue(builder, nameof(builder));

            Contract.CheckValue(builder.ServerFeatures, nameof(builder.ServerFeatures));

            var serverAddressesFeature = builder.ServerFeatures.Get<IServerAddressesFeature>();
            //// Contract.CheckNonEmpty(serverAddressesFeature.Addresses, nameof(serverAddressesFeature.Addresses));
            //// https://github.com/aspnet/Announcements/issues/224
            if (!serverAddressesFeature.Addresses.Any())
            {
                serverAddressesFeature.Addresses.Add("http://localhost");
            }

            return builder.UseStatelessHttpMiddleware(new ServiceFabricRoutingMiddleware(serverAddressesFeature.Addresses.ToList()));
        }

        public static IApplicationBuilder UsePathBaseRouting(this IApplicationBuilder builder, string listeningAddress, string baseAddress)
        {
            Contract.CheckValue(builder, nameof(builder));

            Contract.CheckValue(builder.ServerFeatures, nameof(builder.ServerFeatures));

            if(!string.IsNullOrEmpty(listeningAddress) && !string.IsNullOrEmpty(baseAddress))
            {
                var serverAddressesFeature = builder.ServerFeatures.Get<IServerAddressesFeature>();
                
                if (serverAddressesFeature.Addresses.Contains(baseAddress))
                {
                    serverAddressesFeature.Addresses.Remove(baseAddress);
                }

                serverAddressesFeature.Addresses.Add(listeningAddress);
            }

            return builder;
        }

        public static IApplicationBuilder RemovePathBaseRouting(this IApplicationBuilder builder, string listeningAddress, string baseAddress)
        {
            Contract.CheckValue(builder, nameof(builder));

            Contract.CheckValue(builder.ServerFeatures, nameof(builder.ServerFeatures));

            if (!string.IsNullOrEmpty(listeningAddress) && !string.IsNullOrEmpty(baseAddress))
            {
                var serverAddressesFeature = builder.ServerFeatures.Get<IServerAddressesFeature>();

                if (serverAddressesFeature.Addresses.Contains(listeningAddress))
                {
                    serverAddressesFeature.Addresses.Remove(listeningAddress);
                }

                serverAddressesFeature.Addresses.Add(baseAddress);
            }

            return builder;
        }

    }   
}
