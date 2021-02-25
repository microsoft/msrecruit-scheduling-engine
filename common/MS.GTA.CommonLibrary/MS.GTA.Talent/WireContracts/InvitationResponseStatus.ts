//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export enum InvitationResponseStatus {
    None = 0,
    Accepted = 1,
    TentativelyAccepted = 2,
    Declined = 3,
    Pending = 4,
    Sending = 5,
    ResendRequired = 6,
}
