//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface TalentPoolMetadaRequest {
    skip?: number;
    take?: number;
    searchText?: string;
    talentPoolRoles?: TalentPoolParticipantRole[];
}
