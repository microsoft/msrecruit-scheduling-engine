//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System.Runtime.Serialization;

    public class BooleanOptionSetMetadata : OptionSetMetadataBase
    {
        [DataMember(Name = "TrueOption")]
        public OptionMetadata TrueOption { get; set; }

        [DataMember(Name = "FalseOption")]
        public OptionMetadata FalseOption { get; set; }
    }
}
