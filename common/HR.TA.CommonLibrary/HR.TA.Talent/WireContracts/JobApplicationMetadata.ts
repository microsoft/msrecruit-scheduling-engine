//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobApplicationMetadata {
    jobApplicationId?: string;
    externalJobOpeningId?: string;
    jobTitle?: string;
    jobDescription?: string;
    currentJobApplicationStageStatus?: JobApplicationStageStatus;
    currentJobOpeningStage?: JobStage;
    jobApplicationStatus?: JobApplicationStatus;
    candidate?: CandidateInformation;
    jobApplicationParticipants?: JobApplicationParticipant[];
    jobApplicationParticipantDetails?: IVWorker[];
    IsScheduleSentToCandidate?: boolean;
    HireType?: string;
    JobApplicationStatusReason?: JobApplicationStatusReason;
    isWobAuthenticated?: boolean;
}
