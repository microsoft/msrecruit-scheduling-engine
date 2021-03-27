//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The attendee availability.
    /// </summary>
    [DataContract]
    public class MeetingAttendeeAvailability
    {
        /// <summary>
        /// Gets or sets the attendee for a meeting.
        /// </summary>
        [DataMember(Name = "attendee")]
        public MeetingAttendee Attendee { get; set; }

        /// <summary>
        /// Gets or sets the availability of an attendee.
        /// </summary>
        [DataMember(Name = "availability")]
        public string Availability { get; set; }
    }
}
