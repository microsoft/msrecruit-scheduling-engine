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
    using TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Common;
    // TODO
    // using TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer;
    using TA.CommonLibrary.TalentEntities.Enum;
    using TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer;

    [ODataEntity(PluralName = "msdyn_jobapplicationactivityparticipants", SingularName = "msdyn_jobapplicationactivityparticipant")]
    public class JobApplicationActivityParticipant : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobapplicationactivityparticipantid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationactivityid_value")]
        public Guid? JobApplicationActivityId { get; set; }

        [DataMember(Name = "msdyn_JobapplicationactivityId")]
        public JobApplicationActivity JobApplicationActivity { get; set; }

        [DataMember(Name = "_msdyn_jobopeningparticipantid_value")]
        public Guid? JobOpeningParticipantId { get; set; }
        
        [DataMember(Name = "msdyn_JobopeningparticipantId")]
        public JobOpeningParticipant JobOpeningParticipant { get; set; }
        
        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_ordinal")]
        public int Ordinal { get; set; }

        [DataMember(Name = "msdyn_feedbackproviderid")]
        public string FeedbackProviderId { get; set; }

        [DataMember(Name = "msdyn_overallcomment")]
        public string OverallComment { get; set; }

        [DataMember(Name = "msdyn_weaknesscomment")]
        public string WeaknessComment { get; set; }

        [DataMember(Name = "msdyn_strengthcomment")]
        public string StrengthComment { get; set; }

        [DataMember(Name = "msdyn_isrecommendedtocontinue")]
        public bool IsRecommendedToContinue { get; set; }

        [DataMember(Name = "msdyn_feedbackstatus")]
        public JobApplicationAssessmentStatus? Status { get; set; }

        [DataMember(Name = "msdyn_feedbackstatusreason")]
        public JobApplicationAssessmentStatusReason? StatusReason { get; set; }

        ////[DataMember(Name = "msdyn_jobapplicationactivityparticipant_jobappl")]
        ////public IList<JobApplicationActivityAssessment> JobApplicationActivityAssessments { get; set; }

        [DataMember(Name = "msdyn_jobappactivityparticipant_jobofferpartici")]
        public IList<JobOfferParticipant> JobOfferParticipants { get; set; }

        [DataMember(Name = "msdyn_jobapplicationactivityparticipant_meeting")]
        public IList<JobApplicationActivityParticipantMeeting> JobApplicationActivityParticipantMeetings { get; set; }

        [DataMember(Name = "msdyn_issyncedwithlinkedin")]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "_msdyn_feedbackproviderworkerid_value")]
        public Guid? FeedbackProviderWorkerId { get; set; }

        [DataMember(Name = "msdyn_FeedbackProviderWorkerId")]
        public Worker FeedbackProviderWorker { get; set; }
    }
}
