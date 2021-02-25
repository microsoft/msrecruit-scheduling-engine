//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicationSchedule {
    id?: string;
    stage?: JobStage;
    scheduleEventId?: string;
    scheduleState?: JobApplicationScheduleState;
    application?: Application;
    stageOrder?: number;
    activityOrdinal?: number;
    activitySubOrdinal?: number;
    scheduleAvailabilities?: ScheduleAvailability[];
}
