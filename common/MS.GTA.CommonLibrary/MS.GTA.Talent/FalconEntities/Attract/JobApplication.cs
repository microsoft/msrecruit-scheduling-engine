//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.FalconEntities;
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.Common.OfferManagement.Contracts.V2;

    [DataContract]
    public class JobApplication : DocDbEntity
    {
        [DataMember(Name = "JobApplicationId", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationID { get; set; }

        [DataMember(Name = "JobOpening", EmitDefaultValue = false, IsRequired = false)]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "Candidate", EmitDefaultValue = false, IsRequired = false)]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStatus? Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStatusReason? StatusReason { get; set; }

        [DataMember(Name = "CurrentJobOpeningStage", EmitDefaultValue = false, IsRequired = false)]
        public JobStage? CurrentJobOpeningStage { get; set; }

        [DataMember(Name = "CurrentJobApplicationStageStatus", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStageStatus? CurrentJobApplicationStageStatus { get; set; }

        [DataMember(Name = "CurrentJobApplicationStageStatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStageStatusReason? CurrentJobApplicationStageStatusReason { get; set; }

        [DataMember(Name = "ExternalJobApplicationID", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalJobApplicationID { get; set; }

        [DataMember(Name = "ExternalJobApplicationSource", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationExternalSource? ExternalJobApplicationSource { get; set; }

        [DataMember(Name = "ExternalStatus", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalStatus { get; set; }

        [DataMember(Name = "IsProspect", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsProspect { get; set; }

        [DataMember(Name = "NotifyCandidate", EmitDefaultValue = false, IsRequired = false)]
        public bool? NotifyCandidate { get; set; }

        [DataMember(Name = "InvitationID", EmitDefaultValue = false, IsRequired = false)]
        public string InvitationID { get; set; }

        [DataMember(Name = "ApplicationDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ApplicationDate { get; set; }

        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        [DataMember(Name = "CurrentActivity", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationCurrentActivity? CurrentActivity { get; set; }

        [DataMember(Name = "OfferAcceptDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? OfferAcceptDate { get; set; }

        [DataMember(Name = "JobApplicationActivities", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationActivity> JobApplicationActivities { get; set; }

        [DataMember(Name = "JobApplicationParticipants", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationParticipant> JobApplicationParticipants { get; set; }

        [DataMember(Name = "JobApplicationSchedules", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationSchedule> JobApplicationSchedules { get; set; }

        [DataMember(Name = "JobApplicationComments", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationComment> JobApplicationComments { get; set; }

        [DataMember(Name = "JobOpeningStageCurrent", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningStage JobOpeningStageCurrent { get; set; }

        [DataMember(Name = "IsSyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "IsStageHistorySyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsStageHistorySyncedWithLinkedIn { get; set; }

        [DataMember(Name = "JobOpeningPosition", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningPosition JobOpeningPosition { get; set; }

        [DataMember(Name = "JobApplicationExtendedAttributes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CustomAttributes> JobApplicationExtendedAttributes { get; set; }

        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateNote> Notes { get; set; }

        [DataMember(Name = "IsScheduleSentToCandidate", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsScheduleSentToCandidate { get; set; }
    }
}
