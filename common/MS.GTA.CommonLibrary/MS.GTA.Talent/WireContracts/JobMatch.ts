//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobMatch {
    jobOpeningId: string;
    externalJobOpeningId: string;
    description?: string;
    jobLocation?: Address;
    jobTitle?: string;
    jobOpeningProperties?: JobOpeningProperties;
    computeResult?: JobSkill[];
    score: number;
}
