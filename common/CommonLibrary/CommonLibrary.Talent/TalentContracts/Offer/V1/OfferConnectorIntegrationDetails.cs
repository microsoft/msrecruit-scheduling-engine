//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    /// <summary>
    /// The Entity used to push message to Service bus Queue for communication with Connector Service
    /// </summary>
    public class OfferConnectorIntegrationDetails
    {
        /// <summary>
        /// Gets or sets Id of the Offer
        /// </summary>
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets Job Application Id
        /// </summary>
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Previous Offer Id
        /// </summary>
        public string PreviousOfferId { get; set; }

        /// <summary>
        /// Gets or sets offer status
        /// </summary>
        public string OfferStatus { get; set; }

        /// <summary>
        /// Gets or sets offer statusreason
        /// </summary>
        public string OfferStatusReason { get; set; }

        /// <summary>
        /// Gets or sets Root Activity Id
        /// </summary>
        public string RootActivityId { get; set; }

        /// <summary>
        /// Gets or sets message action type
        /// </summary>
        public string MessageActionType { get; set; }
    }
}
