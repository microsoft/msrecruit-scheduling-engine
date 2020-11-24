// <copyright file="OfferDocumentSign.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;

    /// <summary>The project artifact.</summary>
    [DataContract]
    public class OfferDocumentSign
    {
        /// <summary>Gets or sets the Candidate Name.</summary>
        [DataMember(Name = "candidateName")]
        public string CandidateName { get; set; }

        /// <summary>Gets or sets the offer id.</summary>
        [DataMember(Name = "offerId")]
        public string OfferId { get; set; }

        /// <summary>Gets or sets the offer id.</summary>
        [DataMember(Name = "offerPackageDocumentId")]
        public string OfferPackageDocumentId { get; set; }
    }
}
