//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>The job offer view for attract app, used by recruiter and hiring manager.</summary>
    [DataContract]
    public class JobOfferHiringTeamView
    {
        /// <summary>Gets or sets job application ID.</summary>
        [DataMember(Name = "jobApplicationID", IsRequired = false)]
        public string JobApplicationID { get; set; }

        /// <summary>Gets or sets job offer url that links to offer management app.</summary>
        [DataMember(Name = "jobOfferID", IsRequired = false)]
        public string JobOfferID { get; set; }

        /// <summary>Gets or sets the job offer status.</summary>
        [DataMember(Name = "jobOfferStatus", IsRequired = false)]
        public JobOfferStatus? JobOfferStatus { get; set; }

        /// <summary>Gets or sets the job offer status reason.</summary>
        [DataMember(Name = "jobOfferStatusReason", IsRequired = false)]
        public Provisioning.Entities.XrmEntities.Optionset.JobOfferStatusReason? JobOfferStatusReason { get; set; }

        /// <summary>Gets or sets the job offer publish date.</summary>
        [DataMember(Name = "jobOfferPublishDate", IsRequired = false)]
        public DateTime? JobOfferPublishDate { get; set; }
    }
}