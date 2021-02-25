//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface TalentSource extends TalentBaseContract {
    id?: string;
    name?: string;
    domain?: string;
    description?: string;
    talentSourceCategory?: OptionSetValue;
    referalReference?: Person;
}
