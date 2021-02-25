//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface AppRoleUpsertRequest {
    userObjectId?: string;
    user?: Person;
    userRoles: TalentApplicationRole[];
}
