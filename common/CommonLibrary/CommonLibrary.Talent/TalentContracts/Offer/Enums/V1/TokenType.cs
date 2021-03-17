//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Token Type
    /// </summary>
    [DataContract]
    public enum TokenType
    {
        /// <summary>
        /// System Defined Tokens
        /// </summary>
        SystemToken = 0,

        /// <summary>
        /// User Defined Tokens
        /// </summary>
        PlaceholderToken = 1,

        /// <summary>
        /// Ruleset Token
        /// </summary>
        RulesetToken = 2,
    }
}
