// <copyright file="DashboardFilterBy.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Dashboard Sort By
    /// </summary>
    [DataContract]
    public enum DashboardFilterBy
    {
        /// <summary>
        /// Name
        /// </summary>
        Active,

        /// <summary>
        /// Draft
        /// </summary>
        Draft,

        /// <summary>
        /// Archived
        /// </summary>
        Archived
    }
}
