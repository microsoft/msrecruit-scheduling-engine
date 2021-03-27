//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Configurations
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// Configuration file.
    /// </summary>
    [SettingsSection("RedisCacheConfiguration")]
    public sealed class RedisCacheConfiguration
    {
        /// <summary>
        /// Gets or sets the Redis Cache connection string.
        /// </summary>
        public string RedisCacheConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether redis cache is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
