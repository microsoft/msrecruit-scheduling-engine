//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface TalentPoolParticipant extends Person {
    role?: TalentPoolParticipantRole;
    userAction?: UserAction;
}
