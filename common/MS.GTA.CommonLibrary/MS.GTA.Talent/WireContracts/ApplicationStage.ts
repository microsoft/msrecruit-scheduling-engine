//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
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
