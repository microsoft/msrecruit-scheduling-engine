//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
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