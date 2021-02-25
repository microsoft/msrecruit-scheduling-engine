//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleInvitationRequest {
    subject: string;
    emailContent: string;
    emailAttachments?: FileAttachmentRequest;
    isInterviewTitleShared: boolean;
    isInterviewScheduleShared: boolean;
    primaryEmailRecipient: string;
    ccEmailAddressList?: string[];
    sharedSchedules?: CandidateScheduleCommunication[];
}
