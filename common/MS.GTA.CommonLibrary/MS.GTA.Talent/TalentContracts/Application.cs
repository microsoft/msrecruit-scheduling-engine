//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Application.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Application data contract.
    /// </summary>
    [DataContract]
    public class Application : TalentBaseContract
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets candidate for the application.
        /// </summary>
        [DataMember(Name = "candidate", IsRequired = false, EmitDefaultValue = false)]
        public Applicant Candidate { get; set; }

        /// <summary>
        /// Gets or sets application hiring team.
        /// </summary>
        [DataMember(Name = "hiringTeam", IsRequired = false, EmitDefaultValue = false)]
        public IList<HiringTeamMember> HiringTeam { get; set; }

        /// <summary> 
        /// Gets or sets job external status. 
        /// </summary>
        [DataMember(Name = "externalStatus", IsRequired = false)]
        public string ExternalStatus { get; set; }

        /// <summary>
        /// Gets or sets status of the application.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public JobApplicationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets status reason of the JobApplication.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public JobApplicationStatusReason StatusReason { get; set; }

        /// <summary>
        /// Gets or sets the rejection reason.
        /// </summary>
        [DataMember(Name = "rejectionReason", IsRequired = false)]
        public OptionSetValue RejectionReason { get; set; }

        /// <summary>
        /// Gets or sets current application stage.
        /// </summary>
        [DataMember(Name = "currentStage", IsRequired = false)]
        public JobStage CurrentStage { get; set; }

        /// <summary>
        /// Gets or sets current application stage.
        /// </summary>
        [DataMember(Name = "currentApplicationStage", IsRequired = false)]
        public ApplicationStage CurrentApplicationStage { get; set; }

        /// <summary>
        /// Gets or sets status of the current job opening stage.
        /// </summary>
        [DataMember(Name = "currentStageStatus", IsRequired = false)]
        public JobApplicationStageStatus CurrentStageStatus { get; set; }

        /// <summary>
        /// Gets or sets status reason of the current job opening stage.
        /// </summary>
        [DataMember(Name = "currentStageStatusReason", IsRequired = false)]
        public JobApplicationStageStatusReason CurrentStageStatusReason { get; set; }

        /// <summary>
        /// Gets or sets status of the current applicant status.
        /// </summary>
        [DataMember(Name = "assessmentStatus", IsRequired = false)]
        public AssessmentStatus ApplicantAssessmentStatus { get; set; }

        /// <summary>
        /// Gets or sets a the invitation ID
        /// </summary>
        [DataMember(Name = "invitationId", IsRequired = false, EmitDefaultValue = false)]
        public string InvitationId { get; set; }

        /// <summary>
        /// Gets or sets application date.
        /// </summary>
        [DataMember(Name = "applicationDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets application comment.
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets application comment.
        /// </summary>
        [DataMember(Name = "schedules", IsRequired = false, EmitDefaultValue = false)]
        public IList<StageScheduleEvent> Schedules { get; set; }

        /// <summary>
        /// Gets or sets the external source.
        /// </summary>
        [DataMember(Name = "externalSource", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationExternalSource? ExternalSource { get; set; }

        /// <summary>
        /// Gets or sets the external id.
        /// </summary>
        [DataMember(Name = "externalId", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets application notes.
        /// </summary>
        [DataMember(Name = "notes", IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicationNote> Notes { get; set; }

        /// <summary>
        /// Gets or sets stages.
        /// </summary>
        [DataMember(Name = "stages", IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicationStage> Stages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is a prospect or not.    
        /// </summary>
        [DataMember(Name = "isProspect", IsRequired = false, EmitDefaultValue = false)]
        public bool IsProspect { get; set; }

        /// <summary>
        /// Gets or sets the position to the applicaiton
        /// </summary>
        [DataMember(Name = "jobOpeningPosition", IsRequired = false, EmitDefaultValue = false)]
        public JobOpeningPosition JobOpeningPosition { get; set; }

        /// <summary>
        /// Gets or sets ExtendedAttributes.
        /// </summary>
        [DataMember(Name = "extendedAttributes", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, string> ExtendedAttributes { get; set; }

        /// <summary>
        /// Gets or sets the list of permissions to the job application for the calling user.
        /// </summary>
        [DataMember(Name = "userPermissions", IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicationPermission> UserPermissions { get; set; }


        /// <summary>
        /// Gets or sets the Talent source
        /// </summary>
        [DataMember(Name = "applicationTalentSource", IsRequired = false, EmitDefaultValue = false)]
        public TalentSource ApplicationTalentSource { get; set; }
    }
}
