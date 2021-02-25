//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// The common data model namespace metadata.
    /// </summary>
    public class NamespaceMetadata
    {
        /// <summary>
        /// Gets or sets the namespace id.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the namespace display name.
        /// </summary>
        [JsonProperty]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the namespace runtime uri.
        /// </summary>
        [JsonProperty]
        public Uri RuntimeUri { get; set; }
    }
}
