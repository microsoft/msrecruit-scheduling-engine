//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.TalentAttract.Configuration
{
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>
    /// The LoggingConfiguration.
    /// </summary>
    [SettingsSection("EnvironmentConfiguration")]
    public class EnvironmentConfiguration
    {
        /// <summary>
        /// Gets or sets the XRM instance uri.
        /// </summary>
        public string XRMInstanceApiUrl { get; set; }

        /// <summary>
        /// Get or set the XRM Target Environment ID
        /// </summary>
        public string XRMEnvironment { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database ID
        /// </summary>
        public string FalconDatabaseId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for Offer
        /// </summary>
        public string FalconOfferContainerId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for IV
        /// </summary>
        public string FalconIVContainerId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for common Talent entities
        /// </summary>
        public string FalconCommonContainerId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for pilot entities
        /// </summary>
        public string FalconPilotContainerId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for TalentMatchEntities
        /// </summary>
        public string FalconTalentMatchContainerId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for Common used by Talent Match
        /// </summary>
        public string FalconCommonTMContainerId { get; set; }

        /// <summary>
        /// Get or set the Falcon/Cosmos Database Collection ID for Common used by TeamsIntegrationContainerId
        /// </summary>
        public string TeamsIntegrationContainerId { get; set; }
        

    }
}
