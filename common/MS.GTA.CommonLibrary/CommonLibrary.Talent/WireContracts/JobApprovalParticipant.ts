//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobApprovalParticipant extends Person {
    comment?: string;
    jobApprovalStatus?: JobApprovalStatus;
    userAction?: UserAction;
}
