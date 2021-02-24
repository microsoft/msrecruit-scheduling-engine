//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class OptionMetadata
    {
        [DataMember(Name = "Value")]
        public int? Value { get; set; }
        
        [DataMember(Name = "Label")]
        public Label Label { get; set; }
        
        [DataMember(Name = "Description")]
        public Label Description { get; set; }
        
        [DataMember(Name = "Color")]
        public string Color { get; set; }
        
        [DataMember(Name = "IsManaged")]
        public bool? IsManaged { get; set; }
        
        [DataMember(Name = "ExternalValue")]
        public string ExternalValue { get; set; }
        
        [DataMember(Name = "ParentValues")]
        public IList<int> ParentValues { get; set; }
        
        [DataMember(Name = "MetadataId")]
        public Guid? MetadataId { get; set; }
        
        [DataMember(Name = "HasChanged")]
        public bool? HasChanged { get; set; }
    }
}
