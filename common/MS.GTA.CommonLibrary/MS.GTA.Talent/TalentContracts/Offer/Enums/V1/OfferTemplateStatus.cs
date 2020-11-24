// <copyright file="OfferTemplateStatus.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Template Status
    /// </summary>
    [DataContract]
    public enum OfferTemplateStatus
    {
        /// <summary>
        /// Active
        /// </summary>
        Active,

        /// <summary>
        /// Archive
        /// </summary>
        Archive,

        /// <summary>
        /// Draft
        /// </summary>
        Draft,

        /// <summary>
        /// Inactive
        /// </summary>
        Inactive
    }
}
