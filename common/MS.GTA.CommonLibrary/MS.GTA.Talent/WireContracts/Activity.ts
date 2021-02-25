//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Activity {
    stage?: JobStage;
    activityType?: JobApplicationActivityType;
    location?: string;
    description?: string;
    plannedStartTime?: Date;
    plannedEndTime?: Date;
    status?: JobApplicationActivityStatus;
    statusReason?: JobApplicationActivityStatusReason;
}
