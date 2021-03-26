//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class AttributeTypeDisplayName
    {
        [DataMember(Name = "Value")]
        public string Value { get; set; }
    }
}
