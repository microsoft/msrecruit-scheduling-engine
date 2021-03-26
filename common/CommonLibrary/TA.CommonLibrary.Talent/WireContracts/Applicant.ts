//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Applicant extends Person {
    id?: string;
    guid?: string;
    b2cObjectId?: string;
    status?: CandidateStatus;
    statusReason?: CandidateStatusReason;
    externalId?: string;
    externalSource?: CandidateExternalSource;
    preferredPhone?: CandidatePreferredPhone;
    internal?: boolean;
    gender?: CandidateGender;
    ethnicOrigin?: CandidateEthnicOrigin;
    attachments?: ApplicantAttachment[];
    alertOnApplicationChange?: boolean;
    trackings?: ApplicantTagTracking[];
    jobs?: Job[];
    extendedAttributes?: { [key: string]: string; };
    isProspect?: boolean;
    workExperience?: WorkExperience[];
    education?: Education[];
    skillSet?: string[];
    socialIdentities?: SocialIdentity[];
    rank?: Rank;
    candidateTalentSource?: TalentSource;
}
