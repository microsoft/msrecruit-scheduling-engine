//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Feedback {
    stage?: JobStage;
    stageOrder?: number;
    strengthComment?: string;
    weaknessComment?: string;
    overallComment?: string;
    Status?: JobApplicationAssessmentStatus;
    statusReason?: JobApplicationAssessmentStatusReason;
    isRecommendedToContinue: boolean;
    feedbackProvider?: Delegate;
    jobApplicationID: string;
}
