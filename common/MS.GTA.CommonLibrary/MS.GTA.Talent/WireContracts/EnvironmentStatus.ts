//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EnvironmentStatus {
    bapLocation: string;
    clusterUri: string;
    endDate: Date;
    environmentStatusCode: EnvironmentStatusCode;
    id: string;
    isTenantTakenOver: boolean;
    isRelSearchEnabledOnFirstLogin?: boolean;
    namespaceId: string;
    namespaceRuntimeUri: string;
    packageStatuses: PackageStatus[];
    previousTenantId: string;
    startDate: Date;
    tenantId: string;
    testDrive: boolean;
    errorDetails: string;
    deletionDetails: string;
    environmentMode: EnvironmentMode;
    falconDatabaseId?: string;
    falconResourceName?: string;
    expirationDate?: Date;
    linkedXRMEnvironmentInformation?: LinkedXRMEnvironmentInformation;
}
