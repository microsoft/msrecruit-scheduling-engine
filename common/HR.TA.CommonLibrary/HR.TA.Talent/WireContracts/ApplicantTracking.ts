//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantTagTracking {
    id?: string;
    owner?: Person;
    category?: CandidateTrackingCategory;
    tags?: ApplicantTag[];
}
