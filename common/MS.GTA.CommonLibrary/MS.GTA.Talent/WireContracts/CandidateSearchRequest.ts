//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface CandidateSearchRequest {
    skip?: number;
    take?: number;
    facetSearchRequest?: FacetSearchRequest[];
}
