//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..TalentEntities.Enum;

    [DataContract]
    public class JobApplicationActivity
    {
        [DataMember(Name = "JobApplicationActivityID", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationActivityID { get; set; }

        [DataMember(Name = "JobApplicationStage", EmitDefaultValue = false, IsRequired = false)]
        public JobStage? JobApplicationStage { get; set; }

        [DataMember(Name = "ActivityType", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActivityType? ActivityType { get; set; }

        [DataMember(Name = "Location", EmitDefaultValue = false, IsRequired = false)]
        public string Location { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "PlannedStartDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? PlannedStartDateTime { get; set; }

        [DataMember(Name = "PlannedEndDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? PlannedEndDateTime { get; set; }

        [DataMember(Name = "ActualStartDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ActualStartDateTime { get; set; }

        [DataMember(Name = "ActualEndDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ActualEndDateTime { get; set; }

        [DataMember(Name = "DueDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? DueDateTime { get; set; }

        [DataMember(Name = "IsEnabledForCandidate", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsEnabledForCandidate { get; set; }

        [DataMember(Name = "Required", EmitDefaultValue = false, IsRequired = false)]
        public Required? Required { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActivityStatus? Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActivityStatusReason? StatusReason { get; set; }

        [DataMember(Name = "JobApplication", EmitDefaultValue = false, IsRequired = false)]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "CommunicationThreadID", EmitDefaultValue = false, IsRequired = false)]
        public string CommunicationThreadID { get; set; }

        [DataMember(Name = "ScheduleEventReference", EmitDefaultValue = false, IsRequired = false)]
        public string ScheduleEventReference { get; set; }

        [DataMember(Name = "ScheduleState", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationScheduleState? ScheduleState { get; set; }

        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        [DataMember(Name = "JobApplicationActivityParticipants", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationActivityParticipant> JobApplicationActivityParticipants { get; set; }
    }
}
