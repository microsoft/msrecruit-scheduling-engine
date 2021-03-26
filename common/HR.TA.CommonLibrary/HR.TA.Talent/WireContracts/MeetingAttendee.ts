//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface MeetingAttendee {
    type: string;
    emailAddress: MeetingAttendeeEmailAddress;
    status: MeetingAttendeeStatus;
    proposedNewTime?: MeetingTimeSpan;
}
