//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>The job offer view for candidate app.</summary>
    [DataContract]
    public class JobOffer
    {
        /// <summary>Gets or sets url.</summary>
        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }

        /// <summary>Gets or sets the status.</summary>
        [DataMember(Name = "status", IsRequired = false)]
        public JobOfferStatus Status { get; set; }

        /// <summary>Gets or sets the job offer status reason.</summary>
        [DataMember(Name = "jobOfferStatusReason", IsRequired = false)]
        public Provisioning.Entities.XrmEntities.Optionset.JobOfferStatusReason? JobOfferStatusReason { get; set; }

        /// <summary>Gets or sets the status.</summary>
        [DataMember(Name = "offerDate", IsRequired = false)]
        public DateTime? OfferDate { get; set; }
    }
}
