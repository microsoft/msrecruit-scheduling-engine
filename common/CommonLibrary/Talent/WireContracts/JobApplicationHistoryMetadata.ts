//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobApplicationHistoryMetadata {
    jobApplicationStatus?: JobApplicationStatus;
    jobApplicationDate?: Date;
    hiringTeamMember?: HiringTeamMember;
    jobTitle?: string;
    jobOpeningId?: string;
    jobApplicationId?: string;
    rank?: Rank;
    talentSource?: TalentSource;
}
