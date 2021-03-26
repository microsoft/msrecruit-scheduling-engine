//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export interface IVUserProfile {
    ivperson: IVPerson;
    ivroles: IVApplicationRole[];
    firsttimelogin?: Date;
}
