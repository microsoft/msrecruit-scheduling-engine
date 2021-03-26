//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class AttributeTypeDisplayName
    {
        [DataMember(Name = "Value")]
        public string Value { get; set; }
    }
}
