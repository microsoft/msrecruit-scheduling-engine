// <copyright file="OfferTemplateStatusReason.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Template Status Reason
    /// </summary>
    [DataContract]
    public enum OfferTemplateStatusReason
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
