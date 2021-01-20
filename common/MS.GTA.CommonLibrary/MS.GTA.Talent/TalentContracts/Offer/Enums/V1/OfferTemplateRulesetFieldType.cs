// <copyright file="OfferTemplateRulesetFieldType.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Data type of the ruleset
    /// </summary>
    [DataContract]
    public enum OfferTemplateRulesetFieldType
    {
        /// <summary>
        /// Free Text
        /// </summary>
        FreeText,

        /// <summary>
        /// Numeric Range
        /// </summary>
        NumericRange,

        /// <summary>
        /// Clause Token
        /// </summary>
        Clause = 2,
    }
}
