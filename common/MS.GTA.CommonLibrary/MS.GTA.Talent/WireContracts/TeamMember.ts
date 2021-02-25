//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface TeamMember extends AADUser {
    Role: JobParticipantRole;
    UserAction: UserAction;
    IsMemberOfActivity: boolean;
}

export interface AADUser extends TalentBaseContract {
    name: string;
    id: string;
    title: string;
    email: string;
    given: string;
    mobilePhone: string;
    officeLocation: string;
    userPrincipalName: string;
    mailNickname?: string;
    externalWorkerId?: string;
    externalWorkerSource?: Source;
}
