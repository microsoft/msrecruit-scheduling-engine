//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface StageActivity extends TalentBaseContract {
    id?: string;
    name?: string;
    displayName?: string;
    description?: string;
    ordinal?: number;
    subOrdinal?: number;
    audience?: ActivityAudience;
    isEnableForCandidate?: boolean;
    activityType?: JobApplicationActivityType;
    configuration?: string;
    scheduleEventId?: string;
    comment?: string;
    activityStatus?: JobApplicationActivityStatus;
    participants?: HiringTeamMember[];
    dueDateTime?: Date;
    plannedStartDateTime?: Date;
    required?: Required;
}
