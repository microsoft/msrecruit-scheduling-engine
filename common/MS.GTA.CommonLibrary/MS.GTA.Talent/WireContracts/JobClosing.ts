//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobClosing {
    jobOpeningStatus: JobOpeningStatus;
    jobOpeningStatusReason: JobOpeningStatusReason;
    jobOpeningExternalStatus?: string;
    comment?: string;
    offerAcceptedJobApplicationIds?: string[];
}
