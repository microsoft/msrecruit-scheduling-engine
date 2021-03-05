//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantProfileWithLoginDetails {
    identityProvider: string;
    identityProviderUsername: string;
    applicantProfile: ApplicantProfile;
}
