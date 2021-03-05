//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface PackageStatus {
    status: PackageStatusCode;
    installedVersion: string;
    packageName: string;
    details: string;
}
