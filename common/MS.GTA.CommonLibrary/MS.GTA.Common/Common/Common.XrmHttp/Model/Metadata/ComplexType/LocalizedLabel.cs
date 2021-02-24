//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System;
    using System.Runtime.Serialization;

    public class LocalizedLabel
    {
        [DataMember(Name = "Label")]
        public string Label { get; set; }
        
        [DataMember(Name = "LanguageCode")]
        public int? LanguageCode { get; set; }
        
        [DataMember(Name = "IsManaged")]
        public bool? IsManaged { get; set; }
        
        [DataMember(Name = "MetadataId")]
        public Guid? MetadataId { get; set; }
        
        [DataMember(Name = "HasChanged")]
        public bool? HasChanged { get; set; }
    }
}
