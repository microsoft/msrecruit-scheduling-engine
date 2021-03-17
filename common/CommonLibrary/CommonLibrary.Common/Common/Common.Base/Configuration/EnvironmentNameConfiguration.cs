//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Base.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// Environment name configuration
    /// </summary>
    [SettingsSection("EnvironmentNameConfiguration")]
    public class EnvironmentNameConfiguration
    {
        /// <summary> Gets or sets the environment to use - DEV, INT or PROD  </summary>
        public string EnvironmentName { get; set; }

        /// <summary> Gets or sets the environment region to use - Local, WestUs, Canada, Europe, ect.  </summary>
        public string EnvironmentRegion { get; set; }
    }
}
