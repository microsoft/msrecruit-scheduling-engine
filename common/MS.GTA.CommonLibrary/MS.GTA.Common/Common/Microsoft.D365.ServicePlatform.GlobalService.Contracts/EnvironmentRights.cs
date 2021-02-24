//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// The rights of an environment.
    /// </summary>
    public sealed class EnvironmentRights
    {
        /// <summary>
        /// Gets or sets the owner of this environment.
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        public Authority Owner { get; set; }
    }
}
