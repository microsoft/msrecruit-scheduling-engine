//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Fabric;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Configuration;
using TA.CommonLibrary.ServicePlatform.Fabric.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TA.CommonLibrary.ServicePlatform.Fabric.Extensions
{
    /// <summary>
    /// Extensions for GTA Service Fabric Configuration
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Registers IConfigurationManager type to default ConfigurationManager instance with Service Fabric source.
        /// </summary>
        public static void AddServiceFabricConfigurationSettings(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddSingleton<IConfigurationManager>(new ConfigurationManager(new List<IConfigurationSource>()
            {
                new FabricSettingsConfigurationSource()
            }));
        }

        /// <summary>
        /// Adds the ServiceFabricConfiguration file as a configuration source.
        /// </summary>
        public static IConfigurationBuilder AddServiceFabricConfiguration(this IConfigurationBuilder builder)
        {
            return builder.Add(new FabricSettingsConfigurationSource());
        }

        /// <summary>
        /// Adds the ServiceFabricConfiguration file as a configuration source.
        /// </summary>
        public static IConfigurationBuilder AddServiceFabricConfiguration(this IConfigurationBuilder builder, ICodePackageActivationContext fabricActivationContext)
        {
            return builder.Add(new FabricSettingsConfigurationSource(fabricActivationContext));
        }
    }
}
