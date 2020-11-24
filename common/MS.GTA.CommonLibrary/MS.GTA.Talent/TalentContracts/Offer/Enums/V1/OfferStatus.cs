// <copyright file="OfferStatus.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Status
    /// </summary>
    [DataContract]
    public enum OfferStatus
    {
        /// <summary>
        /// Active
        /// </summary>
        Active,

        /// <summary>
        /// Inactive
        /// </summary>
        Inactive
    }
}