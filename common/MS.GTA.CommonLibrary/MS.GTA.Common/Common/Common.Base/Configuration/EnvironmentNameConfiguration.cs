//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentNameConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Configuration
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