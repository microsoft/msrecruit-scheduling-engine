//----------------------------------------------------------------------------
// <copyright file="JobOfferDetail.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2.Gdpr
{
    using System;
    using System.Collections.Generic;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The person data contract.
    /// </summary>
    public class JobOfferDetail
    {
        /// <summary>
        /// Gets or sets Offer Id.
        /// </summary>
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets Application Id.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets offer comment of Candidate.
        /// </summary>
        public string CandidateComment { get; set; }

        /// <summary>
        /// Gets or sets candidate response date
        /// </summary>
        public DateTime? CandidateResponseDate { get; set; }

        /// <summary>
        /// Gets or sets offer comment of Owner.
        /// </summary>
        public string OwnerComment { get; set; }

        /// <summary>
        /// Gets or sets offer status.
        /// </summary>
        public OfferStatus? OfferStatus { get; set; }

        /// <summary>
        /// Gets or sets offer status reason.
        /// </summary>
        public OfferStatusReason? OfferStatusReason { get; set; }

        /// <summary>
        /// Gets or sets next version offer Id
        /// </summary>
        public string NextOfferId { get; set; }

        /// <summary>
        /// Gets or sets previous offer Id
        /// </summary>
        public string PreviousOfferId { get; set; }

        /// <summary>
        /// Gets or sets Decline comment
        /// </summary>
        public string DeclineComment { get; set; }

        /// <summary>
        /// Gets or sets Decline Reasons
        /// </summary>
        public OfferDeclineReason[] DeclineReasons { get; set; }

        /// <summary>
        /// Gets or sets non standard offer notes
        /// </summary>
        public string NonStandardOfferNotes { get; set; }

        /// <summary>
        /// Gets or sets close offer comments
        /// </summary>
        public string CloseOfferComment { get; set; }

        /// <summary>
        /// Gets or sets participant details
        /// </summary>
        public List<JobOfferParticipantDetail> OfferParticipants { get; set; }

        /// <summary>
        /// Gets or sets offer tokens detail
        /// </summary>
        public List<JobOfferTokensDetail> OfferTokens { get; set; }
    }
}
