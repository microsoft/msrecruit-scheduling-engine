//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface IVApplicant {
    JobApplicationId?: string;
    ExternalJobOpeningId?: string;
    ExternalJobApplicationId?: string;
    JobTitle?: string;
    Candidate?: CandidateInformation;
    HiringManager?: string;
    Recruiter?: string;
    participantRole?: JobParticipantRole;
    CurrentStage?: JobApplicationActivityType;
    JobDescription?: string;
}
