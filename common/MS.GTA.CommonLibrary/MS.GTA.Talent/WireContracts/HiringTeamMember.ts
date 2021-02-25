//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface HiringTeamMember extends Person {
    role: JobParticipantRole;
    userAction?: UserAction;
    title?: string;
    ordinal?: number;
    activities?: Activity[];
    feedbacks?: Feedback[];
    delegates?: Delegate[];
    metadata?: JobApplicationParticipantMetadata;
    isDeleteAllowed?: boolean;
    AddedOnDate?: Date;
}
