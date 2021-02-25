//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface SearchMetadataRequest {
    skip?: number;
    take?: number;
    searchText?: string;
    searchFields?: string[];
    continuationToken?: string;
}
