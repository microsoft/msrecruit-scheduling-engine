//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// Represents the mapping of a domain to a cluster.
    /// </summary>
    public sealed class Mapping
    {
        /// <summary>
        /// Gets or sets the Custer ID.
        /// </summary>
        [JsonProperty(PropertyName = "clusterId")]
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
    }
}
