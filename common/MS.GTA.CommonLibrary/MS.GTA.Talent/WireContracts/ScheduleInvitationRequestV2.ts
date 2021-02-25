//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ScheduleInvitationRequestV2 {
    subject: string;
    emailContent: string;
    emailAttachmentFiles?: File[];
    emailAttachmentFileLabels?: string[];
    isInterviewTitleShared: boolean;
    isInterviewScheduleShared: boolean;
    primaryEmailRecipient: string;
    ccEmailAddressList?: string[];
    sharedSchedulesScheduleId?: string[];
    sharedSchedulesIsInterviewScheduleShared?: boolean[];
    sharedSchedulesIsInterviewerNameShared?: boolean[];

}
