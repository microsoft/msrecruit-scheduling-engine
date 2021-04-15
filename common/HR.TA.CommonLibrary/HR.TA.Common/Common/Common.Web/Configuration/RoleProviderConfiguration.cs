//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.Web.Configuration
{
    using Base.Configuration;
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>The Role provider configuration with cache configuration.</summary>
    [SettingsSection(nameof(RoleProviderConfiguration))]
    public class RoleProviderConfiguration : ExtendedRedisCacheConfiguration
    {
        /// <summary>Gets or sets a value indicating whether to disable caching. </summary>
        public bool EnableRoleProviderCache { get; set; }

        /// <summary>
        /// Gets or sets memory cache size in MB.
        /// </summary>
        public int MemoryCacheSizeInMB { get; set; }

        /// <summary>
        /// Gets or sets service for role check
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets Endpoint for role check
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Gets or sets cluster Url name.
        /// </summary>
        public string ClusterUrl { get; set; }

        /// <summary>
        /// Gets or sets Path
        /// </summary>
        public string Path { get; set; }
    }
}
