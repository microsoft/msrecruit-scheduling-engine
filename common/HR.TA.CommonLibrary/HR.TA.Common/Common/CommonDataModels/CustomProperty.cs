//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.TA.Common.CommonDataModels
{
    /// <summary>
    /// CustomProperties.
    /// </summary>
    public class CustomProperty
    {
        /// <summary>
        /// Gets or sets Key.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>s
        /// <value>
        /// The valueStr.
        /// </value>
        [ExcludeFromResponse]
        [JsonIgnore]
        public string ValueStr { get { return JsonConvert.SerializeObject(this.Value); } }
    }
}
