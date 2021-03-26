//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;

    /// <summary>
    /// Approval email of an Offer.
    /// </summary>
    public class OfferApprovalEmail
    {
        /// <summary>
        /// Gets or sets Offer ID
        /// </summary>
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets EmailSubject
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets EmailContent
        /// </summary>
        public string EmailContent { get; set; }

        /// <summary>
        /// Gets or sets Email Cc
        /// </summary>
        public List<string> EmailCc { get; set; }
    }
}