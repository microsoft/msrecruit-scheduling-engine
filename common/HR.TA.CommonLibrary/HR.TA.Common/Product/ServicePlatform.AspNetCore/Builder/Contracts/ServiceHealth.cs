//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HR.TA.ServicePlatform.AspNetCore.Builder.Contracts
{
    /// <summary>
    /// The contract representing the health of a service.
    /// </summary>
    internal sealed class ServiceHealth
    {
        /// <summary>
        /// Gets or sets the timestamp in which the health was reported.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the application of the service reporting the health.
        /// </summary>
        [JsonProperty(PropertyName = "application")]
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the service reporting health.
        /// </summary>
        [JsonProperty(PropertyName = "service")]
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets the version of the service reporting the health.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the node of the service reporting the health.
        /// </summary>
        [JsonProperty(PropertyName = "node")]
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets any additional health details of the service.
        /// </summary>
        [JsonProperty(PropertyName = "details")]
        public IDictionary<string, object> Details { get; set; }
    }
}
