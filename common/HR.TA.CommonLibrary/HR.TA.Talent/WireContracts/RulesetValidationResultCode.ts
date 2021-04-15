//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
export enum RulesetValidationResultCode {
    Ok = 0,
    InvalidNumericRange = 1,
    OutsideNumericRange = 2,
    UnknownNumericRange = 3,
    NonStandard = 4,
    UnknownHierarchy = 5,
    InactiveRuleset = 6,
    InvalidFields = 7,
    FieldsMismatch = 8,
    InvalidTokenValue = 9,
    Other = 10,
}
