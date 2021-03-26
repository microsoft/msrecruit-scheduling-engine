//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Offer Metadata
    /// </summary>
    [DataContract]
    public class OfferMetadata
    {
        /// <summary>
        /// Gets or sets offer information
        /// </summary>
        [DataMember(Name = "offerData")]
        public List<OfferDetail> OfferData { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in interview stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "offerDraftCount")]
        public int OfferDraftCount { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in offer stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "offerReviewCount")]
        public int OfferReviewCount { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in assessment stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "offerCompletedCount")]
        public int OfferCompletedCount { get; set; }
    }
}
