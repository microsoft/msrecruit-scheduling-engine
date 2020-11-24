﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface PendingFeedback {
    StartDateTime?: Date;
    ModeOfInterview?: InterviewMode;
    PositionTitle?: string;
    InterviewerName?: string;
    CandidateName?: string;
    InterviewerOID?: string;
    jobApplicationID?: string;
    Roles?: JobParticipantRole[];
}