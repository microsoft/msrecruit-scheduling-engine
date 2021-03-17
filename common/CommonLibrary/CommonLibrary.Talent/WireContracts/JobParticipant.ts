//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobParticipant {
    FullName?: string;
    Alias?: string;
    EmailPrimary?: string;
    OfficeGraphIdentifier?: string;
    TeamsIdentifier?: string;
    Role?: JobParticipantRole;
}
