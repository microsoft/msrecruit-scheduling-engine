﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

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