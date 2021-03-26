//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// The Offer data contract
    /// </summary>
    [DataContract]
    public class OfferActivity
    {
        /// <summary>Gets or sets url.</summary>
        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }

        /// <summary>Gets or sets the status.</summary>
        [DataMember(Name = "status", IsRequired = false)]
        public OfferStatus Status { get; set; }

        /// <summary>Gets or sets the job offer status reason.</summary>
        [DataMember(Name = "jobOfferStatusReason", IsRequired = false)]
        public OfferStatusReason? JobOfferStatusReason { get; set; }

        /// <summary>Gets or sets the OfferDate.</summary>
        [DataMember(Name = "offerDate", IsRequired = false)]
        public DateTime? OfferDate { get; set; }

        /// <summary>
        /// Gets or sets job hiring team.
        /// </summary>
        [DataMember(Name = "hiringTeam", IsRequired = false, EmitDefaultValue = false)]
        public IList<OfferParticipant> HiringTeam { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offer valid for candidate.
        /// </summary>
        [DataMember(Name = "isValidOfferVersion", IsRequired = false)]
        public bool? IsValidOfferVersion { get; set; }
    }
}
