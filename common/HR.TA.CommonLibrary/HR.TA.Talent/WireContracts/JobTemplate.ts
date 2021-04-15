//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobTemplate extends TalentBaseContract {
    id?: string;
    name?: string;
    displayName?: string;
    validFrom?: Date;
    validTo?: Date;
    isActive?: boolean;
    isDefault?: boolean;
    templateReference?: string;
    stageTemplates?: JobStageTemplate[];
}
