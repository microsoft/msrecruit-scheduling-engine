//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateOfferSettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Attract.Contract
{
    using MS.GTA.Common.TalentAttract.Contract;
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
