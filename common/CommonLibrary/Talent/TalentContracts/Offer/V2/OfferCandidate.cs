//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.V1;

    /// <summary>
    /// The OfferCandidate data contract
    /// </summary>
    [DataContract]
    public class OfferCandidate
    {
        /// <summary>
        /// Gets or sets OfferActivity
        /// </summary>
        [DataMember(Name = "offerActivity", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<OfferActivity> OfferActivity { get; set; }

        /// <summary>
        /// Gets or sets OfferDocument
        /// </summary>
        [DataMember(Name = "offerDocument", IsRequired = false, EmitDefaultValue = false)]
        public OfferDocument OfferDocument { get; set; }

        /// <summary>
        /// Gets or sets OfferPackageDocument
        /// </summary>
        [DataMember(Name = "offerPackageDocument", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferPackageDocument> OfferPackageDocument { get; set; }
    }
}
