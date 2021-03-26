//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Fabric;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.FileProviders;
    using ServicePlatform.Configuration;

    public class FabricXmlConfigurationHelper
    {
        private static FabricXmlConfigurationHelper privateInstance;

        private readonly IList<IConfigurationSource> sources = new List<IConfigurationSource>();

        private FabricXmlConfigurationHelper()
        {
            var fabricApplicationName = string.Empty;

            try
            {
                var activationContext = FabricRuntime.GetActivationContext();
                fabricApplicationName = activationContext.ApplicationName;
            }
            catch (Exception)
            {
                // ignored
            }

            var isConsoleApp = string.IsNullOrEmpty(fabricApplicationName);
            this.sources.Add(!isConsoleApp
                ? new ServiceFabricConfigurationSource(FabricRuntime.GetActivationContext())
                : this.GetFabricXmlConfigurationManager());
        }

        /// <summary>Gets the instance.</summary>
        public static FabricXmlConfigurationHelper Instance => privateInstance ?? (privateInstance = new FabricXmlConfigurationHelper());

        /// <summary>The configuration manager.</summary>
        public IConfigurationManager ConfigurationManager => new ConfigurationManager(this.sources);

        /// <summary>
        /// Get the configuration manager from the service fabric xml.
        /// </summary>
        /// <returns>The configuration manager.</returns>
        private IConfigurationSource GetFabricXmlConfigurationManager()
        {
            var process = Process.GetCurrentProcess();
            var fullPath = AppDomain.CurrentDomain.BaseDirectory;
            var path = $"{fullPath}\\PackageRoot\\Config\\Settings.xml";
            var provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
            return new FabricXmlConfigurationSource() { Path = Path.GetFileName(path), FileProvider = provider };
        }
    }
}
