//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface Department {
    id?: string;
    description?: string;
    name?: string;
    parentDepartment?: Department;
    jobPositions?: JobOpeningPosition[];
}
