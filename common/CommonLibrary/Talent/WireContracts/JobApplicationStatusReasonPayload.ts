//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobApplicationStatusReasonPayload {
    StatusReason?: JobApplicationStatusReason;
    RejectionReason?: OptionSetValue;
    Comment?: string;
}
