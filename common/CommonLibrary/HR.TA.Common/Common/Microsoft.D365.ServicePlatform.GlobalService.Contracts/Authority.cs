//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace HR.TA.ServicePlatform.GlobalService.Contracts.Client
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
