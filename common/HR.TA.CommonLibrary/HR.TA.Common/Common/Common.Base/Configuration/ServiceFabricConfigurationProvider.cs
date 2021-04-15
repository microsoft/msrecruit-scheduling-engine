//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Configuration
{
    using System.Collections.Generic;
    using System.Fabric;
    using System.Fabric.Description;
    using System.Linq;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Service fabric configuration provider class
    /// </summary>
    public class ServiceFabricConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Configuration settings
        /// </summary>
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFabricConfigurationProvider" /> class.
        /// </summary>
        /// <param name="codePackageActivationContext">Instance of service fabric activation context</param>
        public ServiceFabricConfigurationProvider(CodePackageActivationContext codePackageActivationContext)
        {
            this.configurationSettings = codePackageActivationContext.GetConfigurationPackageObject("Config").Settings;
        }

        /// <summary>
        /// Loads configuration settings
        /// </summary>
        public override void Load()
        {
            this.Data = new Dictionary<string, string>();
            this.configurationSettings.Sections.ToList().ForEach(
                section =>
                    {
                        section.Parameters.ToList().ForEach(parameter => this.Data.Add($"{section.Name}:{parameter.Name}", parameter.Value));
                    });
        }

        /// <summary>
        /// Check if key exists in dictionary
        /// </summary>
        /// <param name="key">Key string</param>
        /// <param name="value">Value string</param>
        /// <returns>True if key exists</returns>
        public override bool TryGet(string key, out string value)
        {
            return this.Data.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set value for key
        /// </summary>
        /// <param name="key">Key string</param>
        /// <param name="value">Value string</param>
        public override void Set(string key, string value)
        {
            this.Data[key] = value;
        }
    }
}
