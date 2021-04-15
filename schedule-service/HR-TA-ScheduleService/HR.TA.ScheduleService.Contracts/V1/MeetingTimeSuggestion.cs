//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The meeting time suggestion.
    /// </summary>
    [DataContract]
    public class MeetingTimeSuggestion
    {
        /// <summary>
        /// Gets or sets the meeting time slot suggestion.
        /// </summary>
        [DataMember(Name = "meetingTimeSlot")]
        public MeetingTimeSlot MeetingTimeSlot { get; set; }

        /// <summary>
        /// Gets or sets the confidence of the meeting time suggestion.
        /// </summary>
        [DataMember(Name = "confidence")]
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the organizer's availability.
        /// </summary>
        [DataMember(Name = "organizerAvailability")]
        public string OrganizerAvailability { get; set; }

        /// <summary>
        /// Gets or sets the attendees' availabilities for the meeting time slot suggestion.
        /// </summary>
        [DataMember(Name = "attendeeAvailability")]
        public List<MeetingAttendeeAvailability> AttendeeAvailability { get; set; }

        /// <summary>
        /// Gets or sets the meeting time slot locations.
        /// </summary>
        [DataMember(Name = "locations")]
        public List<MeetingLocation> Locations { get; set; }
    }
}
