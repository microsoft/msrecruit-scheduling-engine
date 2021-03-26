//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using HR.TA.CommonDataService.Common.Internal;
using Microsoft.Extensions.Configuration;

namespace HR.TA.ServicePlatform.Configuration
{
    /// <summary>
    /// Default implementation for the configuration manager.
    /// </summary>
    public sealed class ConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// Cache of settings object instances. Prevents needing to bind same object multiple times.
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> configurationCache = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Holds the settings after the call to <code>Bind</code>.
        /// </summary>
        private readonly IConfigurationRoot configurationRoot;

        /// <summary>
        /// Constructs an instance of the configuration manager based on the specified configuration sources.
        /// </summary>
        public ConfigurationManager(IEnumerable<IConfigurationSource> configurationSources)
        {
            Contract.CheckValue(configurationSources, nameof(configurationSources));

            var configurationBuilder = new ConfigurationBuilder();

            foreach (var configurationSource in configurationSources)
            {
                configurationBuilder.Add(configurationSource);
            }

            configurationRoot = configurationBuilder.Build();
        }

        /// <summary>
        /// Constructs a configuration manager from an already constructed <see cref="IConfigurationRoot"/>
        /// </summary>
        public ConfigurationManager(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        /// <summary>
        /// Gets an instance of the specified configuration type.
        /// </summary>
        public T Get<T>() where T : class, new()
        {
            return configurationCache.GetOrAdd(
                typeof(T),
                type =>
            {
                // Resolve section name from attribute
                string sectionName;
                var settingsSectionAttribute = (SettingsSectionAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(SettingsSectionAttribute));

                Contract.Check(settingsSectionAttribute != null && !string.IsNullOrWhiteSpace(settingsSectionAttribute.SectionName), $"You attempted to get settings for class: {nameof(T)} but the class does not have the required attribute: {nameof(SettingsSectionAttribute)}. Add the {nameof(SettingsSectionAttribute)} to the class.");

                sectionName = settingsSectionAttribute.SectionName;

                // Create new instance of type and bind settings to its properties.
                var settings = new T();
                configurationRoot.GetSection(sectionName).Bind(settings);

                return settings;
            }) as T;
        }

        /// <summary>
        /// Given section name and parameter name get value and cast to desired type
        /// </summary>
        public T GetValue<T>(string sectionName, string parameterName)
        {
            return configurationRoot.GetSection(sectionName).GetValue<T>(parameterName);
        }
    }
}
