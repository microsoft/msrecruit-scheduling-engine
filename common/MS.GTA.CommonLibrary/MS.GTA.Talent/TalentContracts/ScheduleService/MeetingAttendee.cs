// <copyright file="MeetingAttendee.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The meeting attendee.
    /// </summary>
    [DataContract]
    public class MeetingAttendee
    {
        /// <summary>
        /// Gets or sets the attendee type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the attendee email address.
        /// </summary>
        [DataMember(Name = "emailAddress")]
        public MeetingAttendeeEmailAddress EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the meeting attendee status
        /// </summary>
        [DataMember(Name = "status")]
        public MeetingAttendeeStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the proposed new time for the meeting.
        /// </summary>
        /// <value>
        /// The instance of <see cref="MeetingTimeSpan"/>.
        /// </value>
        [DataMember(Name="proposedNewTime", IsRequired = false)]
        public MeetingTimeSpan ProposedNewTime { get; set; }
    }
}
