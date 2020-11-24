﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface ApplicationStage extends TalentBaseContract {
    stage: JobStage;
    order: number;
    displayName?: string;
    description?: string;
    stageActivities?: StageActivity[];
    isActiveStage?: boolean;
    totalActivities?: number;
    completedActivities?: number;
    lastCompletedActivityDateTime?: Date;
}