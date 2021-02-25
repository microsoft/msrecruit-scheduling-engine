//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobAssessment {
    jobAssessmentID?: string;
    packageID?: string;
    jobOpeningID?: string;
    provider?: AssessmentProvider;
    providerKey?: string;
    title?: string;
    numberOfQuestions?: number;
    previewURL?: string;
    assessment?: ExternalAssessment;
    isRequired?: JobOpeningAssessmentRequirementStatus;
    stage?: JobStage;
}
