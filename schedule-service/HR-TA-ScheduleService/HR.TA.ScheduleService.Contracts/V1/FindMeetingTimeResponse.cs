//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The find meeting time response.
    /// </summary>
    [DataContract]
    public class FindMeetingTimeResponse
    {
        /// <summary>
        /// Gets or sets the meeting time suggestions.
        /// </summary>
        [DataMember(Name = "meetingTimeSuggestions")]
        public List<MeetingTimeSuggestion> MeetingTimeSuggestions { get; set; }

        /// <summary>
        /// Gets or sets the reason the meeting time slot suggestions are empty.
        /// </summary>
        [DataMember(Name = "emptySuggestionsReason")]
        public string EmptySuggestionsReason { get; set; }
    }
}
