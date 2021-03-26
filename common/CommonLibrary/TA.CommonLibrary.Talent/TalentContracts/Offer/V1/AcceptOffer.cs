//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>The project artifact.</summary>
    [DataContract]
    public class AcceptOffer
    {
        /// <summary>Gets or sets the Candidate Name.</summary>
        [DataMember(Name = "candidateName")]
        public string CandidateName { get; set; }

        /// <summary>Gets or sets a value indicating whether offer is accepted or not.</summary>
        [DataMember(Name = "offerAccepted")]
        public bool OfferAccepted { get; set; }

        /// <summary>Gets or sets the offer id.</summary>
        [DataMember(Name = "offerId")]
        public string OfferId { get; set; }
    }
}
