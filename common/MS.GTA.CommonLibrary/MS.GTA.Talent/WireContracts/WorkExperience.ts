﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface WorkExperience extends TalentBaseContract {
    WorkExperienceId?: string;
    Title?: string;
    Organization?: string;
    Location?: string;
    Description?: string;
    IsCurrentPosition?: boolean;
    FromMonthYear?: Date;
    ToMonthYear?: Date;
}
