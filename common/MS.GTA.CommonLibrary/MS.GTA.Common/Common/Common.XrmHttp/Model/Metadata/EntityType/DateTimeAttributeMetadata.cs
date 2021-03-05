//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.XrmHttp.Model.Metadata
{
    using System;
    using System.Runtime.Serialization;

    public class DateTimeAttributeMetadata : AttributeMetadata
    {
        [DataMember(Name = "MinSupportedValue")]
        public DateTime? MinSupportedValue { get; set; }

        [DataMember(Name = "MaxSupportedValue")]
        public DateTime? MaxSupportedValue { get; set; }
    }
}
