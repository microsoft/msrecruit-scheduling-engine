//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="OfferPackageDocument.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// Specifies the Data Contract for Offer Package Document
    /// </summary>
    [DataContract]
    public class OfferPackageDocument
    {
        /// <summary>
        /// Gets or sets offerPackageDocumentId.
        /// </summary>
        [DataMember(Name = "offerPackageDocumentId", IsRequired = true)]
        public string OfferPackageDocumentId { get; set; }

        /// <summary>
        /// Gets or sets Template ID.
        /// </summary>
        [DataMember(Name = "templateID", IsRequired = false)]
        public string TemplateID { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether documnet is required or not.
        /// </summary>
        [DataMember(Name = "IsRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Offer Text is Editable.
        /// </summary>
        [DataMember(Name = "isOfferTextEditable", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsOfferTextEditable { get; set; }

        /// <summary>
        /// Gets or sets Ordinal.
        /// </summary>
        [DataMember(Name = "ordinal", IsRequired = true)]
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets tokens.
        /// </summary>
        [DataMember(Name = "tokens", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> Tokens { get; set; }

        /// <summary>
        /// Gets or sets OfferArtifacts.
        /// </summary>
        [DataMember(Name = "offerArtifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferArtifact> OfferArtifacts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Offer Document deleted.
        /// </summary>
        [DataMember(Name = "isDeleted", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether candidate sign is required or not.
        /// </summary>
        [DataMember(Name = "isCandidateSignRequired")]
        public bool? IsCandidateSignRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether candidate has signed the document or not.
        /// </summary>
        [DataMember(Name = "candidateSigned")]
        public bool? CandidateSigned { get; set; }

        /// <summary>
        /// Gets or sets a value for candidate sign date
        /// </summary>
        [DataMember(Name = "candidateSignDate")]
        public DateTime? CandidateSignDate { get; set; }
    }
}
