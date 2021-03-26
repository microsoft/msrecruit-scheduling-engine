//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA..Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA..Common.XrmHttp;
    using HR.TA..TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobapplicationactivities", SingularName = "msdyn_jobapplicationactivity")]
    public class JobApplicationActivity : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobapplicationactivityid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_JobapplicationId")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "_msdyn_jobopeningstageactivityid_value")]
        public Guid? JobOpeningStageActivityId { get; set; }

        [DataMember(Name = "msdyn_JobopeningstageactivityId")]
        public JobOpeningStageActivity JobOpeningStageActivity { get; set; }

        [DataMember(Name = "msdyn_jobapplicationactivitystatus")]
        public JobApplicationActivityStatus? Status { get; set; }

        [DataMember(Name = "msdyn_jobapplicationactivitystatusreason")]
        public JobApplicationActivityStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_location")]
        public string Location { get; set; }

        [DataMember(Name = "msdyn_comment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_scheduleeventreference")]
        public string ScheduleEventReference { get; set; }

        [DataMember(Name = "msdyn_activitytype")]
        public JobApplicationActivityType? ActivityType { get; set; }

        [DataMember(Name = "msdyn_duedate")]
        public DateTime? DueDate { get; set; }

        [DataMember(Name = "msdyn_plannedstart")]
        public DateTime? PlannedStart { get; set; }

        [DataMember(Name = "msdyn_plannedend")]
        public DateTime? PlannedEnd { get; set; }

        [DataMember(Name = "msdyn_actualstart")]
        public DateTime? ActualStart { get; set; }

        [DataMember(Name = "msdyn_actualend")]
        public DateTime? ActualEnd { get; set; }

        [DataMember(Name = "msdyn_isenabledforcandidate")]
        public bool? IsEnabledForCandidate { get; set; }

        [DataMember(Name = "msdyn_audience")]
        public ActivityAudience? Audience { get; set; }

        [DataMember(Name = "msdyn_required")]
        public Required? Required { get; set; }

        
        [DataMember(Name = "msdyn_jobapplicationactivity_jobapplicationacti")]
        public IList<JobApplicationActivityParticipant> JobApplicationActivityParticipants { get; set; }
        
        [DataMember(Name = "msdyn_jobapplicationactivity_jobapplactavail")]
        public IList<JobApplicationActivityAvailability> JobApplicationActivityAvailabilities { get; set; }
    }
}
