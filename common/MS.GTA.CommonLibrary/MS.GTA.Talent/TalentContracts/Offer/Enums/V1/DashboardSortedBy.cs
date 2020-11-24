﻿// <copyright file="DashboardSortedBy.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Dashboard Sort By
    /// </summary>
    [DataContract]
    public enum DashboardSortedBy
    {
        /// <summary>
        /// Name
        /// </summary>
        Name,

        /// <summary>
        /// Modified date
        /// </summary>
        ModifiedDate
    }
}
