//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface FeedbackSummary {
    Status?: JobApplicationAssessmentStatus;
    IsRecommendedToContinue?: boolean;
    SubmittedDateTime?: Date;
    InterviewerName?: string;
    OID?: string;
    RemindApplicable?: boolean;
    IsScheduleAvailable?: boolean;
    OverallComment?: string;
    FileAttachments?: FileAttachmentInfo[];
    SubmittedByOID?: string;
    SubmittedByName?: string;
    Notes?: string;
    NotesSubmittedByOID?: string;
    NotesSubmittedByName?: string;
    NotesSubmittedDateTime?: Date;
}
