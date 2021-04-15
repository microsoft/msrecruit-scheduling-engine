//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface UserWithRoles extends AADUser {
    roles?: TalentApplicationRole[];
}
