//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class EnumAttributeMetadata : AttributeMetadata
    {
        [DataMember(Name = "DefaultFormValue")]
        public int? DefaultFormValue { get; set; }

        [DataMember(Name = "OptionSet")]
        public OptionSetMetadata OptionSet { get; set; }

        [DataMember(Name = "GlobalOptionSet")]
        public OptionSetMetadata GlobalOptionSet { get; set; }

    }
}
