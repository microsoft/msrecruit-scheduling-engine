// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
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