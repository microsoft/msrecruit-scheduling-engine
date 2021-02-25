//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface GaugeAssessmentResultPayload {
    percentage: number;
    status?: GaugeAssessmentStatus;
    subject?: string;
    gradeUrl?: string;
    detailUrl?: string;
    additionalInformation?: string;
}
