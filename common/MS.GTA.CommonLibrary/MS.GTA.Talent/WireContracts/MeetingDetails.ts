//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface MeetingDetails {
    id: string;
    subject?: string;
    description?: string;
    utcStart?: Date;
    utcEnd?: Date;
    location?: string;
    meetingLocation?: InterviewMeetingLocation;
    calendarEventId?: string;
    skypeOnlineMeetingRequired: boolean;
    onlineMeetingRequired: boolean;
    unknownFreeBusyTime?: boolean;
    status?: FreeBusyScheduleStatus;
    isTentative?: boolean;
    attendees: Attendee[];
    skypeOnlineMeeting?: SkypeSchedulerResponse;
    isDirty?: boolean;
    skypeMeetingType?: SkypeMeetingType;
    schedulerAccountEmail?: string;
    isPrivateMeeting?: boolean;
    isInterviewScheduleShared: boolean;
    isInterviewerNameShared: boolean;
    conference?: ConferenceInfo;
}
