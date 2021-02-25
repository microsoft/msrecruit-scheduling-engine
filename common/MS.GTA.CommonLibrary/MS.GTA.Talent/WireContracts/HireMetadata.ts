//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface HireMetadata {
    candidates?: Applicant[];
    jobs?: Job[];
    candidatesAppliedCount: number;
    candidatesScreenCount: number;
    candidatesInterviewCount: number;
    candidatesOfferCount: number;
    candidatesAssessmentCount: number;
}
