//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ECSProviderConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
using MS.GTA.ServicePlatform.Configuration;
using System;

namespace MS.GTA.ServicePlatform.Flighting.ECSProvider
{
    /// <summary>
    /// Configuration options for the ECS Configuration provider implementation.
    /// </summary>
    [SettingsSection("GTA:ServicePlatform:ECSProviderConfiguration")]
    public class ECSProviderConfiguration
    {
        /// <summary>
        /// Gets or sets the environment as displayed in the ECS portal, i.e. DEV, INT, PROD.
        /// </summary>
        public string EnvironmentName { get; set; } = "Dev";

        /// <summary>
        /// Gets or sets the environment type of ECS which maintains the configuration details - Integration (0) or Production(1).
        /// </summary>
        public int EnvironmentType { get; set; } = 1;

        /// <summary>
        /// Gets or sets the Custom Client in ECS which host the project team as displayed in ECS portal.
        /// </summary>
        public string ClientName { get; set; } = "CSE-Teams";

        /// <summary>
        /// Gets or sets the Project Team which contains the Feature Rollouts of the Application as displayed in ECS portal.
        /// </summary>
        public string ProjectTeamName { get; set; } = "MSGTAApplications";

        /// <summary>
        /// Gets or sets the format in which the feature configuration is set in the ECS portal.
        /// </summary>
        public string FeatureConfigurationTemplate { get; set; } = $"{0}.IsEnabled";

        /// <summary>
        /// Gets or sets the provider initialization timeout.
        /// </summary>
        public TimeSpan InitializationTimeout { get; set; } = TimeSpan.FromMilliseconds(30000);

        /// <summary>
        /// Gets or sets flag to leverage the caching capability of ECS SDK
        /// </summary>
        public bool EnableCaching { get; set; } = false;
    }
}
