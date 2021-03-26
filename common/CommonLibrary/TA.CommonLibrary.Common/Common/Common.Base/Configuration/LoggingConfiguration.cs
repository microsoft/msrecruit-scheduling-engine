//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

using TA.CommonLibrary.ServicePlatform.Configuration;

namespace TA.CommonLibrary.Common.Common.Common.Base.Configuration
{
    /// <summary>
    /// Logging Configuration
    /// </summary>
    [SettingsSection(nameof(LoggingConfiguration))]
    public class LoggingConfiguration
    {
        /// <summary>
        /// Gets or sets Application Insights Instrumentation Key
        /// </summary>
        public string InstrumentationKey { get; set; }
    }
}
