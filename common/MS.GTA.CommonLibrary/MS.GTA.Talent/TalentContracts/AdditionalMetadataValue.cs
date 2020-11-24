﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// One of the values in a dictionary serialized in an AdditionalMetadata field.
    /// </summary>
    [DataContract]
    public class AdditionalMetadataValue
    {
        /// <summary>The value. Interpretation depends on the type field.</summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>The value type.</summary>
        [DataMember(Name = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AdditionalMetadataValueType? Type { get; set; }
    }
}
