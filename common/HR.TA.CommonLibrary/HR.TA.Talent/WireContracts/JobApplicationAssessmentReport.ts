//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobApplicationAssessmentReport {
    jobApplicationId: string;
    candidateGivenName: string;
    candidateSurname: string;
    externalAssessmentReportID: string;
    assessmentStatus: AssessmentStatus;
    assessmentURL: string;
    title: string;
    reportURL: string;
    results: JobApplicationAssessmentReportResult[];
    additionalInformation: string;
}
