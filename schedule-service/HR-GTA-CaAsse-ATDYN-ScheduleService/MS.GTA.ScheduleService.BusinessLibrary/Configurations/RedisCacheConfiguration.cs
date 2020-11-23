// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="RedisCacheConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Configurations
{
    using MS.GTA.ServicePlatform.Configuration;

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