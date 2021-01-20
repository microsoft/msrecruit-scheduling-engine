﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

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
