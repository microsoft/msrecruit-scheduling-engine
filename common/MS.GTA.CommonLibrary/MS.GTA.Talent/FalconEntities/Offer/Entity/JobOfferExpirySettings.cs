﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferExpirySettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.Offer.Entity
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Class for offer expiry properties
    /// </summary>
    [DataContract]
    public class JobOfferExpirySettings
    {
        /// <summary>
        /// Gets or sets value indicating if it is custom date
        /// </summary>
        [DataMember(Name = "isCustomDate", IsRequired = false, EmitDefaultValue = true)]
        public bool IsCustomDate { get; set; }

        /// <summary>
        /// Gets or sets value indicating expiry datys
        /// </summary>
        [DataMember(Name = "expiryDays", IsRequired = false, EmitDefaultValue = true)]
        public int? ExpiryDays { get; set; }
    }
}
