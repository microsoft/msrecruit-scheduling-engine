//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Participation Status
    /// </summary>
    [DataContract]
    public enum OfferParticipantStatus
    {
        /// <summary>
        /// NotStarted
        /// </summary>
        NotStarted,

        /// <summary>
        /// Approved
        /// </summary>
        Approved,

        /// <summary>
        /// Sendback
        /// </summary>
        Sendback,

        /// <summary>
        /// SentForReview
        /// </summary>
        SentForReview,

        /// <summary>
        /// WaitingForReview
        /// </summary>
        WaitingForReview,

        /// <summary>
        /// Skipped
        /// </summary>
        Skipped
    }
}
