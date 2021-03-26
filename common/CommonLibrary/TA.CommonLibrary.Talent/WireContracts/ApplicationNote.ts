//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicationNote extends TalentBaseContract {
    id?: string;
    text?: string;
    visibility?: CandidateNoteVisibility;
    ownerObjectId?: string;
    ownerName?: string;
    ownerEmail?: string;
    createdDate?: Date;
    noteType?: CandidateNoteType;
}
