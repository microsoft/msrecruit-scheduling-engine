//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface SuggestMeetingsRequest {
    interviewStartDateSuggestion: Date;
    interviewEndDateSuggestion: Date;
    timezone?: Timezone;
    panelInterview?: boolean;
    privateMeeting?: boolean;
    teamsMeeting?: boolean;
    meetingDurationInMinutes?: string;
    interviewersList?: UserGroup[];
    candidate?: GraphPerson;
}
