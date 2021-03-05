//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface TalentPool {
    poolId?: string;
    poolName?: string;
    description?: string;
    candidates?: Applicant[];
    candidateCount?: number;
    contributors?: TalentPoolParticipant[];
    lastModified?: Date;
    source?: TalentPoolSource;
    externalId?: string;
    userPermissions?: TalentPoolPermission[];
}
