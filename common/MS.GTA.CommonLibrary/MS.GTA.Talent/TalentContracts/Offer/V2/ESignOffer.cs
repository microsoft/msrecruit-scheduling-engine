//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;
    using MS.GTA.Common.TalentAttract.Contract;

    /// <summary>
    /// Esign Used for Offer
    /// </summary>
    [DataContract]
    public class ESignOffer
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the artifact identifier.
        /// </summary>
        /// <value>
        /// The artifact identifier.
        /// </value>
        [DataMember(Name = "offerId")]
        public string OfferID { get; set; }

        /// <summary>
        /// Gets or sets the external document identifier.
        /// </summary>
        /// <value>
        /// The external document identifier.
        /// </value>
        [DataMember(Name = "externalDocumentId")]
        public string ExternalDocumentID { get; set; }

        /// <summary>
        /// Gets or sets the esign type selected.
        /// </summary>
        /// <value>
        /// The esign type selected.
        /// </value>
        [DataMember(Name = "esignTypeSelected")]
        public ESignType EsignTypeSelected { get; set; }

        /// <summary>
        /// Gets or sets the name of the signing user.
        /// </summary>
        /// <value>
        /// The name of the signing user.
        /// </value>
        [DataMember(Name = "signingUserName")]
        public string SigningUserName { get; set; }

        /// <summary>
        /// Gets or sets the user object identifier.
        /// </summary>
        /// <value>
        /// The user object identifier.
        /// </value>
        [DataMember(Name = "userObjectId")]
        public string UserObjectId { get; set; }
    }
}
