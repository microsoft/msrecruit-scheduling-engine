//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface AssessmentProjectContributors {
    projectId?: string;
    addContributors?: AssessmentUser[];
    removeContributors?: AssessmentUser[];
}
