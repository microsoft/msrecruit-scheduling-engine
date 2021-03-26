//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface DashboardActivity {
    startTime?: Date;
    endTime?: Date;
    jobApplicationActivityType?: JobApplicationActivityType;
    activity?: StageActivity;
    participants?: Person[];
    stageOrder?: number;
    applicant?: Applicant;
    job?: Job;
    jobApplicationId?: string;
    jobApplicationStage?: ApplicationStage;
    attendees?: ScheduleAttendee[];
    isDelegated?: boolean;
}
