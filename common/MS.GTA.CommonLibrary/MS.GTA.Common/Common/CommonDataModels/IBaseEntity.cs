//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.GTA.Common.CommonDataModels
{
    /// <summary>
    /// Holds IBaseEntity interface
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        [JsonProperty("id")]
        string id { get; set; }

        /// <summary>
        /// Gets or sets etag
        /// </summary>
        [JsonProperty("_etag")]
        string ETag { get; set; }
    }
}
