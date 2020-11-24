﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface TalentPool {
    poolId?: string;
    poolName?: string;
    description?: string;
    candidates?: Applicant[];
    candidateCount?: number;
    contributors?: TalentPoolParticipant[];
    lastModified?: Date;
    source?: TalentPoolSource;
    externalId?: string;
    userPermissions?: TalentPoolPermission[];
}
