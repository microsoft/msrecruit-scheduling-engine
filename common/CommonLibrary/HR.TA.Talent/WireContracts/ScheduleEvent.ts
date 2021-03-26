//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleEvent {
    id: string;
    candidate: GraphPerson;
    dates: string[];
    tenantId?: string;
    jobTitle?: string;
    userPermissions?: UserPermission[];
    deepLinkUrl?: string;
    timezone?: Timezone;
}
