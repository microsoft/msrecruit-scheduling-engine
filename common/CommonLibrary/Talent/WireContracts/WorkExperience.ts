//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface WorkExperience extends TalentBaseContract {
    WorkExperienceId?: string;
    Title?: string;
    Organization?: string;
    Location?: string;
    Description?: string;
    IsCurrentPosition?: boolean;
    FromMonthYear?: Date;
    ToMonthYear?: Date;
}
