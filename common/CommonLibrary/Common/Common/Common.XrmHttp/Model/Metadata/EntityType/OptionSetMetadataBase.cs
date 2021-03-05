//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp.Model.Metadata
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Common.XrmHttp;

    [ODataEntity(PluralName = "GlobalOptionSetDefinitions", SingularName = "OptionSetMetadataBase")]
    public class OptionSetMetadataBase : MetadataBase
    {
        [DataMember(Name = "Description")]
        public Label Description { get; set; }
        
        [DataMember(Name = "DisplayName")]
        public Label DisplayName { get; set; }
        
        [DataMember(Name = "IsCustomOptionSet")]
        public bool? IsCustomOptionSet { get; set; }
        
        [DataMember(Name = "IsGlobal")]
        public bool? IsGlobal { get; set; }
        
        [DataMember(Name = "IsManaged")]
        public bool? IsManaged { get; set; }
        
        [DataMember(Name = "IsCustomizable")]
        public BooleanManagedProperty IsCustomizable { get; set; }
        
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        
        [DataMember(Name = "ExternalTypeName")]
        public string ExternalTypeName { get; set; }
        
        [DataMember(Name = "OptionSetType")]
        public OptionSetType? OptionSetType { get; set; }
        
        [DataMember(Name = "IntroducedVersion")]
        public string IntroducedVersion { get; set; }
    }
}
