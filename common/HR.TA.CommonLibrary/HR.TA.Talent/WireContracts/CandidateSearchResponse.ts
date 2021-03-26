//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface CandidateSearchResponse {
    candidates?: Applicant[];
    filters?: FacetResponse[];
    total?: number;
}
