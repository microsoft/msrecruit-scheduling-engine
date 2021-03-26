//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.ScheduleService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// Suggest meeting request
    /// </summary>
    [DataContract]
    public class SuggestMeetingsRequest
    {
        /// <summary>
        /// Gets or sets interview start date suggestion
        /// </summary>
        [DataMember(Name = "interviewStartDateSuggestion")]
        public DateTime InterviewStartDateSuggestion { get; set; }

        /// <summary>
        /// Gets or sets interview end date suggestion
        /// </summary>
        [DataMember(Name = "interviewEndDateSuggestion")]
        public DateTime InterviewEndDateSuggestion { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        [DataMember(Name = "timezone", EmitDefaultValue = false, IsRequired = false)]
        public Timezone Timezone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview is panel or not
        /// </summary>
        [DataMember(Name = "panelInterview", IsRequired = false, EmitDefaultValue = false)]
        public bool PanelInterview { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the meeting's are private or not
        /// </summary>
        [DataMember(Name = "privateMeeting", IsRequired = false, EmitDefaultValue = false)]
        public bool PrivateMeeting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to add teams meeting link or not
        /// </summary>
        [DataMember(Name = "teamsMeeting", IsRequired = false, EmitDefaultValue = false)]
        public bool TeamsMeeting { get; set; }

        /// <summary>
        /// Gets or sets a meeting duration value in minutes
        /// </summary>
        [DataMember(Name = "meetingDurationInMinutes", IsRequired = false, EmitDefaultValue = false)]
        public string MeetingDurationInMinutes { get; set; }

        /// <summary>
        /// Gets or sets interviewers list
        /// </summary>
        [DataMember(Name = "interviewersList", IsRequired = false, EmitDefaultValue = false)]
        public IList<UserGroup> InterviewersList { get; set; }

        /// <summary>
        /// Gets or sets candidate
        /// </summary>
        [DataMember(Name = "candidate", IsRequired = false, EmitDefaultValue = false)]
        public GraphPerson Candidate { get; set; }
    }
}
