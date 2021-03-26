//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.Enums.V1
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
