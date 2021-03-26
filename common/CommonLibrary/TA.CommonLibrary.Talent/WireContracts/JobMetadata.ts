//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface JobMetadata {
    jobs?: Job[];
    total?: number;
    continuationToken?: string;
}
