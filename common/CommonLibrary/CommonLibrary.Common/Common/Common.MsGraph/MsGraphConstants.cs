//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.MsGraph
{
    /// <summary>
    /// Graph constants
    /// </summary>
    public static class MsGraphConstants
    {
        /// <summary>
        /// Header Authorization string 
        /// </summary>
        public static readonly string Authorization = "Authorization";

        /// <summary>
        /// Header TokenExpired string 
        /// </summary>
        public static readonly string TokenExpired = "TokenExpired";

        /// <summary>
        /// Header AAD redirect type - used in tenant takeover scenario 
        /// </summary>
        public static readonly string AADRedirectType = "ocp-aad-redirect-type";

        /// <summary>
        /// Header value for AAD redirect type - used in tenant takeover scenario
        /// </summary>
        public static readonly string AADRedirectTypeValue = "viral-takeover";

        /// <summary>
        /// Cache key prefix for service plan
        /// </summary>
        public static readonly string MSGraphProviderCacheKeyPrefix = "msGraphProvider";

        /// <summary>
        /// Default memory cache - physical memory limit percentage
        /// </summary>
        public static readonly string PhysicalMemoryLimitPercentage = "3";

        /// <summary>
        /// Default memory cache - polling interval
        /// </summary>
        public static readonly double PollingIntervalInMinutes = 2;

        /// <summary>
        /// Default memory cache - time to live for the cache object in hours
        /// </summary>
        public static readonly double TimeToLiveInHours = 0.33;

        /// <summary>
        /// Header in case of redirect from sign up
        /// </summary>
        public static readonly string SignUpRedirectHeader = "x-dynamics365-signup-redirect";
    }
}
