//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface StageScheduleEvent {
    JobApplicationId: string;
    TeamMembers: TeamMember[];
    Stage: JobStage;
    StageOrder?: number;
    Location?: string;
    Comment?: string;
    ScheduleEventId: string;
    ScheduleState: JobApplicationScheduleState;
    ScheduleType?: number;
    ScheduleAvailabilities?: ScheduleAvailability[];
    ScheduleDates?: string[];
    TimezoneName?: string;
}
