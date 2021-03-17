//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TestBase.Contracts
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
