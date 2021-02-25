//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.Offer.Entity
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Class for offer feature properties
    /// </summary>
    [DataContract]
    public class JobOfferFeature
    {
        /// <summary>
        /// Gets or sets offer feature name
        /// </summary>
        [DataMember(Name = "offerFeature", IsRequired = false, EmitDefaultValue = true)]
        public JobOfferFeatureName OfferFeatureName { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether offer feature is enabled or not.
        /// </summary>
        [DataMember(Name = "isEnabled", IsRequired = false, EmitDefaultValue = true)]
        public bool IsEnabled { get; set; }
    }
}
