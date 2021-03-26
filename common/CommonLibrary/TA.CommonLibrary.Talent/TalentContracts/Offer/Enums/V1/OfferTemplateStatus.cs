//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1
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
