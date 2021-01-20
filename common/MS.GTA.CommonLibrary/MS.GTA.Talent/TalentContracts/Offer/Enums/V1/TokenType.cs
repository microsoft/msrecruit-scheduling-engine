// <copyright file="TokenType.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
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
