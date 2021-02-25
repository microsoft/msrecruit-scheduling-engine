//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface CandidacyDetails {
    CandidacyId?: string;
    RequisitionId?: string;
    JobTitle?: string;
    Candidate?: CandidateInformation;
    HiringManager?: string;
    Recruiter?: string;
    CandidacyStage?: CandidacyStage;
    CandidacyStatus?: string;
    CandidateScore?: string;
}
