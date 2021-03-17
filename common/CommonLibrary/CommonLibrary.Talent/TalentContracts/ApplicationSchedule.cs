//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// Contract for Job closing. 
    /// </summary>
    [DataContract]
    public class ApplicationSchedule
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets stage.</summary>
        [DataMember(Name = "stage", IsRequired = false, EmitDefaultValue = false)]
        public JobStage Stage { get; set; }

        /// <summary>Gets or sets schedule event id.</summary>
        [DataMember(Name = "scheduleEventId", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleEventId { get; set; }

        /// <summary>Gets or sets schedule state.</summary>
        [DataMember(Name = "scheduleState", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationScheduleState ScheduleState { get; set; }

        /// <summary>Gets or sets application.</summary>
        [DataMember(Name = "application", IsRequired = false, EmitDefaultValue = false)]
        public Application Application { get; set; }

        /// <summary>Gets or sets stage.</summary>
        [DataMember(Name = "stageOrder", IsRequired = false, EmitDefaultValue = false)]
        public int StageOrder { get; set; }

        [DataMember(Name = "activityOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? ActivityOrdinal { get; set; }

        [DataMember(Name = "activitySubOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? ActivitySubOrdinal { get; set; }

        /// <summary>Gets or sets schedule availabilities.</summary>
        [DataMember(Name = "scheduleAvailabilities", IsRequired = false, EmitDefaultValue = false)]
        public IList<ScheduleAvailability> ScheduleAvailabilities { get; set; }
    }
}
