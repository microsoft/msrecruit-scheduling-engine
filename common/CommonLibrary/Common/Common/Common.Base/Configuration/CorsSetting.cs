//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.Configuration
{
    using Microsoft.AspNetCore.Http;
    using ServicePlatform.Configuration;

    /// <summary>
    /// CORS settings class
    /// </summary>
    [SettingsSection("CorsSetting")]
    public class CorsSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorsSetting"/> class.
        /// </summary>
        public CorsSetting()
        {
            // Default to 10 minutes (Chrome's max value)
            this.AccessControlMaxAge = 600;
        }

        /// <summary>
        /// Gets or sets access control request methods
        /// </summary>
        public string AccessControlAllowMethods { get; set; }

        /// <summary>
        /// Gets or sets access control allow headers
        /// </summary>
        public string AccessControlAllowHeaders { get; set; }

        /// <summary>
        /// Gets or sets access control allow origin
        /// </summary>
        public string AccessControlAllowOrigin { get; set; }

        /// <summary>
        /// Gets or sets access control expose headers for reading on clients
        /// </summary>
        public string AccessControlExposeHeaders { get; set; }

        /// <summary>
        /// Gets or sets the maximum time in seconds until the endpoint must be pre-flighted again.
        /// </summary>
        public long AccessControlMaxAge { get; set; }

        /// <summary>
        /// Gets or sets the CORS headers.
        /// </summary>
        public IHeaderDictionary CorsHeader => 
            new HeaderDictionary
            {
                {
                    "Access-Control-Allow-Methods",
                    this.AccessControlAllowMethods
                },
                {
                    "Access-Control-Allow-Headers",
                    this.AccessControlAllowHeaders
                },
                {
                    "Access-Control-Allow-Origin",
                    this.AccessControlAllowOrigin
                },
                {
                    "Access-Control-Expose-Headers",
                    this.AccessControlExposeHeaders
                },
                {
                    "Access-Control-Max-Age",
                    this.AccessControlMaxAge.ToString()
                },
            };
    }
}
