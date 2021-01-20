﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface DelegationRequest extends TalentBaseContract {
    DelegationId?: string;
    DelegationStatus?: DelegationStatus;
    DelegationStatusReason?: string;
    RequestStatus?: RequestStatus;
    RequestStatusReason?: string;
    FromDate: Date;
    ToDate: Date;
    UTCOffsetInMinutes: string;
    From: Person;
    To: Person;
    RequestedBy: Person;
    Notes?: string;
}
