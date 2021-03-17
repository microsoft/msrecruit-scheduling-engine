//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantsMetadataRequest {
    Skip?: number;
    Take?: number;
    SearchText?: string;
    Stage?: JobApplicationActivityType;
}
