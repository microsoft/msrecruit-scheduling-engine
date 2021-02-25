//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface UsersWithRolesSearchRequest {
    roles?: TalentApplicationRole[];
    skip?: number;
    take?: number;
    searchText?: string;
}
