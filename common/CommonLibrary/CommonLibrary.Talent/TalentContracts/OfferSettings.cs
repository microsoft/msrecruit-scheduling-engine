//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The offer setting data contract.
    /// </summary>
    [DataContract]
    public class OfferSettings
    {
        /// <summary>
        /// Gets or sets offer feature.
        /// </summary>
        [DataMember(Name = "offerFeature", IsRequired = false, EmitDefaultValue = true)]
        public IEnumerable<OfferFeature> OfferFeature { get; set; }

        /// <summary>
        /// Gets or sets value of expiry date property
        /// </summary>
        [DataMember(Name = "offerExpiry", IsRequired = false, EmitDefaultValue = false)]
        public OfferExpirySettings OfferExpiry { get; set; }

        /// <summary>
        /// Gets or sets last modified by.
        /// </summary>
        [DataMember(Name = "modifiedBy", IsRequired = false, EmitDefaultValue = false)]
        public Person ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets last modified date time.
        /// </summary>
        [DataMember(Name = "modifiedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets value of post offer redirection settings
        /// </summary>
        [DataMember(Name = "offerAcceptanceRedirectionSettings", IsRequired = false, EmitDefaultValue = false)]
        public OfferAcceptanceRedirectionSettings OfferAcceptanceRedirectionSettings { get; set; }
    }
}
