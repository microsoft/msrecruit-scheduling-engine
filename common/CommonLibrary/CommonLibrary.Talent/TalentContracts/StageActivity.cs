//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.Contracts;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The job opening stage activity.
    /// </summary>
    [DataContract]
    public class StageActivity : TalentBaseContract
    {
        /// <summary>
        /// Gets or sets job opening stage activity id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets display label.
        /// </summary>
        [DataMember(Name = "displayName", IsRequired = false, EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets ordinal.
        /// </summary>
        [DataMember(Name = "ordinal", IsRequired = false, EmitDefaultValue = false)]
        public int? Ordinal { get; set; }

        /// <summary>
        /// Gets or sets sub ordinal.
        /// </summary>
        [DataMember(Name = "subOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public int? SubOrdinal { get; set; }

        /// <summary>
        /// Gets or sets audience for activity
        /// </summary>
        [DataMember(Name = "audience", IsRequired = false, EmitDefaultValue = false)]
        public ActivityAudience? Audience { get; set; }

        /// <summary>
        /// Gets or sets ordinal.
        /// </summary>
        [DataMember(Name = "isEnableForCandidate", IsRequired = false, EmitDefaultValue = false)]
        public bool IsEnableForCandidate { get; set; }

        /// <summary>
        /// Gets or sets display name.
        /// </summary>
        [DataMember(Name = "activityType", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationActivityType ActivityType { get; set; }

        /// <summary>
        /// Gets or sets display name.
        /// </summary>
        [DataMember(Name = "configuration", IsRequired = false, EmitDefaultValue = false)]
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets schedule event id.
        /// </summary>
        [DataMember(Name = "scheduleEventId", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleEventId { get; set; }

        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets Job application activity status
        /// </summary>
        [DataMember(Name = "activityStatus", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationActivityStatus ActivityStatus { get; set; }

        /// <summary>
        /// Gets or sets application hiring team.
        /// </summary>
        [DataMember(Name = "participants", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<HiringTeamMember> Participants { get; set; }

        /// <summary>
        /// Gets or sets activity due date time.
        /// </summary>
        [DataMember(Name = "dueDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? DueDateTime { get; set; }

        /// <summary>
        /// Gets or sets activity planned start date.
        /// </summary>
        [DataMember(Name = "plannedStartDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? PlannedStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets required enum for activity.
        /// </summary>
        [DataMember(Name = "required", IsRequired = false, EmitDefaultValue = false)]
        public Required? Required { get; set; }
    }
}
