//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.OfferManagement.Contracts.Enums.V1;
    using HR.TA.Common.OfferManagement.Contracts.V1;

    /// <summary>
    /// The Offer data contract
    /// </summary>
    [DataContract]
    public class OfferTemplatePackage
    {
        /// <summary>
        /// Gets or sets OfferID of model
        /// </summary>
        [DataMember(Name = "offerID", IsRequired = true, EmitDefaultValue = false)]
        public string OfferID { get; set; }

        /// <summary>
        /// Gets or sets TokenValues
        /// </summary>
        [DataMember(Name = "tokenValues", IsRequired = false, EmitDefaultValue = false)]
        public IList<TokenValue> TokenValues { get; set; }

        /// <summary>
        /// Gets or sets templatePackageID for the offer
        /// </summary>
        [DataMember(Name = "templatePackageID", IsRequired = false, EmitDefaultValue = false)]
        public string TemplatePackageID { get; set; }

        /// <summary>
        /// Gets or sets offer Package Documents.
        /// </summary>
        [DataMember(Name = "offerPackageDocuments", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferPackageDocument> OfferPackageDocuments { get; set; }

        /// <summary>
        /// Gets or sets optional tokens.
        /// </summary>
        [DataMember(Name = "optionalTokens", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> OptionalTokens { get; set; }

        /// <summary>
        /// Gets or sets optional tokens.
        /// </summary>
        [DataMember(Name = "offerArtifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferArtifact> OfferArtifacts { get; set; }
    }
}