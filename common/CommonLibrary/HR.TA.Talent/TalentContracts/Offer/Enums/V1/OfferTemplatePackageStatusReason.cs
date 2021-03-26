//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.Enums.V1
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
