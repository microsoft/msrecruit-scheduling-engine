// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ConfigurationHelper.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Extensions.Configuration.Memory;
    using Microsoft.Extensions.FileProviders;
    using MS.GTA.ServicePlatform.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Utilities;

    /// <summary>The configuration helper.</summary>
    public class ConfigurationHelper
    {
        /// <summary>The private instance.</summary>
        private static ConfigurationHelper privateInstance;

        private ConfigurationManager privateConfigurationManagerInstance;

        public ConfigurationHelper(string environmentOverride = null) : this(environmentOverride, null)
        {
        }

        public ConfigurationHelper(string environmentOverride, IConfigurationSource[] configurationSources = null) : this(environmentOverride, configurationSources, true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ConfigurationHelper"/> class.</summary>
        public ConfigurationHelper(string environmentOverride, IConfigurationSource[] configurationSources = null, bool loadDefaultConfigurationSources = true, string applicationName = null)
        {
            string fabricApplicationName = string.Empty;

            IConfigurationSource regionBasedConfig = null;
            IConfigurationSource environmentBasedConfig = null;
            IConfigurationSource globalBasedConfig = null;
            MemoryConfigurationSource memoryConfig;

            if (!string.IsNullOrEmpty(applicationName))
            {
                fabricApplicationName = applicationName;
            }
            else
            {
                try
                {
                    var activationContext = FabricRuntime.GetActivationContext();
                    fabricApplicationName = activationContext.ApplicationName;
                }
                catch (Exception)
                {
                }
            }
            
            this.IsConsoleApp = string.IsNullOrEmpty(fabricApplicationName);

            if (loadDefaultConfigurationSources)
            {
                if (!this.IsConsoleApp)
                {
                    //if (TalentProgram.GlobalLogger != null)
                    //{
                    //    TalentProgram.GlobalLogger.LogInformation("Loading service fabric sources.");                    
                    //}

                    regionBasedConfig = new ServiceFabricConfigurationSource(FabricRuntime.GetActivationContext());
                    this.Sources.Add(regionBasedConfig);
                }
                else
                {
                    regionBasedConfig = this.LoadPackageRootSettings();
                }
                
                globalBasedConfig = this.LoadGlobalAppSettings();
            }
            
            // Add the global config first so that anything loaded in a specific environment is overwritten.
            memoryConfig = new MemoryConfigurationSource { InitialData = new [] {new KeyValuePair<string, string>("Environment:IsConsoleApp", this.IsConsoleApp.ToString())}};
            this.Sources.Add(memoryConfig);

            EnvironmentConfiguration environmentConfiguration;

            if (string.IsNullOrEmpty(environmentOverride))
            {
                var tempSources = new List<IConfigurationSource>(this.Sources);

                if (configurationSources != null)
                {
                    tempSources.AddRange(configurationSources);
                }
                
                // Build a temporary configuration manager so we can figure out which environment configuration to load.
                var environmentConfigurationManager = new ConfigurationManager(tempSources);
                environmentConfiguration = environmentConfigurationManager.Get<EnvironmentConfiguration>();
            
                //if (TalentProgram.GlobalLogger != null)
                //{
                //    this.Log($"Finished loading environment configuration: {(environmentConfiguration != null ? JsonConvert.SerializeObject(environmentConfiguration) : "No environment configuration!")}");                    
                //}                
            }
            else
            {
                environmentConfiguration = new EnvironmentConfiguration
                {
                    Name = environmentOverride
                };
                memoryConfig.InitialData = memoryConfig.InitialData.Append(new KeyValuePair<string, string>("Environment:Name", environmentOverride));
            }

            if (!string.IsNullOrEmpty(fabricApplicationName))
            {
                memoryConfig.InitialData = memoryConfig.InitialData.Append(new KeyValuePair<string, string>("Environment:ApplicationName", fabricApplicationName));
            }

            if (loadDefaultConfigurationSources)
            {
                environmentBasedConfig = this.LoadEnvironmentAppSettings(environmentConfiguration);
            }
            
            // Rebuild the stack to a specific order (order here being, #1 is the most important and those values always win)
            // 1. Overrides
            // 2. Memory config, determined from other configs that came from temp sources.
            // 3. XML Config read from the regional configs, i.e westus vs westeurope
            // 4. Environment based config i.e PROD vs INT vs DEV
            // 5. Global config, the same in every deployment.
            this.Sources = new List<IConfigurationSource>();

            if (globalBasedConfig != null)
            {
                this.Log(globalBasedConfig, "GlobalBasedConfig");
                this.Sources.Add(globalBasedConfig);
            }

            if (environmentBasedConfig != null)
            {
                this.Log(environmentBasedConfig, "EnvironmentBasedConfig");
                this.Sources.Add(environmentBasedConfig);
            }

            if (regionBasedConfig != null)
            {
                this.Log(regionBasedConfig, "RegionBasedConfig");
                this.Sources.Add(regionBasedConfig);
            }
            
            this.Log(memoryConfig, "MemoryConfig");
            this.Sources.Add(memoryConfig);
            configurationSources?.ForEach(c =>
            {
                this.Log(c, "ConfigOverrideSource");
                this.Sources.Add(c);
            });
        }

        public JsonConfigurationSource LoadGlobalAppSettings()
        {
            var source = EnvironmentConfiguration.GetGlobalEnvironmentJsonConfiguration();
            this.Sources.Add(source);
            return source;
        }

        public JsonConfigurationSource LoadEnvironmentAppSettings(EnvironmentConfiguration environmentConfiguration)
        {
            var source = EnvironmentConfiguration.GetEnvironmentJsonConfiguration(environmentConfiguration.GetEnvironment());
            this.Sources.Add(source);

            return source;
        }
        
        public FabricXmlConfigurationSource LoadPackageRootSettings()
        {
            var fullPath = Directory.GetCurrentDirectory();
            var path = $"{fullPath}\\PackageRoot\\Config\\Settings.xml";
            var provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
            var source = new FabricXmlConfigurationSource() {Path = Path.GetFileName(path), FileProvider = provider};
            this.Sources.Add(source);

            return source;
        }

        /// <summary>Gets the instance.</summary>
        public static ConfigurationHelper Instance => privateInstance ?? (privateInstance = new ConfigurationHelper());

        public static void InitalizeConfigurationHelperWithOverride(string environmentOverride = null, IConfigurationSource[] configurationSource = null, bool loadDefaultConfigurationSources = true, string applicationName = null)
        {
            privateInstance = new ConfigurationHelper(environmentOverride, configurationSource, loadDefaultConfigurationSources, applicationName);
        }

        /// <summary>Gets or sets a value indicating whether is console app.</summary>
        public bool IsConsoleApp { get; set; }

        /// <summary>The configuration manager.</summary>
        public IConfigurationManager ConfigurationManager
        {
            get
            {
                if (privateConfigurationManagerInstance == null)
                {
                    privateConfigurationManagerInstance = new ConfigurationManager(this.Sources);
                }

                return privateConfigurationManagerInstance;
            }
        }

        public IList<IConfigurationSource> Sources { get; } = new List<IConfigurationSource>();

        public static bool IsInitialized => privateInstance != null;

        public void AddSource(IConfigurationSource item)
        {
            this.Sources.Add(item);
        }

        private void Log(IConfigurationSource source, string loadType)
        {
            this.Log($"Loading {loadType}");
            
            var builder = new ConfigurationBuilder();
            builder.Add(source);
            var built = builder.Build();

            foreach (var setting in built.AsEnumerable())
            {
                this.Log($"Setting {setting.Key}: {setting.Value}");
            }
        }

        private void Log(string log)
        {
            //if (TalentProgram.GlobalLogger != null)
            //{
            //    TalentProgram.GlobalLogger.LogInformation(log);                    
            //}  
        }
    }
}
