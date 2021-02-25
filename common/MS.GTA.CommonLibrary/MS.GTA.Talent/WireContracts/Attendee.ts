//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Attendee {
    user: GraphPerson;
    responseStatus?: InvitationResponseStatus;
    isResponseStatusInvalid?: boolean;
    responseComment?: string;
    responseDateTime?: Date;
    freeBusyStatus?: FreeBusyScheduleStatus;
}
