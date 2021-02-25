//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobOpeningPositionRequest {
    skip?: number;
    take?: number;
    searchText?: string;
    continuationToken?: string;
}
