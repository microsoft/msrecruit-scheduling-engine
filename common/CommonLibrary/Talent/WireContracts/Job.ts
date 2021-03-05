//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Job extends TalentBaseContract {
    id?: string;
    title?: string;
    description?: string;
    numberOfOpenings?: number;
    numberOfOffers?: number;
    status?: JobOpeningStatus;
    statusReason?: JobOpeningStatusReason;
    location?: string;
    source?: JobOpeningExternalSource;
    externalId?: string;
    externalStatus?: string;
    createdDate?: Date;
    comment?: string;
    primaryPositionID?: string;
    positionURI?: string;
    applyURI?: string;
    skills?: string[];
    seniorityLevel?: string;
    seniorityLevelValue?: OptionSetValue;
    employmentType?: string;
    employmentTypeValue?: OptionSetValue;
    jobFunctions?: string[];
    companyIndustries?: string[];
    isTemplate?: boolean;
    jobTemplate?: JobTemplate;
    stages?: ApplicationStage[];
    hiringTeam?: HiringTeamMember[];
    applications?: Application[];
    externalJobPost?: ExternalJobPost[];
    templateParticipants?: JobOpeningTemplateParticipant[];
    jobOpeningVisibility?: JobOpeningVisibility;
    jobOpeningPositions?: JobOpeningPosition[];
    positionStartDate?: Date;
    positionEndDate?: Date;
    applicationStartDate?: Date;
    applicationCloseDate?: Date;
    postalAddress?: Address;
    extendedAttributes?: { [key: string]: string; };
    configuration?: string;
    approvalParticipants?: JobApprovalParticipant[];
    delegates?: Delegate[];
    userPermissions?: JobPermission[];
    priority?: string;
}
