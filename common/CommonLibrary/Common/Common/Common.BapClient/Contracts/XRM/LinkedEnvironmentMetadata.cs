//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace Common.BapClient.Contracts.XRM
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// The Linked Environment Metadata
    /// </summary>
    public class LinkedEnvironmentMetadata
    {
        /// <summary>
        /// Gets or sets the linked resource type.
        /// </summary>
        [JsonProperty]
        public LinkedResourceType Type { get; set; }

        /// <summary>
        /// Gets or sets the link resource id.
        /// </summary>
        [JsonProperty]
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets CRM Friendly Name
        /// </summary>
        [JsonProperty]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets CRM Unique Name
        /// </summary>
        [JsonProperty]
        public string UniqueName { get; set; }

        /// <summary>
        /// Gets or sets CRM Domain Name
        /// </summary>
        [JsonProperty]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the CRM Version
        /// </summary>
        [JsonProperty]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the CRM Instance Url
        /// </summary>
        [JsonProperty]
        public Uri InstanceUrl { get; set; }

        /// <summary>
        /// Gets or sets the CRM Instance API Url
        /// </summary>
        [JsonProperty]
        public Uri InstanceApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the CRM instance base language code
        /// </summary>
        [JsonProperty]
        public int BaseLanguage { get; set; }

        /// <summary>
        /// Gets or sets the CRM instance state.
        /// </summary>
        [JsonProperty]
        public InstanceState InstanceState { get; set; }

        /// <summary>
        /// Gets or sets the linked metadata created time
        /// </summary>
        [JsonProperty]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the linked metadata modified time.
        /// </summary>
        [JsonProperty]
        public DateTime? ModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the host name suffix.
        /// </summary>
        [JsonProperty]
        public string HostNameSuffix { get; set; }
    }
}
