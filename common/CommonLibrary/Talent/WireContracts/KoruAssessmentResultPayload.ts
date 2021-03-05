//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface KoruAssessmentResultPayload {
    extraData?: any;
    webhookUrl?: string;
    processed?: boolean;
    tenantId?: string;
    scores?: string;
    candidate?: string;
    inProgress?: boolean;
    scored?: boolean;
    uuid?: string;
    initialized?: boolean;
    projectUuid?: string;
    profileUrl?: string;
}

export interface KoruAssessmentResultData {
    curiosity?: number;
    grit?: number;
    impact?: number;
    ownership?: number;
    polish?: number;
    rigor?: number;
    teamwork?: number;
    profileUrl?: string;
}
