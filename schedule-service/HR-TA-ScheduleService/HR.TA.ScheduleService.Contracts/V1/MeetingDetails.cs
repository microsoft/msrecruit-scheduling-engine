//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using HR.TA.Talent.EnumSetModel.SchedulingService;
    using HR.TA.Talent.TalentContracts.ScheduleService.Conferencing;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Meeting details
    /// </summary>
    [DataContract]
    public class MeetingDetails
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        [DataMember(Name = "subject", IsRequired = false, EmitDefaultValue = false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start
        /// </summary>
        [DataMember(Name = "utcStart", IsRequired = false, EmitDefaultValue = false)]
        public DateTime UtcStart { get; set; }

        /// <summary>
        /// Gets or sets the end
        /// </summary>
        [DataMember(Name = "utcEnd", IsRequired = false, EmitDefaultValue = false)]
        public DateTime UtcEnd { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        [DataMember(Name = "location", IsRequired = false, EmitDefaultValue = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the meeting location - this should be used instead of location
        /// </summary>
        [DataMember(Name = "meetingLocation", IsRequired = false, EmitDefaultValue = false)]
        public InterviewMeetingLocation MeetingLocation { get; set; }

        /// <summary>
        /// Gets or sets the calendar event id
        /// </summary>
        [DataMember(Name = "calendarEventId", IsRequired = false, EmitDefaultValue = false)]
        public string CalendarEventId { get; set; }

        // TODO : To be removed after UI fixes.
        /// <summary>
        /// Gets or sets a value indicating whether a skype for business online meeting is requested
        /// </summary>
        [DataMember(Name = "skypeOnlineMeetingRequired")]
        public bool SkypeOnlineMeetingRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a online meeting is requested
        /// </summary>
        [DataMember(Name = "onlineMeetingRequired")]
        public bool OnlineMeetingRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the free busy time is unknown
        /// </summary>
        [DataMember(Name = "unknownFreeBusyTime", IsRequired = false, EmitDefaultValue = false)]
        public bool UnknownFreeBusyTime { get; set; }

        /// <summary>
        /// Gets or sets a uber level meeting detail status
        /// </summary>
        [DataMember(Name = "status", IsRequired = false, EmitDefaultValue = true)]
        public FreeBusyScheduleStatus Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the time slot is tentative
        /// </summary>
        [DataMember(Name = "isTentative", IsRequired = false, EmitDefaultValue = false)]
        public bool IsTentative { get; set; }

        /// <summary>
        /// Gets or sets the list of attendees
        /// </summary>
        [DataMember(Name = "attendees")]
        public List<Attendee> Attendees { get; set; }

        // TODO: Remove after teams integration
        /// <summary>
        /// Gets or sets the skype for business online meeting information.
        /// </summary>
        [DataMember(Name = "skypeOnlineMeeting", IsRequired = false, EmitDefaultValue = false)]
        public SkypeSchedulerResponse SkypeOnlineMeeting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the meeting details contains update that hasn't been sent
        /// </summary>
        [DataMember(Name = "isDirty", IsRequired = false, EmitDefaultValue = false)]
        public bool IsDirty { get; set; }

        /// <summary>
        /// Gets or sets Schedule creation account email
        /// </summary>
        [DataMember(Name = "schedulerAccountEmail", IsRequired = false, EmitDefaultValue = false)]
        public string SchedulerAccountEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the meeting private
        /// </summary>
        [DataMember(Name = "isPrivateMeeting", IsRequired = false, EmitDefaultValue = false)]
        public bool IsPrivateMeeting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview schedule is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interview schedule is shared.
        /// </value>
        [DataMember(Name = "isInterviewScheduleShared", IsRequired = true)]
        public bool? IsInterviewScheduleShared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interviewer name is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interviewer name is shared.
        /// </value>
        [DataMember(Name = "isInterviewerNameShared", IsRequired = true)]
        public bool? IsInterviewerNameShared { get; set; }

        /// <summary>
        /// Gets or sets the conference information.
        /// </summary>
        /// <value>
        /// The instance of <see cref="ConferenceInfo"/>.
        /// </value>
        [DataMember(Name = "conference", IsRequired = false)]
        public ConferenceInfo Conference { get; set; }
    }
}