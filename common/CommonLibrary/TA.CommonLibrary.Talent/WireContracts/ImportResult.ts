//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface ImportResult {
    index: number;
    isSuccessful: boolean;
    exceptionCode: string;
    jobOpeningId?: string;
}
