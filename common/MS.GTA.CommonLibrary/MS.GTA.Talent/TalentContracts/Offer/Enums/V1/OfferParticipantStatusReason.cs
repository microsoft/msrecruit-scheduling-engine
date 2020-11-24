// <copyright file="OfferParticipantStatusReason.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Participation Status Reason
    /// </summary>
    [DataContract]
    public enum OfferParticipantStatusReason
    {
        /// <summary>
        /// Not started
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
        /// Send for review
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
