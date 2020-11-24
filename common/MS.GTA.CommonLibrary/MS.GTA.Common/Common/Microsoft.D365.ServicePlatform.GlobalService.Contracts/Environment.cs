//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// An environment representation.
    /// </summary>
    /// <remarks>
    /// The document identifier is unique given the authority identifier and the environment identifier.
    /// The document partition is the cluster partition's document partition (which is the CNAME that services the partition).
    /// </remarks>
    public sealed class Environment
    {
        /// <summary>
        /// Gets or sets the identifier of the environment.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the cluster partition's document identifier.
        /// </summary>
        [JsonProperty(PropertyName = "partitionId")]
        public string PartitionId { get; set; }

        /// <summary>
        /// Gets or sets the cluster partition's url
        /// </summary>
        [JsonProperty(PropertyName = "partitionUrl")]
        public string PartitionUrl { get; set; }

        /// <summary>
        /// Gets or sets the rights for accessing this environment.
        /// </summary>
        [JsonProperty(PropertyName = "rights")]
        public EnvironmentRights Rights { get; set; }

        /// <summary>
        /// Gets or sets the namespace id of this environment.
        /// This property is deprecated but necessary for the current gateway.
        /// </summary>
        [JsonProperty(PropertyName = "namespaceId")]
        [Obsolete("Namespace Id will be deprecated and should not be relied on. This is for CDS only.", error: false)]
        public string NamespaceId { get; set; }
    }
}
