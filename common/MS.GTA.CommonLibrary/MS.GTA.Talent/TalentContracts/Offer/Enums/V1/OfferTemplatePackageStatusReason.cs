// <copyright file="OfferTemplatePackageStatusReason.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Template Status Reason
    /// </summary>
    [DataContract]
    public enum OfferTemplatePackageStatusReason
    {
        /// <summary>
        /// Draft
        /// </summary>
        Draft,

        /// <summary>
        /// Published
        /// </summary>
        Published,

        /// <summary>
        /// Versioned
        /// </summary>
        Versioned
    }
}
