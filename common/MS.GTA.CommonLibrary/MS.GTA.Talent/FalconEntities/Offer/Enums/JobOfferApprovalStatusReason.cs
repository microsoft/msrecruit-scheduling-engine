// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferApprovalStatusReason.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferApprovalStatusReason
    {
        /// <summary> notStarted </summary>
        NotStarted = 0,

        /// <summary> approved </summary>
        Approved = 1,

        /// <summary> sendback </summary>
        Sendback = 2,

        /// <summary> sentForReview </summary>
        SentForReview = 3,

        /// <summary>  waitingForReview </summary>
        WaitingForReview = 4,

        /// <summary> skipped </summary>
        Skipped = 5

    }
}
