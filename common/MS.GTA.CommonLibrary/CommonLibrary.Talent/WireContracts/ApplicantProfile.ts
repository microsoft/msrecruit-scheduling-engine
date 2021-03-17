//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ApplicantProfile extends Person {
    id?: string;
    guid?: string;
    b2cObjectId?: string;
    isInternal?: boolean;
    status?: CandidateStatus;
    statusReason?: CandidateStatusReason;
    externalSource?: CandidateExternalSource;
    preferredPhone?: CandidatePreferredPhone;
    gender?: CandidateGender;
    ethnicOrigin?: CandidateEthnicOrigin;
    attachments?: ApplicantAttachment[];
    alertOnApplicationChange?: boolean;
    extendedAttributes?: { [key: string]: string; };
    workExperience?: WorkExperience[];
    education?: Education[];
    skillSet?: string[];
    talentSource?: TalentSource;
}
