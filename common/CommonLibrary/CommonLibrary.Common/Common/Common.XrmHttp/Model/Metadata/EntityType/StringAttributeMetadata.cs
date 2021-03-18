//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class StringAttributeMetadata : AttributeMetadata
    {
        [DataMember(Name = "MaxLength")]
        public int? MaxLength { get; set; }

        [DataMember(Name = "DatabaseLength")]
        public int? DatabaseLength { get; set; }
    }
}
