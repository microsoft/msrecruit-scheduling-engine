//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface SkypeSchedulerResponse {
    joinUrl: string;
    onlineMeetingId: string;
    expirationTime: Date;
    subject: string;
    leaders: string[];
    attendees: string[];
    accessLevel: string;
    entryExitAnnouncement: string;
    automaticLeaderAssignment: string;
    onlineMeetingUri: string;
    organizerUri: string;
    phoneUserAdmission: string;
    lobbyBypassForPhoneUsers: string;
    etag: string;
    _links: SkypeLinks;
    _embedded: SkypeEmbedded;
}
