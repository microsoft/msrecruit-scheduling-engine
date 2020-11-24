//----------------------------------------------------------------------------
// <copyright file="MeetingInfo.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Talent.EnumSetModel;

    /// <summary>
    /// A meeting event
    /// </summary>
    [DataContract]
    public class MeetingInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingInfo"/> class.
        /// </summary>
        public MeetingInfo()
        {
        }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the users
        /// </summary>
        [DataMember(Name = "userGroups")]
        public UserGroup UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the requester
        /// </summary>
        [DataMember(Name = "requester", IsRequired = false, EmitDefaultValue = false)]
        public GraphPerson Requester { get; set; }

        /// <summary>
        /// Gets or sets the schedule event id
        /// </summary>
        [DataMember(Name = "scheduleEventId", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleEventId { get; set; }

        /// <summary>
        /// Gets or sets the schedule event id
        /// </summary>
        [DataMember(Name = "scheduleStatus", IsRequired = false, EmitDefaultValue = false)]
        public ScheduleStatus ScheduleStatus { get; set; }

        /// <summary>
        /// Gets or sets the list of meeting details
        /// </summary>
        [DataMember(Name = "meetingDetails")]
        public List<MeetingDetails> MeetingDetails { get; set; }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        [DataMember(Name = "tenantId", IsRequired = false, EmitDefaultValue = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        [DataMember(Name = "scheduleOrder", IsRequired = false, EmitDefaultValue = false)]
        public int ScheduleOrder { get; set; }

        /// <summary>
        /// Gets or sets the Interviewer TimeSlot Id.
        /// </summary>
        [DataMember(Name = "InterviewerTimeSlotId")]
        public string InterviewerTimeSlotId { get; set; }
    }
}
