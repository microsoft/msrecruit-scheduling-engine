//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface FeedbackParticipant extends Person {
    role: JobParticipantRole;
    feedbacks?: Feedback[];
}
