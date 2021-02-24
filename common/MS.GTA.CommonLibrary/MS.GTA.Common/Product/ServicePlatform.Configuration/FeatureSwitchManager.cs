//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Configuration.FeatureSwitches;

namespace MS.GTA.ServicePlatform.Configuration
{
    public sealed class FeatureSwitchManager : IFeatureSwitchManager
    {
        /// <summary>
        /// Instance of ConfigurationManager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Section Name for Feature Switches
        /// </summary>
        private readonly string sectionName = "GTA:ServicePlatform:FeatureSwitches";

        public FeatureSwitchManager(IConfigurationManager configurationManager)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            this.configurationManager = configurationManager;
        }

        public FeatureSwitchManager(IConfigurationManager configurationManager, string sectionName)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            Contract.CheckNonEmpty(sectionName, nameof(sectionName));

            this.configurationManager = configurationManager;
            this.sectionName = sectionName;
        }

        /// <summary>
        /// Given a feature get the value of the feature from configuration.
        /// </summary>
        public bool IsEnabled<T>(T feature) where T : SingletonFeature<T>, new()
        {
            return configurationManager.GetValue<bool>(sectionName, feature.Name);
        }
    }
}
