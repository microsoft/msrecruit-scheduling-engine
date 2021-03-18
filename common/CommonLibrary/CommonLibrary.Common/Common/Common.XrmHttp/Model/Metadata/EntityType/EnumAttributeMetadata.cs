//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.XrmHttp.Model.Metadata
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
