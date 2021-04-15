//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// Interview schedule event 
    /// </summary>
    [DataContract]
    public class StageScheduleEvent
    {
        /// <summary>
        /// Gets or sets job application id
        /// </summary>
        [DataMember(Name = "JobApplicationId")]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets interview team member
        /// </summary>
        [DataMember(Name = "TeamMembers")]
        public IList<TeamMember> TeamMembers { get; set; }

        /// <summary>
        /// Gets or sets job stage
        /// </summary>
        [DataMember(Name = "Stage")]
        public JobStage Stage { get; set; }

        /// <summary>
        /// Gets or sets job stage
        /// </summary>
        [DataMember(Name = "StageOrder", IsRequired = false)]
        public int StageOrder { get; set; }

        /// <summary>
        /// Gets or sets location
        /// </summary>
        [DataMember(Name = "Location", IsRequired = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets comment
        /// </summary>
        [DataMember(Name = "Comment", IsRequired = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets schedule event Id.
        /// </summary>
        [DataMember(Name = "ScheduleEventId")]
        public string ScheduleEventId { get; set; }

        /// <summary>
        /// Gets or sets schedule state.
        /// </summary>
        [DataMember(Name = "ScheduleState")]
        public JobApplicationScheduleState ScheduleState { get; set; }

        /// <summary>
        /// Gets or sets schedule type.
        /// </summary>
        [DataMember(Name = "ScheduleType", IsRequired = false, EmitDefaultValue = false)]
        public int ScheduleType { get; set; }

        /// <summary>
        /// Gets or sets schedule availabilities.
        /// </summary>
        [DataMember(Name = "ScheduleAvailabilities", IsRequired = false, EmitDefaultValue = false)]
        public IList<ScheduleAvailability> ScheduleAvailabilities { get; set; }

        /// <summary>
        /// Gets or sets dates.
        /// </summary>
        [DataMember(Name = "ScheduleDates", IsRequired = false, EmitDefaultValue = false)]
        public string[] ScheduleDates { get; set; }

        /// <summary>
        /// Gets or sets time zone name.
        /// </summary>
        [DataMember(Name = "TimezoneName", IsRequired = false, EmitDefaultValue = false)]
        public string TimezoneName { get; set; }
    }
}
