//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface SocialIdentity {
    applicant?: Applicant;
    provider?: SocialNetworkProvider;
    providerMemberId?: string;
}
