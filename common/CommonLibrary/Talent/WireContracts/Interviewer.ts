//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Interviewer {
    PrimaryEmail?: string;
    Name?: string;
    OfficeGraphIdentifier?: string;
    Profession?: string;
    Role?: JobParticipantRole;
    InterviewerResponseStatus?: InvitationResponseStatus;
    InterviewerComments?: string;
}
