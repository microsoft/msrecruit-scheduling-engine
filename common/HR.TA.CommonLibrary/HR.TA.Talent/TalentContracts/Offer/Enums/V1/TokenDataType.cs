﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Token Data Type
    /// </summary>
    [DataContract]
    public enum TokenDataType
    {
        /// <summary>
        /// Free text
        /// </summary>
        FreeText = 0,

        /// <summary>
        /// Numeric Value Range
        /// </summary>
        NumericRange = 1,

        /// <summary>
        /// Clause Token
        /// </summary>
        Clause = 2,
    }
}
