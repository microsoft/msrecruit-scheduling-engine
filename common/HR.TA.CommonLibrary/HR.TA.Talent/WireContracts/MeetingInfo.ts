//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface MeetingInfo {
    id: string;
    userGroups: UserGroup;
    requester?: GraphPerson;
    scheduleEventId?: string;
    scheduleStatus?: ScheduleStatus;
    meetingDetails: MeetingDetails[];
    tenantId?: string;
    scheduleOrder?: number;
    InterviewerTimeSlotId: string;
}
