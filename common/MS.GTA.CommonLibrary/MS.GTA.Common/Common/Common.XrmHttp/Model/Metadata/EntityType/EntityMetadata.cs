//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "EntityDefinitions", SingularName = "EntityMetadata")]
    public class EntityMetadata : MetadataBase
    {
        [DataMember(Name = "ActivityTypeMask")]
        public int? ActivityTypeMask { get; set; }

        [DataMember(Name = "AutoRouteToOwnerQueue")]
        public bool? AutoRouteToOwnerQueue { get; set; }

        [DataMember(Name = "CanTriggerWorkflow")]
        public bool? CanTriggerWorkflow { get; set; }

        [DataMember(Name = "Description")]
        public Label Description { get; set; }

        [DataMember(Name = "DisplayCollectionName")]
        public Label DisplayCollectionName { get; set; }

        [DataMember(Name = "DisplayName")]
        public Label DisplayName { get; set; }

        [DataMember(Name = "EntityHelpUrlEnabled")]
        public bool? EntityHelpUrlEnabled { get; set; }

        [DataMember(Name = "EntityHelpUrl")]
        public string EntityHelpUrl { get; set; }

        [DataMember(Name = "IsDocumentManagementEnabled")]
        public bool? IsDocumentManagementEnabled { get; set; }

        [DataMember(Name = "IsOneNoteIntegrationEnabled")]
        public bool? IsOneNoteIntegrationEnabled { get; set; }

        [DataMember(Name = "IsInteractionCentricEnabled")]
        public bool? IsInteractionCentricEnabled { get; set; }

        [DataMember(Name = "IsKnowledgeManagementEnabled")]
        public bool? IsKnowledgeManagementEnabled { get; set; }

        [DataMember(Name = "IsSLAEnabled")]
        public bool? IsSLAEnabled { get; set; }

        [DataMember(Name = "IsBPFEntity")]
        public bool? IsBPFEntity { get; set; }

        [DataMember(Name = "IsDocumentRecommendationsEnabled")]
        public bool? IsDocumentRecommendationsEnabled { get; set; }

        [DataMember(Name = "IsMSTeamsIntegrationEnabled")]
        public bool? IsMSTeamsIntegrationEnabled { get; set; }

        [DataMember(Name = "DataProviderId")]
        public Guid? DataProviderId { get; set; }

        [DataMember(Name = "DataSourceId")]
        public Guid? DataSourceId { get; set; }

        [DataMember(Name = "AutoCreateAccessTeams")]
        public bool? AutoCreateAccessTeams { get; set; }

        [DataMember(Name = "IsActivity")]
        public bool? IsActivity { get; set; }

        [DataMember(Name = "IsActivityParty")]
        public bool? IsActivityParty { get; set; }

        [DataMember(Name = "IsAuditEnabled")]
        public BooleanManagedProperty IsAuditEnabled { get; set; }

        [DataMember(Name = "IsAvailableOffline")]
        public bool? IsAvailableOffline { get; set; }

        [DataMember(Name = "IsChildEntity")]
        public bool? IsChildEntity { get; set; }

        [DataMember(Name = "IsAIRUpdated")]
        public bool? IsAIRUpdated { get; set; }

        [DataMember(Name = "IsValidForQueue")]
        public BooleanManagedProperty IsValidForQueue { get; set; }

        [DataMember(Name = "IsConnectionsEnabled")]
        public BooleanManagedProperty IsConnectionsEnabled { get; set; }

        [DataMember(Name = "IconLargeName")]
        public string IconLargeName { get; set; }

        [DataMember(Name = "IconMediumName")]
        public string IconMediumName { get; set; }

        [DataMember(Name = "IconSmallName")]
        public string IconSmallName { get; set; }

        [DataMember(Name = "IconVectorName")]
        public string IconVectorName { get; set; }

        [DataMember(Name = "IsCustomEntity")]
        public bool? IsCustomEntity { get; set; }

        [DataMember(Name = "IsBusinessProcessEnabled")]
        public bool? IsBusinessProcessEnabled { get; set; }

        [DataMember(Name = "IsCustomizable")]
        public BooleanManagedProperty IsCustomizable { get; set; }

        [DataMember(Name = "IsRenameable")]
        public BooleanManagedProperty IsRenameable { get; set; }

        [DataMember(Name = "IsMappable")]
        public BooleanManagedProperty IsMappable { get; set; }

        [DataMember(Name = "IsDuplicateDetectionEnabled")]
        public BooleanManagedProperty IsDuplicateDetectionEnabled { get; set; }

        [DataMember(Name = "CanCreateAttributes")]
        public BooleanManagedProperty CanCreateAttributes { get; set; }

        [DataMember(Name = "CanCreateForms")]
        public BooleanManagedProperty CanCreateForms { get; set; }

        [DataMember(Name = "CanCreateViews")]
        public BooleanManagedProperty CanCreateViews { get; set; }

        [DataMember(Name = "CanCreateCharts")]
        public BooleanManagedProperty CanCreateCharts { get; set; }

        [DataMember(Name = "CanBeRelatedEntityInRelationship")]
        public BooleanManagedProperty CanBeRelatedEntityInRelationship { get; set; }

        [DataMember(Name = "CanBePrimaryEntityInRelationship")]
        public BooleanManagedProperty CanBePrimaryEntityInRelationship { get; set; }

        [DataMember(Name = "CanBeInManyToMany")]
        public BooleanManagedProperty CanBeInManyToMany { get; set; }

        [DataMember(Name = "CanBeInCustomEntityAssociation")]
        public BooleanManagedProperty CanBeInCustomEntityAssociation { get; set; }

        [DataMember(Name = "CanEnableSyncToExternalSearchIndex")]
        public BooleanManagedProperty CanEnableSyncToExternalSearchIndex { get; set; }

        [DataMember(Name = "SyncToExternalSearchIndex")]
        public bool? SyncToExternalSearchIndex { get; set; }

        [DataMember(Name = "CanModifyAdditionalSettings")]
        public BooleanManagedProperty CanModifyAdditionalSettings { get; set; }

        [DataMember(Name = "CanChangeHierarchicalRelationship")]
        public BooleanManagedProperty CanChangeHierarchicalRelationship { get; set; }

        [DataMember(Name = "IsOptimisticConcurrencyEnabled")]
        public bool? IsOptimisticConcurrencyEnabled { get; set; }

        [DataMember(Name = "ChangeTrackingEnabled")]
        public bool? ChangeTrackingEnabled { get; set; }

        [DataMember(Name = "CanChangeTrackingBeEnabled")]
        public BooleanManagedProperty CanChangeTrackingBeEnabled { get; set; }

        [DataMember(Name = "IsImportable")]
        public bool? IsImportable { get; set; }

        [DataMember(Name = "IsIntersect")]
        public bool? IsIntersect { get; set; }

        [DataMember(Name = "IsMailMergeEnabled")]
        public BooleanManagedProperty IsMailMergeEnabled { get; set; }

        [DataMember(Name = "IsManaged")]
        public bool? IsManaged { get; set; }

        [DataMember(Name = "IsEnabledForCharts")]
        public bool? IsEnabledForCharts { get; set; }

        [DataMember(Name = "IsEnabledForTrace")]
        public bool? IsEnabledForTrace { get; set; }

        [DataMember(Name = "IsValidForAdvancedFind")]
        public bool? IsValidForAdvancedFind { get; set; }

        [DataMember(Name = "IsVisibleInMobile")]
        public BooleanManagedProperty IsVisibleInMobile { get; set; }

        [DataMember(Name = "IsVisibleInMobileClient")]
        public BooleanManagedProperty IsVisibleInMobileClient { get; set; }

        [DataMember(Name = "IsReadOnlyInMobileClient")]
        public BooleanManagedProperty IsReadOnlyInMobileClient { get; set; }

        [DataMember(Name = "IsOfflineInMobileClient")]
        public BooleanManagedProperty IsOfflineInMobileClient { get; set; }

        [DataMember(Name = "DaysSinceRecordLastModified")]
        public int? DaysSinceRecordLastModified { get; set; }

        [DataMember(Name = "MobileOfflineFilters")]
        public string MobileOfflineFilters { get; set; }

        [DataMember(Name = "IsReadingPaneEnabled")]
        public bool? IsReadingPaneEnabled { get; set; }

        [DataMember(Name = "IsQuickCreateEnabled")]
        public bool? IsQuickCreateEnabled { get; set; }

        [DataMember(Name = "LogicalName")]
        public string LogicalName { get; set; }

        [DataMember(Name = "ObjectTypeCode")]
        public int? ObjectTypeCode { get; set; }

        [DataMember(Name = "OwnershipType")]
        public OwnershipTypes? OwnershipType { get; set; }

        [DataMember(Name = "PrimaryNameAttribute")]
        public string PrimaryNameAttribute { get; set; }

        [DataMember(Name = "PrimaryImageAttribute")]
        public string PrimaryImageAttribute { get; set; }

        [DataMember(Name = "PrimaryIdAttribute")]
        public string PrimaryIdAttribute { get; set; }

        [DataMember(Name = "RecurrenceBaseEntityLogicalName")]
        public string RecurrenceBaseEntityLogicalName { get; set; }

        [DataMember(Name = "ReportViewName")]
        public string ReportViewName { get; set; }

        [DataMember(Name = "SchemaName")]
        public string SchemaName { get; set; }

        [DataMember(Name = "IntroducedVersion")]
        public string IntroducedVersion { get; set; }

        [DataMember(Name = "IsStateModelAware")]
        public bool? IsStateModelAware { get; set; }

        [DataMember(Name = "EnforceStateTransitions")]
        public bool? EnforceStateTransitions { get; set; }

        [DataMember(Name = "ExternalName")]
        public string ExternalName { get; set; }

        [DataMember(Name = "EntityColor")]
        public string EntityColor { get; set; }

        [DataMember(Name = "LogicalCollectionName")]
        public string LogicalCollectionName { get; set; }

        [DataMember(Name = "ExternalCollectionName")]
        public string ExternalCollectionName { get; set; }

        [DataMember(Name = "CollectionSchemaName")]
        public string CollectionSchemaName { get; set; }

        [DataMember(Name = "EntitySetName")]
        public string EntitySetName { get; set; }

        [DataMember(Name = "IsEnabledForExternalChannels")]
        public bool? IsEnabledForExternalChannels { get; set; }

        [DataMember(Name = "IsPrivate")]
        public bool? IsPrivate { get; set; }

        [DataMember(Name = "UsesBusinessDataLabelTable")]
        public bool? UsesBusinessDataLabelTable { get; set; }

        [DataMember(Name = "IsLogicalEntity")]
        public bool? IsLogicalEntity { get; set; }

        [DataMember(Name = "HasNotes")]
        public bool? HasNotes { get; set; }

        [DataMember(Name = "HasActivities")]
        public bool? HasActivities { get; set; }

        [DataMember(Name = "HasFeedback")]
        public bool? HasFeedback { get; set; }

        //// [DataMember(Name = "Privileges")]
        //// public IList<SecurityPrivilegeMetadata> Privileges { get; set; }

        [DataMember(Name = "Attributes")]
        public IList<AttributeMetadata> Attributes { get; set; }

        //// [DataMember(Name="ManyToManyRelationships")]
        //// public IList<ManyToManyRelationshipMetadata> ManyToManyRelationships { get; set; }

        //// [DataMember(Name="ManyToOneRelationships")]
        //// public IList<OneToManyRelationshipMetadata> ManyToOneRelationships { get; set; }

        //// [DataMember(Name="OneToManyRelationships")]
        //// public IList<OneToManyRelationshipMetadata> OneToManyRelationships { get; set; }

        //// [DataMember(Name="Keys")]
        //// public IList<EntityKeyMetadata> Keys { get; set; }
    }
}