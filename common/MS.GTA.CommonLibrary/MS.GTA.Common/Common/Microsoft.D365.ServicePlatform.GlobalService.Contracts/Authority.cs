//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// An authority principal.
    /// </summary>
    public sealed class Authority
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
