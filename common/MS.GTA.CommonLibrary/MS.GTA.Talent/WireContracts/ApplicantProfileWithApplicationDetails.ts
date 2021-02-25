//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantProfileWithApplicationDetails {
    identityProvider: string;
    identityProviderUsername: string;
    isTermsAcceptedByCandidate: boolean;
    applicantProfile: ApplicantProfile;
}
