//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Application extends TalentBaseContract {
    id?: string;
    candidate?: Applicant;
    hiringTeam?: HiringTeamMember[];
    externalStatus?: string;
    status?: JobApplicationStatus;
    statusReason?: JobApplicationStatusReason;
    rejectionReason?: OptionSetValue;
    currentStage?: JobStage;
    currentApplicationStage?: ApplicationStage;
    currentStageStatus?: JobApplicationStageStatus;
    currentStageStatusReason?: JobApplicationStageStatusReason;
    assessmentStatus?: AssessmentStatus;
    invitationId?: string;
    applicationDate?: Date;
    comment?: string;
    schedules?: StageScheduleEvent[];
    externalSource?: JobApplicationExternalSource;
    externalId?: string;
    notes?: ApplicationNote[];
    stages?: ApplicationStage[];
    isProspect?: boolean;
    jobOpeningPosition?: JobOpeningPosition;
    extendedAttributes?: { [key: string]: string; };
    userPermissions?: ApplicationPermission[];
    applicationTalentSource?: TalentSource;
}
