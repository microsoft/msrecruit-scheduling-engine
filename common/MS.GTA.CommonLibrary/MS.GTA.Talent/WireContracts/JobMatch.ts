﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface JobMatch {
    jobOpeningId: string;
    externalJobOpeningId: string;
    description?: string;
    jobLocation?: Address;
    jobTitle?: string;
    jobOpeningProperties?: JobOpeningProperties;
    computeResult?: JobSkill[];
    score: number;
}
