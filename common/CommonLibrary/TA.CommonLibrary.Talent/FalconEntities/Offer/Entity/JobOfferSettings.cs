//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.FalconEntities.Offer.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.DocumentDB.Contracts;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// The job offer setting data contract.
    /// </summary>
    [DataContract]
    public class JobOfferSettings : DocDbEntity
    {
        /// <summary>
        /// Gets or sets offer feature.
        /// </summary>
        [DataMember(Name = "offerFeature", IsRequired = false, EmitDefaultValue = true)]
        public IEnumerable<JobOfferFeature> OfferFeature { get; set; }

        /// <summary>
        /// Gets or sets value of expiry date property
        /// </summary>
        [DataMember(Name = "offerExpiry", IsRequired = false, EmitDefaultValue = false)]
        public JobOfferExpirySettings OfferExpiry { get; set; }

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
        public JobOfferAcceptanceRedirectionSettings OfferAcceptanceRedirectionSettings { get; set; }
    }
}
