// <copyright file="TokenDataType.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
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
