//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface GraphPerson {
    name: string;
    id?: string;
    title?: string;
    email: string;
    givenName?: string;
    mobilePhone?: string;
    officeLocation?: string;
    preferredLanguage?: string;
    surname?: string;
    userPrincipalName?: string;
    InvitationResponseStatus?: InvitationResponseStatus;
    Comments?: string;
}
