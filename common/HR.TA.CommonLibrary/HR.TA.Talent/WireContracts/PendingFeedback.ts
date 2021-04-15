//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface PendingFeedback {
    StartDateTime?: Date;
    ModeOfInterview?: InterviewMode;
    PositionTitle?: string;
    InterviewerName?: string;
    CandidateName?: string;
    InterviewerOID?: string;
    jobApplicationID?: string;
    Roles?: JobParticipantRole[];
}
