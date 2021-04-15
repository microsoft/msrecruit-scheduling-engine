//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface InterviewDetails {
    JobApplicationID: string;
    CandidateName?: string;
    PositionTitle?: string;
    SchedulesForDay?: InterviewSchedule[];
}
