//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    [DataContract]
    public class DashboardActivity
    {
        /// <summary>
        /// Gets or sets planned start date and time.
        /// </summary>
        [DataMember(Name = "startTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets planned end date and time.
        /// </summary>
        [DataMember(Name = "endTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets planned end date and time.
        /// </summary>
        [DataMember(Name = "jobApplicationActivityType", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationActivityType? JobApplicationActivityType { get; set; }

        /// <summary>
        /// Gets or sets activity
        /// </summary>
        [DataMember(Name = "activity", IsRequired = false, EmitDefaultValue = false)]
        public StageActivity Activity { get; set; }

        /// <summary>
        /// Gets or sets participants
        /// </summary>
        [DataMember(Name = "participants", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Person> Participants { get; set; }

        /// <summary>
        /// Gets or sets stage order
        /// </summary>
        [DataMember(Name = "stageOrder", IsRequired = false, EmitDefaultValue = false)]
        public long? StageOrder { get; set; }

        /// <summary>
        /// Gets or sets planned end date and time.
        /// </summary>
        [DataMember(Name = "applicant", IsRequired = false, EmitDefaultValue = false)]
        public Applicant Applicant { get; set; }

        /// <summary>
        /// Gets or sets job
        /// </summary>
        [DataMember(Name = "job", IsRequired = false, EmitDefaultValue = false)]
        public Job Job { get; set; }

        /// <summary>
        /// Gets or sets job application id
        /// </summary>
        [DataMember(Name = "jobApplicationId", IsRequired = false, EmitDefaultValue = false)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets job application stage
        /// </summary>
        [DataMember(Name = "jobApplicationStage", IsRequired = false, EmitDefaultValue = false)]
        public ApplicationStage JobApplicationStage { get; set; }

        /// <summary>
        /// Gets or sets attendees
        /// </summary>
        [DataMember(Name = "attendees", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<ScheduleAttendee> Attendees { get; set; }

        /// <summary>
        /// Gets or sets Flag that notify either activity is delegated
        /// </summary>
        [DataMember(Name = "isDelegated", IsRequired = false, EmitDefaultValue = false)]
        public bool IsDelegated { get; set; }
    }
}
