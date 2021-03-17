//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface AssessmentProject {
    id?: string;
    created?: string;
    title?: string;
    previewURL?: string;
    projectStatus?: string;
    instructions?: AssessmentInstructions;
    templates?: ExternalAssessment[];
    users?: AssessmentUser[];
    companyName?: string;
    jobTitle?: string;
    jobDescription?: string;
}
