//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleInvitationDetailsV2 {
    requesterName?: string;
    requesterId?: string;
    requesterTitle?: string;
    requesterEmail?: string;
    requesterGivenName?: string;
    requesteMobilePhoner?: string;
    requesterOfficeLocation?: string;
    requesterPreferredLanguage?: string;
    requesterSurname?: string;
    requesterUserPrincipalName?: string;
    requesterInvitationResponseStatus?: string;
    requesterComments?: string;
    primaryEmailRecipients: string[];
    ccEmailAddressList?: string[];
    bccEmailAddressList?: string[];
    subject: string;
    emailContent: string;
    emailAttachmentFiles?: File[];
    emailAttachmentFileLabels?: string[];
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
