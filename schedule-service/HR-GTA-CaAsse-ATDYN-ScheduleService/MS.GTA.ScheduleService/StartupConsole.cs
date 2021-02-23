//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService
{
    using System.Diagnostics;
    using System.IO;

    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

    /// <summary>
    /// The startup steps when running as a console application.
    /// </summary>
    public class StartupConsole : StartupBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartupConsole"/> class.
        /// </summary>
        /// <param name="configuration">Instance of <see cref="Microsoft.Extensions.Configuration.IConfiguration"/></param>
        public StartupConsole(Microsoft.Extensions.Configuration.IConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Configures the dependency injection services used.
        /// </summary>
        /// <param name="services">The services collection to populate.</param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            var configurationManager = this.GetFabricXmlConfigurationManager();
            services.AddSingleton(typeof(IConfigurationManager), p => configurationManager);

            // Build an intermediate service provider and resolve the secret manager from the service provider to get cache connection string from key vault.
            var redisConfig = configurationManager?.Get<RedisCacheConfiguration>();
            var secretManager = services.BuildServiceProvider().GetService<ISecretManager>();

            if (redisConfig.IsEnabled)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = secretManager.ReadSecretAsync(redisConfig.RedisCacheConnectionString).Result.Value;
                });
            }
        }

        /// <summary>
        /// Get the configuration manager from the service fabric xml.
        /// </summary>
        /// <returns>The configuration manager.</returns>
        private ConfigurationManager GetFabricXmlConfigurationManager()
        {
            var process = Process.GetCurrentProcess();
            var fullPath = Path.GetDirectoryName(process?.MainModule?.FileName);
            var path = $"{fullPath}\\PackageRoot\\Config\\Settings.xml";
            var provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
            return new ConfigurationManager(
                    new IConfigurationSource[]
                    {
                        new FabricXmlConfigurationSource() { Path = Path.GetFileName(path), FileProvider = provider }
                    });
        }
    }
}
