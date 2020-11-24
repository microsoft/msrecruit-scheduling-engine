//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Activity.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Application activity data contract.
    /// </summary>
    [DataContract]
    public class Activity
    {       
        /// <summary>
        /// Gets or sets job stage.
        /// </summary>
        [DataMember(Name = "stage", IsRequired = false)]
        public JobStage Stage { get; set; }

        /// <summary>
        /// Gets or sets activity type.
        /// </summary>
        [DataMember(Name = "activityType", IsRequired = false)]
        public JobApplicationActivityType ActivityType { get; set; }

        /// <summary>
        /// Gets or sets location.
        /// </summary>
        [DataMember(Name = "location", IsRequired = false, EmitDefaultValue = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets planned start date and time.
        /// </summary>
        [DataMember(Name = "plannedStartTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? PlannedStartTime { get; set; }

        /// <summary>
        /// Gets or sets planned end date and time.
        /// </summary>
        [DataMember(Name = "plannedEndTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? PlannedEndTime { get; set; }

        /// <summary>
        /// Gets or sets job application activity status.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public JobApplicationActivityStatus Status { get; set; }

        /// <summary>
        /// Gets or sets job application status reason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public JobApplicationActivityStatusReason StatusReason { get; set; }
    }
}
