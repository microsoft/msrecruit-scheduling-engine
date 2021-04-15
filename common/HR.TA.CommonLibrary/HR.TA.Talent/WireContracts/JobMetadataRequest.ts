//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobMetadataRequest {
    jobStatuses?: JobOpeningStatus[];
    skip?: number;
    take?: number;
    searchText?: string;
    continuationToken?: string;
}
