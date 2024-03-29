﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Class for offer feature properties
    /// </summary>
    [DataContract]
    public class OfferFeature
    {
        /// <summary>
        /// Gets or sets offer feature name
        /// </summary>
        [DataMember(Name = "offerFeature", IsRequired = false, EmitDefaultValue = true)]
        public OfferFeatureName OfferFeatureName { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether offer feature is enabled or not.
        /// </summary>
        [DataMember(Name = "isEnabled", IsRequired = false, EmitDefaultValue = true)]
        public bool IsEnabled { get; set; }
    }
}
