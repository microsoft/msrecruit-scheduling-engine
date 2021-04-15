//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface EnvironmentDocument extends Document {
    environmentStatus: EnvironmentStatus;
    environmentHistory: HistoryEvent[];
    createdDate: Date;
}
