//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.EnumSetModel.SchedulingService;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The <see cref="JobApplicationSchedule"/> class represents the job application schedule entity.
    /// </summary>
    /// <seealso cref="DocDbEntity" />
    [DataContract]
    public class JobApplicationSchedule : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        /// <value>
        /// The schedule identifier.
        /// </value>
        [DataMember(Name = "ScheduleID", EmitDefaultValue = false, IsRequired = false)]
        public string ScheduleID { get; set; }

        /// <summary>
        /// Gets or sets the start date time.
        /// </summary>
        /// <value>
        /// The start date time or <c>null</c>.
        /// </value>
        [DataMember(Name = "StartDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end date time.
        /// </summary>
        /// <value>
        /// The end date time or <c>null</c>.
        /// </value>
        [DataMember(Name = "EndDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone string.
        /// </value>
        [DataMember(Name = "TimeZone", EmitDefaultValue = false, IsRequired = false)]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the schedule status.
        /// </summary>
        /// <value>
        /// The value from <see cref="ScheduleStatus"/> enumeration.
        /// </value>
        [DataMember(Name = "ScheduleStatus", EmitDefaultValue = false, IsRequired = false)]
        public ScheduleStatus ScheduleStatus { get; set; }

        /// <summary>
        /// Gets or sets the job application identifier.
        /// </summary>
        /// <value>
        /// The job application identifier.
        /// </value>
        [DataMember(Name = "JobApplicationID", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the interview schedule participants.
        /// </summary>
        /// <value>
        /// The interview schedule participants.
        /// </value>
        [DataMember(Name = "JobApplicationScheduleParticipants", EmitDefaultValue = false, IsRequired = false)]
        public List<JobApplicationScheduleParticipant> Participants { get; set; }

        /// <summary>
        /// Gets or sets whether the hiring team is available.
        /// </summary>
        /// <value>
        /// The availability of hiring team or <c>null</c>.
        /// </value>
        [DataMember(Name = "IsHiringTeamAvailable", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsHiringTeamAvailable { get; set; }

        /// <summary>
        /// Gets or sets whether the candidate is available.
        /// </summary>
        /// <value>
        /// The candidate availability.
        /// </value>
        [DataMember(Name = "IsCandidateAvailable", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsCandidateAvailable { get; set; }

        /// <summary>
        /// Gets or sets the meeting detail identifier.
        /// </summary>
        /// <value>
        /// The meeting detail identifier.
        /// </value>
        [DataMember(Name = "MeetingDetailId", EmitDefaultValue = false, IsRequired = false)]
        public string MeetingDetailId { get; set; }

        /// <summary>
        /// Gets or sets the meeting information identifier.
        /// </summary>
        /// <value>
        /// The meeting information identifier.
        /// </value>
        [DataMember(Name = "MeetingInfoId", EmitDefaultValue = false, IsRequired = false)]
        public string MeetingInfoId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [DataMember(Name = "Subject", IsRequired = false, EmitDefaultValue = false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember(Name = "Description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the calendar event identifier.
        /// </summary>
        /// <value>
        /// The calendar event identifier.
        /// </value>
        [DataMember(Name = "CalendarEventId", IsRequired = false, EmitDefaultValue = false)]
        public string CalendarEventId { get; set; }

        /// <summary>
        /// Gets or sets the schedule requester.
        /// </summary>
        /// <value>
        /// The schedule requester as instance of <see cref="Worker"/>.
        /// </value>
        [DataMember(Name = "ScheduleRequester", EmitDefaultValue = false, IsRequired = false)]
        public Worker ScheduleRequester { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether skype online meeting is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the skype online meeting is required; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "SkypeOnlineMeetingRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool SkypeOnlineMeetingRequired { get; set; }

        /// <summary>
        /// Gets or sets the service account email.
        /// </summary>
        /// <value>
        /// The service account email.
        /// </value>
        [DataMember(Name = "ServiceAccountEmail", IsRequired = false, EmitDefaultValue = false)]
        public string ServiceAccountEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsDirty", IsRequired = false, EmitDefaultValue = false)]
        public bool IsDirty { get; set; }

        /// <summary>
        /// Gets or sets the skype HTML view href.
        /// </summary>
        /// <value>
        /// The skype HTML view href.
        /// </value>
        [DataMember(Name = "SkypeHtmlViewHref", IsRequired = false, EmitDefaultValue = false)]
        public string SkypeHtmlViewHref { get; set; }

        /// <summary>
        /// Gets or sets the skype join URL.
        /// </summary>
        /// <value>
        /// The skype join URL.
        /// </value>
        [DataMember(Name = "SkypeJoinUrl", IsRequired = false, EmitDefaultValue = false)]
        public string SkypeJoinUrl { get; set; }

        /// <summary>
        /// Gets or sets the skype online meeting identifier.
        /// </summary>
        /// <value>
        /// The skype online meeting identifier.
        /// </value>
        [DataMember(Name = "SkypeOnlineMeetingId", IsRequired = false, EmitDefaultValue = false)]
        public string SkypeOnlineMeetingId { get; set; }

        /// <summary>
        /// Gets or sets the name of the schedule timezone.
        /// </summary>
        /// <value>
        /// The name of the schedule timezone.
        /// </value>
        [DataMember(Name = "ScheduleTimezoneName", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleTimezoneName { get; set; }

        /// <summary>
        /// Gets or sets the schedule UTC offset.
        /// </summary>
        /// <value>
        /// The schedule UTC offset.
        /// </value>
        [DataMember(Name = "ScheduleUTCOffset", IsRequired = false, EmitDefaultValue = false)]
        public int ScheduleUTCOffset { get; set; }

        /// <summary>
        /// Gets or sets the schedule UTC offset in hours.
        /// </summary>
        /// <value>
        /// The schedule UTC offset in hours.
        /// </value>
        [DataMember(Name = "ScheduleUTCOffsetInHours", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleUTCOffsetInHours { get; set; }

        /// <summary>
        /// Gets or sets the schedule timezone abbrevation.
        /// </summary>
        /// <value>
        /// The schedule timezone abbrevation.
        /// </value>
        [DataMember(Name = "ScheduleTimezoneAbbr", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleTimezoneAbbr { get; set; }

        /// <summary>
        /// Gets or sets the name of the building.
        /// </summary>
        /// <value>
        /// The name of the building.
        /// </value>
        [DataMember(Name = "BuildingName", EmitDefaultValue = false, IsRequired = false)]
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building address.
        /// </summary>
        /// <value>
        /// The building address.
        /// </value>
        [DataMember(Name = "BuildingAddress", EmitDefaultValue = false, IsRequired = false)]
        public string BuildingAddress { get; set; }

        /// <summary>
        /// Gets or sets the building response status.
        /// </summary>
        /// <value>
        /// The building response status as value from <see cref="InvitationResponseStatus"/> enumeration.
        /// </value>
        [DataMember(Name = "BuildingResponseStatus", EmitDefaultValue = false, IsRequired = false)]
        public InvitationResponseStatus BuildingResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        [DataMember(Name = "RoomName", EmitDefaultValue = false, IsRequired = false)]
        public string RoomName { get; set; }

        /// <summary>
        /// Gets or sets the room address.
        /// </summary>
        /// <value>
        /// The room address.
        /// </value>
        [DataMember(Name = "RoomAddress", EmitDefaultValue = false, IsRequired = false)]
        public string RoomAddress { get; set; }

        /// <summary>
        /// Gets or sets the room response status.
        /// </summary>
        /// <value>
        /// The room response status as value of <see cref="InvitationResponseStatus"/> enumeration.
        /// </value>
        [DataMember(Name = "RoomResponseStatus", EmitDefaultValue = false, IsRequired = false)]
        public InvitationResponseStatus RoomResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the response date time.
        /// </summary>
        /// <value>
        /// The response date time.
        /// </value>
        [DataMember(Name = "ResponseDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime ResponseDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private meeting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is private meeting; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsPrivateMeeting", EmitDefaultValue = false, IsRequired = false)]
        public bool IsPrivateMeeting { get; set; }

        /// <summary>
        /// Gets or sets whether the response status is invalid.
        /// </summary>
        /// <value>
        /// The response status is invalid or not.
        /// </value>
        [DataMember(Name = "IsResponseStatusInvalid", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsResponseStatusInvalid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the free busy time is unknown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if free busy time is unknown; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "UnknownFreeBusyTime", IsRequired = false, EmitDefaultValue = false)]
        public bool UnknownFreeBusyTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview title shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interview title shared; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsInterviewTitleShared", EmitDefaultValue = false, IsRequired = false)]
        public bool IsInterviewTitleShared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview schedule is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interview schedule is shared; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsInterviewScheduleShared", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsInterviewScheduleShared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interviewer name is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interviewer name is shared; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsInterviewerNameShared", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsInterviewerNameShared { get; set; }

        /// <summary>
        /// Gets or sets the mode of interview.
        /// </summary>
        /// <value>
        /// The mode of interview as value from <see cref="InterviewMode"/> enumeration.
        /// </value>
        [DataMember(Name = "ModeOfInterview", EmitDefaultValue = false, IsRequired = false)]
        public InterviewMode ModeOfInterview { get; set; }

        /// <summary>
        /// Gets or sets the primary email recipients.
        /// </summary>
        /// <value>
        /// The primary email recipients.
        /// </value>
        [DataMember(Name = "PrimaryEmailRecipients", IsRequired = false, EmitDefaultValue = false)]
        public List<string> PrimaryEmailRecipients { get; set; }

        /// <summary>
        /// Gets or sets the BCC email address list.
        /// </summary>
        /// <value>
        /// The BCC email address list.
        /// </value>
        [DataMember(Name = "BccEmailAddressList", IsRequired = false, EmitDefaultValue = false)]
        public List<string> BccEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets the CC email address list.
        /// </summary>
        /// <value>
        /// The CC email address list.
        /// </value>
        [DataMember(Name = "CcEmailAddressList", IsRequired = false, EmitDefaultValue = false)]
        public List<string> CcEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember(Name = "Location", IsRequired = false, EmitDefaultValue = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the interviewer time slot identifier.
        /// </summary>
        /// <value>
        /// The interviewer time slot identifier.
        /// </value>
        [DataMember(Name = "InterviewerTimeSlotId", IsRequired = false, EmitDefaultValue = false)]
        public string InterviewerTimeSlotID { get; set; }

        /// <summary>
        /// Gets or sets the onlinemeeting required or not
        /// </summary>
        [DataMember(Name = "OnlineMeetingRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool OnlineMeetingRequired { get; set; }

        /// <summary>
        /// Gets or sets the online meeting
        /// </summary>
        [DataMember(Name = "OnlineTeamMeeting", IsRequired = false, EmitDefaultValue = false)]
        public OnlineMeeting OnlineTeamMeeting { get; set; }

        /// <summary>
        /// Gets or sets the online meeting content
        /// </summary>
        [DataMember(Name = "OnlineMeetingContent", IsRequired = false, EmitDefaultValue = false)]
        public string OnlineMeetingContent { get; set; }

        /// <summary>
        /// Gets or sets the online conference meeting identifier. Refers the <see cref="ConferenceMeeting"/> type of entity.
        /// </summary>
        /// <value>
        /// The online conference meeting identifier.
        /// </value>
        [DataMember(Name = "conferenceMeetingId", IsRequired = false, EmitDefaultValue = false)]
        public string ConferenceMeetingId { get; set; }

        /// <summary>
        /// Gets or sets the type of the conference provider.
        /// </summary>
        /// <value>
        /// The type of the conference provider.
        /// </value>
        [DataMember(Name = "conferenceProviderType", IsRequired = false, EmitDefaultValue = false)]
        public ConferenceProvider? ConferenceProviderType { get; set; }
    }
}
