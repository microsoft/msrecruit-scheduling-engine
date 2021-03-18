//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Status Reason
    /// </summary>
    [DataContract]
    public enum OfferStatusReason
    {
        /// <summary>
        /// New
        /// </summary>
        New,

        /// <summary>
        /// Draft
        /// </summary>
        Draft,

        /// <summary>
        /// Review
        /// </summary>
        Review,

        /// <summary>
        /// Approved
        /// </summary>
        Approved,

        /// <summary>
        /// Published
        /// </summary>
        Published,

        /// <summary>
        /// Accepted
        /// </summary>
        Accepted,

        /// <summary>
        /// Withdrawn with Candidate Dispositioned reason
        /// </summary>
        WithdrawnCandidateDispositioned,

        /// <summary>
        /// Need Revision
        /// </summary>
        NeedRevision,

        /// <summary>
        /// Withdrawn with Other reason
        /// </summary>
        WithdrawnOther,

        /// <summary>
        /// candidate declined
        /// </summary>
        Declined,

        /// <summary>
        /// Closed
        /// </summary>
        Closed,

        /// <summary>
        /// Expired
        /// </summary>
        Expired
    }
}
