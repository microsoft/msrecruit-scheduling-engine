//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Sample {
    deleteJobs?: boolean;
    job: Job;
    teams?: TeamMember[];
    candidates?: Applicant[];
    advanceStages?: { [key: string]: JobStage[]; };
    advanceStageOrder?: { [key: string]: number[]; };
    assessments?: { [key: string]: Feedback[]; };
    rejectedCandidates?: { [key: string]: JobApplicationStatusReasonPayload; };
}
