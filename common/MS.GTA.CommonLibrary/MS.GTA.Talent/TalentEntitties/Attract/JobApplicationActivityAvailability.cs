//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobapplicationactivityavailabilities", SingularName = "msdyn_jobapplicationactivityavailability")]
    public class JobApplicationActivityAvailability : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobapplicationactivityavailabilityid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationactivityid_value")]
        public Guid? JobApplicationActivityId { get; set; }

        [DataMember(Name = "msdyn_JobapplicationactivityId")]
        public JobApplicationActivity JobApplicationActivity { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_start")]
        public DateTime? Start { get; set; }

        [DataMember(Name = "msdyn_end")]
        public DateTime? End { get; set; }

        [DataMember(Name = "msdyn_offset")]
        public string Offset { get; set; }

        [DataMember(Name = "msdyn_timezone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "msdyn_location")]
        public string Location { get; set; }

        [DataMember(Name = "msdyn_iscandidateavailable")]
        public bool? IsCandidateAvailable { get; set; }

        [DataMember(Name = "msdyn_ishiringteamavailable")]
        public bool? IsHiringTeamAvailable { get; set; }

        [DataMember(Name = "msdyn_meetingdetailid")]
        public string MeetingDetailId { get; set; }

        [DataMember(Name = "msdyn_meetinginfoid")]
        public string MeetingInfoId { get; set; }

        [DataMember(Name = "msdyn_scheduleorder")]
        public int ScheduleOrder { get; set; }

        [DataMember(Name = "msdyn_subject")]
        public string Subject { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_calendareventid")]
        public string CalendarEventId { get; set; }

        [DataMember(Name = "msdyn_skypeonlinemeetingrequired")]
        public bool? SkypeOnlineMeetingRequired { get; set; }

        [DataMember(Name = "msdyn_isdirty")]
        public bool? IsDirty { get; set; }

        [DataMember(Name = "msdyn_skypemeetingtype")]
        public SkypeMeetingType? SkypeMeetingType { get; set; }

        [DataMember(Name = "msdyn_scheduleraccountemail")]
        public string SchedulerAccountEmail { get; set; }

        [DataMember(Name = "msdyn_skypehtmlviewhref")]
        public string SkypeHtmlViewHref { get; set; }

        [DataMember(Name = "msdyn_skypejoinurl")]
        public string SkypeJoinUrl { get; set; }

        [DataMember(Name = "msdyn_skypeonlinemeetingid")]
        public string SkypeOnlineMeetingId { get; set; }

        [DataMember(Name = "msdyn_schedulerequester")]
        public Worker ScheduleRequester { get; set; }

        [DataMember(Name = "_msdyn_schedulerequester_value")]
        public Guid? ScheduleRequesterId { get; set; }

        [DataMember(Name = "msdyn_scheduletimezonename")]
        public string ScheduleTimezoneName { get; set; }

        [DataMember(Name = "msdyn_scheduleutcoffset")]
        public int ScheduleUTCOffset { get; set; }

        [DataMember(Name = "msdyn_scheduleutcoffsetinhours")]
        public string ScheduleUTCOffsetInHours { get; set; }

        [DataMember(Name = "msdyn_scheduletimezoneabbreviation")]
        public string ScheduleTimezoneAbbreviation { get; set; }

        [DataMember(Name = "msdyn_buildingname")]
        public string BuildingName { get; set; }

        [DataMember(Name = "msdyn_buildingaddress")]
        public string BuildingAddress { get; set; }

        [DataMember(Name = "msdyn_buildingresponsestatus")]
        public InvitationResponseStatus? BuildingResponseStatus { get; set; }

        [DataMember(Name = "msdyn_roomname")]
        public string RoomName { get; set; }

        [DataMember(Name = "msdyn_roomaddress")]
        public string RoomAddress { get; set; }

        [DataMember(Name = "msdyn_roomresponsestatus")]
        public InvitationResponseStatus? RoomResponseStatus { get; set; }

        [DataMember(Name = "msdyn_interviewerresponsestatus")]
        public InvitationResponseStatus? InterviewerResponseStatus { get; set; }

        [DataMember(Name = "msdyn_responsecomment")]
        public string ResponseComment { get; set; }

        [DataMember(Name = "msdyn_responsedatetime")]
        public DateTime? ResponseDateTime { get; set; }

        [DataMember(Name = "msdyn_isprivatemeeting")]
        public bool? IsPrivateMeeting { get; set; }

        [DataMember(Name = "msdyn_finalschedulestatus")]
        public FinalScheduleStatus? FinalScheduleStatus { get; set; }

        [DataMember(Name = "msdyn_unknownfreebusytime")]
        public bool? UnknownFreeBusyTime { get; set; }

        [DataMember(Name = "msdyn_istentative")]
        public bool? IsTentative { get; set; }

        [DataMember(Name = "msdyn_isinterviewtitleshared")]
        public bool? IsInterviewTitleShared { get; set; }

        [DataMember(Name = "msdyn_isinterviewscheduleshared")]
        public bool? IsInterviewScheduleShared { get; set; }

        [DataMember(Name = "msdyn_isinterviewernameshared")]
        public bool? IsInterviewerNameShared { get; set; }
    }
}
