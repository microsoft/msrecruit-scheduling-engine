//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface DelegationRequestPagedResponse {
    items?: DelegationRequest[];
    nextPage?: string;
    previousPage?: string;
    pageNumber?: number;
    pageSize?: number;
    totalCount?: number;
}
