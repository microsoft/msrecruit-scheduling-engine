//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Attract.Contract
{
    using CommonLibrary.Common.TalentAttract.Contract;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The offer setting data contract.
    /// </summary>
    [DataContract]
    public class CandidateOfferSettings
    {
        /// <summary>
        /// Gets or sets offer feature.
        /// </summary>
        [DataMember(Name = "offerFeature", IsRequired = false, EmitDefaultValue = true)]
        public IEnumerable<OfferFeature> OfferFeature { get; set; }

        /// <summary>
        /// Gets or sets value of post offer redirection settings
        /// </summary>
        [DataMember(Name = "offerAcceptanceRedirectionSettings", IsRequired = false, EmitDefaultValue = false)]
        public OfferAcceptanceRedirectionSettings OfferAcceptanceRedirectionSettings { get; set; }
    }
}
