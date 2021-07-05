//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.TalentEntities.Enum;
    using HR.TA.Common.DocumentDB.Contracts;
    using HR.TA.Talent.FalconEntities;
    using HR.TA.Talent.FalconEntities.Attract;

    /// <summary>
    /// Job Application
    /// </summary>
    [DataContract]
    public class JobApplication : DocDbEntity
    {
        /// <summary>
        /// Job Application Id
        /// </summary>
        [DataMember(Name = "JobApplicationId", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationID { get; set; }

        /// <summary>
        /// Job Opening
        /// </summary>
        [DataMember(Name = "JobOpening", EmitDefaultValue = false, IsRequired = false)]
        public JobOpening JobOpening { get; set; }

        /// <summary>
        /// Candidate
        /// </summary>
        [DataMember(Name = "Candidate", EmitDefaultValue = false, IsRequired = false)]
        public Candidate Candidate { get; set; }

        /// <summary>
        /// Job Application Status
        /// </summary>
        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStatus? Status { get; set; }

        /// <summary>
        /// Job Application Status Reason
        /// </summary>
        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStatusReason? StatusReason { get; set; }

        /// <summary>
        /// Job Stage
        /// </summary>
        [DataMember(Name = "CurrentJobOpeningStage", EmitDefaultValue = false, IsRequired = false)]
        public JobStage? CurrentJobOpeningStage { get; set; }

        /// <summary>
        /// Job Application Stage Status
        /// </summary>
        [DataMember(Name = "CurrentJobApplicationStageStatus", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStageStatus? CurrentJobApplicationStageStatus { get; set; }

        /// <summary>
        /// Job Application Stage Status Reason
        /// </summary>
        [DataMember(Name = "CurrentJobApplicationStageStatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStageStatusReason? CurrentJobApplicationStageStatusReason { get; set; }

        /// <summary>
        /// External JobApplication ID
        /// </summary>
        [DataMember(Name = "ExternalJobApplicationID", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalJobApplicationID { get; set; }

        /// <summary>
        /// Job Application External Source
        /// </summary>
        [DataMember(Name = "ExternalJobApplicationSource", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationExternalSource? ExternalJobApplicationSource { get; set; }

        /// <summary>
        /// External Status
        /// </summary>
        [DataMember(Name = "ExternalStatus", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalStatus { get; set; }

        /// <summary>
        /// Is prospect
        /// </summary>
        [DataMember(Name = "IsProspect", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsProspect { get; set; }

        /// <summary>
        /// Notify Candidate
        /// </summary>
        [DataMember(Name = "NotifyCandidate", EmitDefaultValue = false, IsRequired = false)]
        public bool? NotifyCandidate { get; set; }

        /// <summary>
        /// Invitation Id
        /// </summary>
        [DataMember(Name = "InvitationID", EmitDefaultValue = false, IsRequired = false)]
        public string InvitationID { get; set; }

        /// <summary>
        /// Application Date
        /// </summary>
        [DataMember(Name = "ApplicationDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ApplicationDate { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Job Application Current Activity
        /// </summary>
        [DataMember(Name = "CurrentActivity", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationCurrentActivity? CurrentActivity { get; set; }

        /// <summary>
        /// Offer Accept Date
        /// </summary>
        [DataMember(Name = "OfferAcceptDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? OfferAcceptDate { get; set; }

        /// <summary>
        /// Job Application Activities
        /// </summary>
        [DataMember(Name = "JobApplicationActivities", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationActivity> JobApplicationActivities { get; set; }

        /// <summary>
        /// Job Application Participants
        /// </summary>
        [DataMember(Name = "JobApplicationParticipants", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationParticipant> JobApplicationParticipants { get; set; }

        /// <summary>
        /// Job Application Schedules
        /// </summary>
        [DataMember(Name = "JobApplicationSchedules", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationSchedule> JobApplicationSchedules { get; set; }

        /// <summary>
        /// Job Application Comments
        /// </summary>
        [DataMember(Name = "JobApplicationComments", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationComment> JobApplicationComments { get; set; }

        /// <summary>
        /// Job Opening Stage Current
        /// </summary>
        [DataMember(Name = "JobOpeningStageCurrent", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningStage JobOpeningStageCurrent { get; set; }

        /// <summary>
        /// Is Synced With Linked In
        /// </summary>
        [DataMember(Name = "IsSyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsSyncedWithLinkedIn { get; set; }

        /// <summary>
        /// IsStageHistorySyncedWithLinkedIn
        /// </summary>
        [DataMember(Name = "IsStageHistorySyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsStageHistorySyncedWithLinkedIn { get; set; }

        /// <summary>
        /// Job Opening Position
        /// </summary>
        [DataMember(Name = "JobOpeningPosition", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningPosition JobOpeningPosition { get; set; }

        /// <summary>
        /// Job Application Extended Attributes
        /// </summary>
        [DataMember(Name = "JobApplicationExtendedAttributes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CustomAttributes> JobApplicationExtendedAttributes { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateNote> Notes { get; set; }

        /// <summary>
        /// Is Schedule Sent To Candidate
        /// </summary>
        [DataMember(Name = "IsScheduleSentToCandidate", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsScheduleSentToCandidate { get; set; }
    }
}