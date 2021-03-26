//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V1;

    /// <summary>
    /// The Offer data contract
    /// </summary>
    [DataContract]
    public class OfferDocument
    {
        /// <summary>
        /// Gets or sets OfferID of model
        /// </summary>
        [DataMember(Name = "offerID", IsRequired = true, EmitDefaultValue = false)]
        public string OfferID { get; set; }

        /// <summary>
        /// Gets or sets Offer artifacts.
        /// </summary>
        [DataMember(Name = "offerArtifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferArtifact> OfferArtifacts { get; set; }

        /// <summary>
        /// Gets or sets Date when candidate uploads the documents successfully
        /// </summary>
        [DataMember(Name = "candidateDocumentUploadDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CandidateDocumentUploadDate { get; set; }

        /// <summary>
        /// Gets or sets Offer expiration date.
        /// </summary>
        [DataMember(Name = "offerExpirationDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? OfferExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets Required Candidate Documents.
        /// </summary>
        [DataMember(Name = "requiredCandidateDocuments", IsRequired = false, EmitDefaultValue = false)]
        public List<string> RequiredCandidateDocuments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets offer expiry status.
        /// </summary>
        [DataMember(Name = "IsOfferExpired", IsRequired = false, EmitDefaultValue = false)]
        public bool IsOfferExpired { get; set; }
    }
}
