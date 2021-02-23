//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The meeting timeslot constraints.
    /// </summary>
    [DataContract]
    public class MeetingTimeConstraint
    {
        /// <summary>
        /// Gets or sets the activity domain for the request. "work" = working hours, "personal" = working hours plus weekends, "unlimited" = 24 hours
        /// </summary>
        [DataMember(Name = "activityDomain", IsRequired = false, EmitDefaultValue = false)]
        public string ActivityDomain { get; set; }

        /// <summary>
        /// Gets or sets the time slots for meeting.
        /// </summary>
        [DataMember(Name = "timeslots")]
        public List<MeetingTimeSlot> Timeslots { get; set; }
    }
}
