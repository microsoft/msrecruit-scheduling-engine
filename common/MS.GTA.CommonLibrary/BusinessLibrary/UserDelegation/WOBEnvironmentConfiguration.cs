//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BusinessLibrary.UserDelegation
{
    using Common.ServicePlatform.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// Configuration for WOB scenario
    /// </summary>
    [SettingsSection("WOBConfiguration")]
    public class WOBEnvironmentConfiguration
    {
        /// <summary>
        /// Cache Refresh time.
        /// </summary>
        public int CacheRefreshTimeInMins { get; set; }
    }
}
