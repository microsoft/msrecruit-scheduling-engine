//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleAvailability {
    id: string;
    startDate?: Date;
    endDate?: Date;
    isHiringTeamAvailable?: boolean;
    isCandidateAvailable?: boolean;
    timeZone?: string;
    userAction?: UserAction;
}
