//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Gets the details for reseting the expiration of an offer
    /// </summary>
    [DataContract]
    public class ResetExpirationDetails
    {
        /// <summary>
        /// Gets or sets Offer Id
        /// </summary>
        [DataMember(Name = "offerId", IsRequired = true, EmitDefaultValue = false)]
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets expiration date
        /// </summary>
        [DataMember(Name = "expirationDate", IsRequired = true, EmitDefaultValue = false)]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets Tenant Id
        /// </summary>
        [IgnoreDataMember]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets Environment Id
        /// </summary>
        [IgnoreDataMember]
        public string EnvironmentId { get; set; }
    }
}
