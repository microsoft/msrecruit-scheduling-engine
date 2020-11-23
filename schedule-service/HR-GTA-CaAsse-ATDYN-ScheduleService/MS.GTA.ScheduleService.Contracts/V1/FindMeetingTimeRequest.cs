// <copyright file="FindMeetingTimeRequest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The find meeting time request.
    /// </summary>
    [DataContract]
    public class FindMeetingTimeRequest
    {
        /// <summary>
        /// Gets or sets the meeting attendees.
        /// </summary>
        [DataMember(Name = "attendees")]
        public List<MeetingAttendee> Attendees { get; set; }

        /// <summary>
        /// Gets or sets the meeting location constraint.
        /// </summary>
        [DataMember(Name = "locationConstraint")]
        public LocationConstraintModel LocationConstraint { get; set; }

        /// <summary>
        /// Gets or sets the meeting time constraint.
        /// </summary>
        [DataMember(Name = "timeConstraint")]
        public MeetingTimeConstraint TimeConstraint { get; set; }

        /// <summary>
        /// Gets or sets the meeting duration.
        /// </summary>
        [DataMember(Name = "meetingDuration")]
        public string MeetingDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of meeting time slot suggestions to return.
        /// </summary>
        [DataMember(Name = "maxCandidates")]
        public int MaxCandidates { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the meeting organizer is optional in the meeting.
        /// </summary>
        [DataMember(Name = "isOrganizerOptional")]
        public bool IsOrganizerOptional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to return a reason for the suggestion.
        /// </summary>
        [DataMember(Name = "returnSuggestionReasons")]
        public bool ReturnSuggestionReasons { get; set; }

        /// <summary>
        /// Gets or sets the minimum percentage of attendees that must be free for any suggestion.
        /// </summary>
        [DataMember(Name = "minimumAttendeePercentage")]
        public double MinimumAttendeePercentage { get; set; }
    }
}
