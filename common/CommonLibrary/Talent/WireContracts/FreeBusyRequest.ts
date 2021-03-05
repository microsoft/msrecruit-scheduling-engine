//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface FreeBusyRequest {
    userGroups: UserGroup[];
    utcStart?: Date;
    utcEnd?: Date;
    isRoom?: boolean;
}
