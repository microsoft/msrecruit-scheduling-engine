//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.XrmHttp;
    using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract;
    using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "cdm_jobpositions", SingularName = "cdm_jobposition")]
    public class JobPosition : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_jobpositionid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_jobpositionnumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "cdm_description")]
        public string Description { get; set; }

        [DataMember(Name = "cdm_fulltimeequivalent")]
        public decimal? FullTimeEquivalent { get; set; }

        [DataMember(Name = "cdm_activation")]
        public DateTime? Activation { get; set; }

        [DataMember(Name = "cdm_availableforassignment")]
        public DateTime? AvailableForAssignment { get; set; }

        [DataMember(Name = "cdm_retirement")]
        public DateTime? Retirement { get; set; }

        [DataMember(Name = "cdm_validfrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "cdm_validto")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "msdyn_additionalmetadata")]
        public string AdditionalMetadata { get; set; }

        [DataMember(Name = "_cdm_jobid_value")]
        public Guid? JobId { get; set; }

        [DataMember(Name = "cdm_JobId")]
        public Job Job { get; set; }

        [DataMember(Name = "_cdm_parentjobpositionid_value")]
        public Guid? ParentJobPositionId { get; set; }

        [DataMember(Name = "cdm_ParentJobPositionId")]
        public JobPosition ParentJobPosition { get; set; }

        [DataMember(Name = "_cdm_positiontypeid_value")]
        public Guid? PositionTypeId { get; set; }

        [DataMember(Name = "cdm_PositionTypeId")]
        public JobPositionType PositionType { get; set; }

        [DataMember(Name = "msdyn_jobopening_cdm_jobposition")]
        public IList<JobOpening> JobOpenings { get; set; }

        [DataMember(Name = "cdm_parentjobposition_jobposition")]
        public IList<JobPosition> ChildJobPositions { get; set; }

        [DataMember(Name = "msdyn_cdm_jobposition_msdyn_jobapplication_jobpositionid")]
        public IList<JobApplication> JobApplications { get; set; }

        [DataMember(Name = "_cdm_departmentid_value")]
        public Guid? DepartmentId { get; set; }

        [DataMember(Name = "cdm_department")]
        public Department Department { get; set; }

        // Used to join Department table when using AddParentLinkEntity
        [Obsolete("Use DepartmentId with AddInnerJoin instead of AddParentLinkEntity", false)]
        [DataMember(Name = "cdm_departmentid")]
        public Department DepartmentMappingId { get; set; }

        [Obsolete("Use XrmStatusCode", false)]
        [DataMember(Name = "statuscode")]
        public StatusReason StatusReason { get; set; }

        [Obsolete("Use XrmStateCode", false)]
        [DataMember(Name = "statecode")]
        public Status Status { get; set; }

        /*
        <Property Name="cdm_title" Type="Edm.Int32" />

        <NavigationProperty Name="cdm_position_positionworkerassignment" Type="Collection(mscrm.cdm_positionworkerassignmentmap)" Partner="cdm_JobPositionId" />
        */
    }
}
