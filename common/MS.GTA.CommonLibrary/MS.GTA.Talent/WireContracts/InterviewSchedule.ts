//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface InterviewSchedule {
    InterviewStartDateTime?: Date;
    InterviewEndDateTime?: Date;
    Interviewers?: Interviewer[];
    InterviewMode?: InterviewMode;
    InterviewScheduleStatus?: ScheduleStatus;
}
