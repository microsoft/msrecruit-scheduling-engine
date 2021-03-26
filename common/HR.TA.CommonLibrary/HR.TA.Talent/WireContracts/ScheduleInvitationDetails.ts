//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleInvitationDetails {
    requester?: GraphPerson;
    primaryEmailRecipients: string[];
    ccEmailAddressList?: string[];
    bccEmailAddressList?: string[];
    subject: string;
    emailContent: string;
    emailAttachments?: FileAttachmentRequest;
    interviewDate?: string;
    startTime?: string;
    endTime?: string;
    location?: string;
    skypeMeetingUrl?: string;
    interviewTitle?: string;
    ScheduleId?: string;
    IsInterviewTitleShared?: boolean;
    IsInterviewScheduleShared?: boolean;
    IsInterviewerNameShared?: boolean;
}
