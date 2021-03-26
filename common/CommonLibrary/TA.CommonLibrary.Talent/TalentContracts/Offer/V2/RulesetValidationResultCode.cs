//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of the validation on a Ruleset
    /// </summary>
    [DataContract]
    public enum RulesetValidationResultCode
    {
        /// <summary>
        /// Ruleset token is valid
        /// </summary>
        Ok = 0,

        /// <summary>
        /// Numeric range token has non-numeric value
        /// </summary>
        InvalidNumericRange = 1,

        /// <summary>
        /// Numeric range token value is not within the range
        /// </summary>
        OutsideNumericRange = 2,

        /// <summary>
        /// Numeric range token could not be validated
        /// </summary>
        UnknownNumericRange = 3,

        /// <summary>
        /// Ruleset token value doesn't match any valid value
        /// </summary>
        NonStandard = 4,

        /// <summary>
        /// Ruleset hierarchy doesn't exist for the token value
        /// </summary>
        UnknownHierarchy = 5,

        /// <summary>
        /// No active ruleset exists
        /// </summary>
        InactiveRuleset = 6,

        /// <summary>
        /// Ruleset contains invalid fields
        /// </summary>
        InvalidFields = 7,

        /// <summary>
        /// Ruleset fields count mismatch for ruleset
        /// </summary>
        FieldsMismatch = 8,

        /// <summary>
        /// Ruleset token contains null or invalid value
        /// </summary>
        InvalidTokenValue = 9,

        /// <summary>
        /// Unknown state
        /// </summary>
        Other = 10
    }
}
