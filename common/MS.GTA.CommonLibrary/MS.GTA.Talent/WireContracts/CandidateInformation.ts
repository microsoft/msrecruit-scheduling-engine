﻿//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// This file is auto-generated by the MS.GTA.Talent/TalentContracts/ContractGenerator.tst script.
//---------------------------------------------------------------------------

export interface CandidateInformation {
    CandidateID?: string;
    ExternalCandidateID?: string;
    OID: string;
    FullName: string;
    GivenName: string;
    MiddleName: string;
    Surname: string;
    EmailPrimary: string;
    PhonePrimary: string;
    AttachmentID: string;
    AttachmentName: string;
    Educations: CandidateEducationInformation[];
    WorkExperiences: CandidateWorkExperienceInformation[];
    Skills: CandidateSkillInformation[];
}

export interface CandidateEducationInformation {
    School: string;
    Degree: string;
    FieldOfStudy: string;
    Grade: string;
    ActivitiesSocieties: string;
    Description: string;
    FromMonthYear: Date;
    ToMonthYear: Date;
}

export interface CandidateWorkExperienceInformation {
    Title: string;
    Organization: string;
    Location: string;
    Description: string;
    IsCurrentPosition: boolean;
    FromMonthYear: Date;
    ToMonthYear: Date;
}

export interface CandidateSkillInformation {
    Skill: string;
}
