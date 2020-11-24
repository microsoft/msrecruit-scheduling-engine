﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface JobTemplate extends TalentBaseContract {
    id?: string;
    name?: string;
    displayName?: string;
    validFrom?: Date;
    validTo?: Date;
    isActive?: boolean;
    isDefault?: boolean;
    templateReference?: string;
    stageTemplates?: JobStageTemplate[];
}