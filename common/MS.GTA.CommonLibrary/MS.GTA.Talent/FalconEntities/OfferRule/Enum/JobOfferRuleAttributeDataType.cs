// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRuleAttributeDataType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferRuleAttributeDataType
    {
        /// <summary>
        /// FreeText
        /// </summary>
        FreeText = 0,

        /// <summary>
        /// NumericRange
        /// </summary>
        NumericRange = 1,

        /// <summary>
        /// Clause Token
        /// </summary>
        Clause = 2,
    }
}
