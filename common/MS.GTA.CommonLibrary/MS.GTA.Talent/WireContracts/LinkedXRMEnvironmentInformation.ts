//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface LinkedXRMEnvironmentInformation {
    instanceId: string;
    instanceUrl: string;
    instanceApiUrl: string;
    createdTime: Date;
    modifiedTime: Date;
    hostNameSuffix: string;
    localeId: number;
    initialUserObjectId: string;
    friendlyName: string;
    uniqueName: string;
    domainName: string;
    hasAttractServiceEndpointConfigured: boolean;
}
