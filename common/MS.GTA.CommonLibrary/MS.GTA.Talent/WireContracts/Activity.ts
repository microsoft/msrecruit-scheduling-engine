﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

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