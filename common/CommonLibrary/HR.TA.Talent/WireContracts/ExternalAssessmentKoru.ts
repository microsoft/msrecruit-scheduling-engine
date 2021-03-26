//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ExternalAssessmentKoru {
    assessmentId?: string;
    assessmentType?: string;
    name?: string;
    previewUrl?: string;
    numberOfQuestions?: number;
}

export interface KoruAssessments {
    assessments?: ExternalAssessmentKoru[];
}
