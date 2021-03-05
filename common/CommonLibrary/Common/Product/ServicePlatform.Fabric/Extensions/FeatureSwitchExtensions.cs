//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonDataService.Common.Internal;
using ServicePlatform.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePlatform.Fabric.Extensions
{
    /// <summary>
    /// Extensions for GTA Service Fabric Feature Switches
    /// </summary>
    public static class FeatureSwitchExtensions
    {
        /// <summary>
        /// Registers IFeatureSwitchManager type to default FeatureSwitchManager instance with default configurations.
        /// </summary>
        public static void AddServiceFabricFeatureSwitchManager(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddSingleton<IFeatureSwitchManager>(serviceProvider =>
            {
                var configurationManager = serviceProvider.GetRequiredService<IConfigurationManager>();
                return new FeatureSwitchManager(configurationManager);
            });
        }
    }
}
