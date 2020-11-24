//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="RunnerServiceConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TestBase.Contracts
{
    using System.Collections.Generic;
    using ServicePlatform.Configuration;

    /// <summary>
    /// Runner service configuration.
    /// </summary>
    [SettingsSection("RunnerServiceSetting")]
    public class RunnerServiceConfiguration
    {
        public IList<string> DllNamespacesToScan { get; set; }
        
        /// <summary>
        /// Gets or sets the EnvironmentId
        /// </summary>
        public string EnvironmentIdForRunnerService { get; set; }
        
        public string RunnerName { get; set; }
    }
}
