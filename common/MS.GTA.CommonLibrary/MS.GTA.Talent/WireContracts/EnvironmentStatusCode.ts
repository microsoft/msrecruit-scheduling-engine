//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export enum EnvironmentStatusCode {
    Unknown = 0,
    Installing = 1,
    Completed = 2,
    Upgrading = 3,
    Error = 4,
    NotStarted = 5,
    Deleting = 6,
    SoftDeleted = 7,
    Deleted = 8,
    Migrating = 9,
}
