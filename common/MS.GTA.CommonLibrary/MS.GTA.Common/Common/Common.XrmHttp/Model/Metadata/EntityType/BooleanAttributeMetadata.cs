//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class BooleanAttributeMetadata : AttributeMetadata
    {
        [DataMember(Name = "OptionSet")]
        public BooleanOptionSetMetadata OptionSet { get; set; }

        [DataMember(Name = "GlobalOptionSet")]
        public BooleanOptionSetMetadata GlobalOptionSet { get; set; }

        [DataMember(Name = "DefaultValue")]
        public bool? DefaultValue { get; set; }
    }
}