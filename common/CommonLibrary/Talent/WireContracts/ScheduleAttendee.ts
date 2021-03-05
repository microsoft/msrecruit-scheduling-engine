//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleAttendee {
    userId?: string;
    userName?: string;
    scheduleEventId?: string;
    responseStatus?: InvitationResponseStatus;
    startTime?: Date;
}
