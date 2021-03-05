//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
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
