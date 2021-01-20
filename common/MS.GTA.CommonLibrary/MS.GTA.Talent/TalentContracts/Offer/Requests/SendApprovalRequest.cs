// <copyright file="SendApprovalRequest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.BusinessLibrary.Requests
{
    using MS.GTA.Common.OfferManagement.Contracts.V1;

    /// <summary>
    /// Extend Offer Request
    /// </summary>
    public class SendApprovalRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets Offer ID
        /// </summary>
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy the mail to owner?
        /// </summary>
        public bool IsCopyToOwner { get; set; }

        /// <summary>
        /// Gets or sets a value of the email
        /// </summary>
        public Email EmailDetails { get; set; }

        /// <summary>
        /// Gets or sets NonStandardReason Text
        /// </summary>
        public string NonStandardReason { get; set; }

        /// <summary>
        /// Gets or sets the environment ID.
        /// </summary>
        public string EnvironmentId { get; set; }
    }
}
