//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface UserESignConfiguration {
    id?: string;
    oid?: string;
    refreshToken?: string;
    esignTypeSelected?: ESignType;
    tenantId?: string;
    environmentId?: string;
    emailAddress?: string;
    apiAccessPoint?: string;
    webAccessPoint?: string;
}
