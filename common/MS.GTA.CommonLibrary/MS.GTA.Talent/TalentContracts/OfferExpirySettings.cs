//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Class for offer expiry properties
    /// </summary>
    [DataContract]
    public class OfferExpirySettings
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
