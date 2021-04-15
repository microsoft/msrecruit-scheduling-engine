//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobOpeningPosition extends TalentBaseContract {
    jobOpeningPositionId?: string;
    jobId?: string;
    jobIds?: string[];
    title?: string;
    userAction?: UserAction;
    careerLevel?: string;
    costCenter?: string;
    sourcePositionNumber?: string;
    roleType?: string;
    incentivePlan?: string;
    jobGrade?: string;
    remunerationPeriod?: RenumerationPeriod;
    maximumRemuneration?: number;
    minimumRemuneration?: number;
    currencyCode?: CurrencyCode;
    department?: string;
    positionType?: JobOpeningPositionType;
    jobType?: string;
    jobFunction?: string;
    referenceJobOpeningIds?: string[];
    referenceApplicationIds?: string[];
    extendedAttributes?: { [key: string]: string; };
    reportsTo?: Worker;
    status?: JobPositionStatus;
    statusReason?: JobPositionStatusReason;
}
