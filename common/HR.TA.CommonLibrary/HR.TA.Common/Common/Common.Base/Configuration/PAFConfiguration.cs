//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Configuration
{

    using HR.TA.ServicePlatform.Configuration;
    
    /// <summary>
    /// PAF Configuration
    /// </summary>
    [SettingsSection(nameof(PAFConfiguration))]
    public class PAFConfiguration
    {
        /// <summary>
        /// Gets or sets PAF API Service Version
        /// </summary>
        public string PafServiceVersion { get; set; }

        /// <summary>
        /// Gets or sets PAF API Service URL
        /// </summary>
        public string PafServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets PAF App Name associated to Offer
        /// </summary>
        public string PafAppName { get; set; }

        /// <summary>
        /// Gets or sets Tenant that shall issue the token
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets PAF Resource Id for the token
        /// </summary>
        public string PAFResource { get; set; }
    }
}
