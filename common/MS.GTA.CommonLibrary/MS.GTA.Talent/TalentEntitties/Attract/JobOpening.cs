//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_jobopenings", SingularName = "msdyn_jobopening")]
    public class JobOpening : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobopeningid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_jobopeningstatus")]
        public JobOpeningStatus? Status { get; set; }

        [DataMember(Name = "msdyn_jobopeningstatusreason")]
        public JobOpeningStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_applicationstartdate")]
        public DateTime? ApplicationStartDate { get; set; }

        [DataMember(Name = "msdyn_applicationclosedate")]
        public DateTime? ApplicationCloseDate { get; set; }

        [DataMember(Name = "msdyn_applyurl")]
        public string ApplyURL { get; set; }

        [DataMember(Name = "_msdyn_businessunitid_value")]
        public Guid? BusinessUnitId { get; set; }

        ////[DataMember(Name = "msdyn_BusinessunitId")]
        ////public BusinessUnit BusinessUnit { get; set; }

        [DataMember(Name = "msdyn_comment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_istemplate")]
        public bool IsTemplate { get; set; }

        [DataMember(Name = "msdyn_jobstartdate")]
        public DateTime? JobStartDate { get; set; }

        [DataMember(Name = "msdyn_jobenddate")]
        public DateTime? JobEndDate { get; set; }

        [DataMember(Name = "msdyn_joblocation")]
        public string JobLocation { get; set; }

        [DataMember(Name = "msdyn_jobtitle")]
        public string JobTitle { get; set; }

        [DataMember(Name = "msdyn_numberofopenings")]
        public int? NumberOfOpenings { get; set; }

        [DataMember(Name = "msdyn_qualificationsummary")]
        public string QualificationSummary { get; set; }

        [DataMember(Name = "msdyn_responsibilities")]
        public string Responsibilities { get; set; }

        [DataMember(Name = "msdyn_employmenttype")]
        public EmploymentType? EmploymentType { get; set; }

        [DataMember(Name = "msdyn_senioritylevel")]
        public JobOpeningSeniorityLevel? SeniorityLevel { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_travelpercentage")]
        public int? TravelPercentage { get; set; }

        [DataMember(Name = "msdyn_additionalmetadata")]
        public string AdditionalMetadata { get; set; }
        
        [DataMember(Name = "msdyn_jobopening_jobopeningparticipant")]
        public IList<JobOpeningParticipant> JobOpeningParticipants { get; set; }
        
        [DataMember(Name = "msdyn_jobopening_jobposting")]
        public IList<JobPosting> JobPostings { get; set; }

        [DataMember(Name = "msdyn_jobopening_jobopeningstage")]
        public IList<JobOpeningStage> JobOpeningStages { get; set; }

        [DataMember(Name = "msdyn_jobopening_jobapplication")]
        public IList<JobApplication> JobApplications { get; set; }

        [DataMember(Name = "msdyn_jobopening_cdm_jobposition")]
        public IList<JobPosition> JobPositions { get; set; }
        // TODO

        [DataMember(Name = "msdyn_jobopening_jobskill")]
        public IList<JobSkill> Skills { get; set; }

        [DataMember(Name = "msdyn_jobopening_cdm_jobfunction")]
        public IList<JobFunction> Functions { get; set; }
        

        [DataMember(Name = "msdyn_jobopening_jobindustry")]
        public IList<JobIndustry> Industries { get; set; }

        [DataMember(Name = "msdyn_issyncedwithlinkedin")]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "msdyn_ishiringteamsyncedwithlinkedin")]
        public bool? IsHiringTeamSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "msdyn_addressline1")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "msdyn_addressline2")]
        public string AddressLine2 { get; set; }

        [DataMember(Name = "msdyn_addresscity")]
        public string AddressCity { get; set; }

        [DataMember(Name = "msdyn_addressstate")]
        public string AddressState { get; set; }

        [DataMember(Name = "msdyn_addresscountrycode")]
        public string AddressCountryCode { get; set; }

        [DataMember(Name = "msdyn_addresspostalcode")]
        public string AddressPostalCode { get; set; }

        [DataMember(Name = "msdyn_jobopeningconfiguration")]
        public string Configuration { get; set; }

        [DataMember(Name = "msdyn_jobopening_jobopeningapprovalparticipant")]
        public IList<JobOpeningApprovalParticipant> JobOpeningApprovalParticipants { get; set; }
        
        [DataMember(Name = "msdyn_jobactivationdate")]
        public DateTime? JobActivationDate { get; set; }

        [DataMember(Name = "msdyn_visibility")]
        public JobOpeningVisibility? Visibility { get; set; }

        [DataMember(Name = "msdyn_JobApprovalRequester")]
        public Worker JobApprovalRequester { get; set; }

        [DataMember(Name = "_msdyn_jobapprovalrequester_value")]
        public Guid? JobApprovalRequesterId { get; set; }

        [DataMember(Name = "msdyn_jobapprovalrequesterdate")]
        public DateTime? JobApprovalRequesterDate { get; set; }
    }
}
