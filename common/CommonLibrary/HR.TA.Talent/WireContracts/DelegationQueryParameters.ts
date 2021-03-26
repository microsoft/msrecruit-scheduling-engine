//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface DelegationQueryParameters extends PaginationAndSortQueryParameters {
    includeWorker?: boolean;
    searchToEmail?: string;
    searchFromEmail?: string;
}
