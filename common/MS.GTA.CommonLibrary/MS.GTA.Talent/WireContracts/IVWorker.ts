//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface IVWorker {
    workerId?: string;
    profession?: string;
    name?: PersonName;
    fullName?: string;
    emailPrimary?: string;
    alias?: string;
    phonePrimary?: string;
    officeGraphIdentifier?: string;
    isEmailContactAllowed?: boolean;
}
