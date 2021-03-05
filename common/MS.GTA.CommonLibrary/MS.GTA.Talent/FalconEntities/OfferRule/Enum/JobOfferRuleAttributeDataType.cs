//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Provisioning.Entities.FalconEntities.OfferRule
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
