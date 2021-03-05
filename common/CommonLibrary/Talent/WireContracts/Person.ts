//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Person extends TalentBaseContract {
    objectId?: string;
    givenName?: string;
    middleName?: string;
    surname?: string;
    fullName?: string;
    email?: string;
    alternateEmail?: string;
    linkedIn?: string;
    linkedInAPI?: string;
    facebook?: string;
    twitter?: string;
    homePhone?: string;
    workPhone?: string;
    mobilePhone?: string;
    profession?: string;
    mailNickname?: string;
    externalWorkerId?: string;
    externalWorkerSource?: Source;
    mailingPostalAddress?: Address;
    otherPostalAddress?: Address;
}
