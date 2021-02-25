//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ExternalAssessment {
    id?: string;
    created?: string;
    title?: string;
    previewUrl?: string;
    numberOfQuestions?: number;
    instructions?: AssessmentInstructions;
    questions?: AssessmentQuestion[];
}
