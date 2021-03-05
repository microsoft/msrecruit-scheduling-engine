//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface DashboardActivitiesRequest {
    startTime?: Date;
    endTime?: Date;
    skip?: number;
    take?: number;
    continuationToken?: string;
}
