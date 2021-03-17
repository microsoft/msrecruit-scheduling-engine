//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface CandidacySearchRequest {
    RequisitionFilter?: string[];
    StageFilter?: CandidacyStage;
    SearchText?: string;
    Skip?: number;
    Take?: number;
}
