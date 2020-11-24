//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class PicklistAttributeMetadata : EnumAttributeMetadata
    {
        [DataMember(Name = "FormulaDefinition")]
        public string FormulaDefinition { get; set; }

        [DataMember(Name = "SourceTypeMask")]
        public int? SourceTypeMask { get; set; }

        [DataMember(Name = "ParentPicklistLogicalName")]
        public string ParentPicklistLogicalName { get; set; }

        [DataMember(Name = "ChildPicklistLogicalNames")]
        public IList<string> ChildPicklistLogicalNames { get; set; }
    }
}