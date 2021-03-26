//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model.Metadata
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.XrmHttp;

    public class AttributeMetadata : MetadataBase
    {
        [DataMember(Name = "AttributeOf")]
        public string AttributeOf { get; set; }
        
        [DataMember(Name = "AttributeType")]
        public AttributeTypeCode? AttributeType { get; set; }
        
        [DataMember(Name = "AttributeTypeName")]
        public AttributeTypeDisplayName AttributeTypeName { get; set; }
        
        [DataMember(Name = "ColumnNumber")]
        public int? ColumnNumber { get; set; }
        
        [DataMember(Name = "Description")]
        public Label Description { get; set; }
        
        [DataMember(Name = "DisplayName")]
        public Label DisplayName { get; set; }
        
        [DataMember(Name = "DeprecatedVersion")]
        public string DeprecatedVersion { get; set; }
        
        [DataMember(Name = "IntroducedVersion")]
        public string IntroducedVersion { get; set; }
        
        [DataMember(Name = "EntityLogicalName")]
        public string EntityLogicalName { get; set; }
        
        [DataMember(Name = "IsAuditEnabled")]
        public BooleanManagedProperty IsAuditEnabled { get; set; }
        
        [DataMember(Name = "IsCustomAttribute")]
        public bool? IsCustomAttribute { get; set; }
        
        [DataMember(Name = "IsPrimaryId")]
        public bool? IsPrimaryId { get; set; }
        
        [DataMember(Name = "IsPrimaryName")]
        public bool? IsPrimaryName { get; set; }
        
        [DataMember(Name = "IsValidForCreate")]
        public bool? IsValidForCreate { get; set; }
        
        [DataMember(Name = "IsValidForRead")]
        public bool? IsValidForRead { get; set; }
        
        [DataMember(Name = "IsValidForUpdate")]
        public bool? IsValidForUpdate { get; set; }
        
        [DataMember(Name = "CanBeSecuredForRead")]
        public bool? CanBeSecuredForRead { get; set; }
        
        [DataMember(Name = "CanBeSecuredForCreate")]
        public bool? CanBeSecuredForCreate { get; set; }
        
        [DataMember(Name = "CanBeSecuredForUpdate")]
        public bool? CanBeSecuredForUpdate { get; set; }
        
        [DataMember(Name = "IsSecured")]
        public bool? IsSecured { get; set; }
        
        [DataMember(Name = "IsRetrievable")]
        public bool? IsRetrievable { get; set; }
        
        [DataMember(Name = "IsFilterable")]
        public bool? IsFilterable { get; set; }
        
        [DataMember(Name = "IsSearchable")]
        public bool? IsSearchable { get; set; }
        
        [DataMember(Name = "IsManaged")]
        public bool? IsManaged { get; set; }
        
        [DataMember(Name = "IsGlobalFilterEnabled")]
        public BooleanManagedProperty IsGlobalFilterEnabled { get; set; }
        
        [DataMember(Name = "IsSortableEnabled")]
        public BooleanManagedProperty IsSortableEnabled { get; set; }
        
        [DataMember(Name = "LinkedAttributeId")]
        public Guid? LinkedAttributeId { get; set; }
        
        [DataMember(Name = "LogicalName")]
        public string LogicalName { get; set; }
        
        [DataMember(Name = "IsCustomizable")]
        public BooleanManagedProperty IsCustomizable { get; set; }
        
        [DataMember(Name = "IsRenameable")]
        public BooleanManagedProperty IsRenameable { get; set; }
        
        [DataMember(Name = "IsValidForAdvancedFind")]
        public BooleanManagedProperty IsValidForAdvancedFind { get; set; }
        
        [DataMember(Name = "IsValidForForm")]
        public bool? IsValidForForm { get; set; }
        
        [DataMember(Name = "IsRequiredForForm")]
        public bool? IsRequiredForForm { get; set; }
        
        [DataMember(Name = "IsValidForGrid")]
        public bool? IsValidForGrid { get; set; }
        
        [DataMember(Name = "RequiredLevel")]
        public AttributeRequiredLevelManagedProperty RequiredLevel { get; set; }
        
        [DataMember(Name = "CanModifyAdditionalSettings")]
        public BooleanManagedProperty CanModifyAdditionalSettings { get; set; }
        
        [DataMember(Name = "SchemaName")]
        public string SchemaName { get; set; }
        
        [DataMember(Name = "ExternalName")]
        public string ExternalName { get; set; }
        
        [DataMember(Name = "IsLogical")]
        public bool? IsLogical { get; set; }
        
        [DataMember(Name = "IsDataSourceSecret")]
        public bool? IsDataSourceSecret { get; set; }
        
        [DataMember(Name = "InheritsFrom")]
        public string InheritsFrom { get; set; }
        
        [DataMember(Name = "SourceType")]
        public int? SourceType { get; set; }
        
        [DataMember(Name = "AutoNumberFormat")]
        public string AutoNumberFormat { get; set; }
    }
}
