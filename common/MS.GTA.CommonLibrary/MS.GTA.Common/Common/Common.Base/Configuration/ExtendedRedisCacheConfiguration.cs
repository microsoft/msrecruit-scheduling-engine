//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.Base.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>The talent engagement cache configuration.</summary>
    [SettingsSection(nameof(ExtendedRedisCacheConfiguration))]
    public class ExtendedRedisCacheConfiguration
    {
        /// <summary>Gets or sets the Application Name.</summary>
        public string RedisKeyNamespace { get; set; }

        /// <summary>Gets or sets the synchronization channel name.</summary>
        public string RedisSynchronizationChannelName { get; set; }

        /// <summary>Gets or sets the Secret Name.</summary>
        public string RedisSecretName { get; set; }

        /// <summary> Gets or sets the life time of cache.</summary>
        public string CacheLifeTimeInMinutes { get; set; }

        /// <summary>Gets or sets a value indicating whether to enable memory cache.</summary>
        public bool EnableMemoryCache { get; set; }

        /// <summary>Gets or sets a value indicating whether to synchronize changes.</summary>
        public bool SynchronizeChanges { get; set; }
        
        /// <summary>
        /// Gets or sets memory cache size in MB.
        /// </summary>
        public int MemoryCacheSizeInMB { get; set; }
    }
}
