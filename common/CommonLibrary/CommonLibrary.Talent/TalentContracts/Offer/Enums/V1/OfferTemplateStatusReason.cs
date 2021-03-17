//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.Enums.V1
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
