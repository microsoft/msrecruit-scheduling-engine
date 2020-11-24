//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// A storage configuration.
    /// </summary>
    public sealed class StorageConfiguration
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type, such as AzureBlob or DocumentDB.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the connection details for reading this storage.
        /// </summary>
        [JsonProperty(PropertyName = "connectionDetails")]
        public ConnectionDetails ConnectionDetails { get; set; }
    }
}
