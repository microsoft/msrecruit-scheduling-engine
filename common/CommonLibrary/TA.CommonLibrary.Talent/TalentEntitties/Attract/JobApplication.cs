//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.XrmHttp;
    using TA.CommonLibrary.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobapplications", SingularName = "msdyn_jobapplication")]
    public class JobApplication : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobapplicationid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobopeningid_value")]
        public Guid? JobOpeningId { get; set; }

        [DataMember(Name = "msdyn_JobopeningId")]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "msdyn_jobapplicationstatus")]
        public JobApplicationStatus? Status { get; set; }

        [DataMember(Name = "msdyn_jobapplicationstatusreason")]
        public JobApplicationStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_rejectionreason")]
        public int? RejectionReason { get; set; }

        [DataMember(Name = "_msdyn_currentjobopeningstageid_value")]
        public Guid? CurrentJobOpeningStageId { get; set; }

        [DataMember(Name = "msdyn_CurrentjobopeningstageId")]
        public JobOpeningStage CurrentJobOpeningStage { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_externalstatus")]
        public string ExternalStatus { get; set; }

        [DataMember(Name = "msdyn_comment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_applicationdate")]
        public DateTime? ApplicationDate { get; set; }

        [DataMember(Name = "msdyn_offeracceptdate")]
        public DateTime? OfferAcceptDate { get; set; }

        [DataMember(Name = "msdyn_isprospect")]
        public bool? IsProspect { get; set; }

        [DataMember(Name = "msdyn_invitationid")]
        public string InvitationId { get; set; }

        [DataMember(Name = "msdyn_additionalmetadata")]
        public string AdditionalMetadata { get; set; }

        [DataMember(Name = "_msdyn_jobpositionid_value")]
        public Guid? JobPositionId { get; set; }

        [DataMember(Name = "msdyn_jobpositionid")]
        public Common.JobPosition JobPosition { get; set; }

        [DataMember(Name = "msdyn_jobapplication_candidateeducation")]
        public IList<CandidateEducation> CandidateEducations { get; set; }

        [DataMember(Name = "msdyn_jobapplication_candidateworkexperience")]
        public IList<CandidateWorkExperience> CandidateWorkExperiences { get; set; }

        [DataMember(Name = "msdyn_jobapplication_candidateskill")]
        public IList<CandidateSkill> CandidateSkills { get; set; }

        [DataMember(Name = "msdyn_jobapplication_candidateartifact")]
        public IList<CandidateArtifact> CandidateArtifacts { get; set; }

        [DataMember(Name = "msdyn_jobapplication_jobapplicationcomment")]
        public IList<JobApplicationComment> JobApplicationComments { get; set; }
        
        [DataMember(Name = "msdyn_jobapplication_jobapplicationactivity")]
        public IList<JobApplicationActivity> JobApplicationActivities { get; set; }
        /*
        [DataMember(Name = "msdyn_jobapplication_jobapplicationhistory")]
        public IList<JobApplicationHistory> JobApplicationHistories { get; set; }

        [DataMember(Name = "msdyn_jobapplication_jobapplicationstagehistory")]
        public IList<JobApplicationStageHistory> JobApplicationStageHistories { get; set; }
        */
        
        [DataMember(Name = "msdyn_currentstagestatus")]
        public JobApplicationStageStatus? CurrentStageStatus { get; set; }

        [DataMember(Name = "msdyn_currentstagestatusreason")]
        public JobApplicationStageStatusReason? CurrentStageStatusReason { get; set; }

        [DataMember(Name = "msdyn_rank")]
        public Rank? Rank { get; set; }

        [DataMember(Name = "msdyn_issyncedwithlinkedin")]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "msdyn_istermsacceptedbycandidate")]
        public bool? IsTermsAcceptedByCandidate { get; set; }

        [DataMember(Name = "msdyn_isstagehistorysyncedwithlinkedin")]
        public bool? IsStageHistorySyncedWithLinkedIn { get; set; }

        [DataMember(Name = "msdyn_jobapplication_jobapplicationoffernote")]
        public IList<JobApplicationOfferNote> JobApplicationOfferNotes { get; set; }

        [DataMember(Name = "msdyn_jobapplication_talentsourcedetail")]
        public IList<TalentSourceDetail> TalentSourceDetails { get; set; }

    }
}
