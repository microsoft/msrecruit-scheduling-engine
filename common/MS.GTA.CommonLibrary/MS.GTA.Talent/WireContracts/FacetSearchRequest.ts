//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface FacetSearchRequest {
    facet?: FacetType;
    searchText?: string;
    isFilter?: boolean;
}
