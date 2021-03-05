//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobOpeningTemplateParticipant extends AADUser {
    userObjectId?: string;
    groupObjectId?: string;
    tenantObjectId?: string;
    isDefault?: boolean;
    canEdit?: boolean;
}
