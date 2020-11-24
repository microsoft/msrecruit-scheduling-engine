﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WOBEnvironmentConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.BusinessLibrary.UserDelegation
{
    using MS.GTA.ServicePlatform.Configuration;
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
