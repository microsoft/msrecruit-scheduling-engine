// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------


namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class IntegerAttributeMetadata : AttributeMetadata
    {
        [DataMember(Name = "MaxValue")]
        public int? MaxValue { get; set; }

        [DataMember(Name = "MinValue")]
        public int? MinValue { get; set; }
    }
}
