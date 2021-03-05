//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface CandidatePersonalDetails extends TalentBaseContract {
    Gender?: CandidateGender;
    Ethnicity?: CandidateEthnicOrigin;
    DisabilityStatus?: CandidateDisabilityStatus;
    VeteranStatus?: CandidateVeteranStatus;
    MilitaryStatus?: CandidateMilitaryStatus;
    submittedOn?: Date;
    jobApplicationActivityId?: string;
}
