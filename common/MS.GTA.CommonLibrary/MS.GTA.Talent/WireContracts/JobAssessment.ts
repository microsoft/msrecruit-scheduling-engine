﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface JobAssessment {
    jobAssessmentID?: string;
    packageID?: string;
    jobOpeningID?: string;
    provider?: AssessmentProvider;
    providerKey?: string;
    title?: string;
    numberOfQuestions?: number;
    previewURL?: string;
    assessment?: ExternalAssessment;
    isRequired?: JobOpeningAssessmentRequirementStatus;
    stage?: JobStage;
}
