//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface CandidateRecommendationFeedback {
    applicantId?: string;
    interested?: boolean;
    experience?: boolean;
    skills?: boolean;
    education?: boolean;
    other?: boolean;
}
