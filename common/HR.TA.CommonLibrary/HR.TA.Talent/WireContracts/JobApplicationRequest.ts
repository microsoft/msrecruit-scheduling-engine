//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobApplicationRequest {
    applicationStatuses?: JobApplicationStatus[];
    skip?: number;
    take?: number;
    searchText?: string;
    stageOrder?: number;
    prospectOnly?: boolean;
}
