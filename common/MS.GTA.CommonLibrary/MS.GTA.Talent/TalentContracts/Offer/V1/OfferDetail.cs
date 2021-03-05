//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Offer Information
    /// </summary>
    [DataContract]
    public class OfferDetail
    {
        /// <summary>
        /// Gets or sets Offer Id
        /// </summary>
        [DataMember(Name = "offerId")]
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets candidate Id
        /// </summary>
        [DataMember(Name = "candidateId")]
        public string CandidateId { get; set; }

        /// <summary>
        /// Gets or sets candidate Name
        /// </summary>
        [DataMember(Name = "candidateName")]
        public string CandidateName { get; set; }

        /// <summary>
        /// Gets or sets candidate Email
        /// </summary>
        [DataMember(Name = "candidateEmail")]
        public string CandidateEmail { get; set; }

        /// <summary>
        /// Gets or sets JobOpening Id
        /// </summary>
        [DataMember(Name = "jobOpeningId")]
        public string JobOpeningId { get; set; }

        /// <summary>
        /// Gets or sets jobOpening title
        /// </summary>
        [DataMember(Name = "jobOpeningTitle")]
        public string JobOpeningTitle { get; set; }

        /// <summary>
        /// Gets or sets Job Application Id
        /// </summary>
        [DataMember(Name = "jobApplicationId")]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Offer Status
        /// </summary>
        [DataMember(Name = "offerStatus")]
        public OfferStatusReason OfferStatus { get; set; }

        /// <summary>
        /// Gets or sets Hiring Manager
        /// </summary>
        [DataMember(Name = "hiringManager")]
        public string HiringManager { get; set; }

        /// <summary>
        /// Gets or sets Created Date Time.
        /// </summary>
        [DataMember(Name = "createdDateTimeUTC", IsRequired = false)]
        public DateTime CreatedDateTimeUTC { get; set; }

        /// <summary>
        /// Gets or sets Modified Date Time.
        /// </summary>
        [DataMember(Name = "modifiedDateTimeUTC", IsRequired = false)]
        public DateTime ModifiedDateTimeUTC { get; set; }

        /// <summary>
        /// Gets or sets Participant Role.
        /// </summary>
        [DataMember(Name = "role", IsRequired = false)]
        public OfferParticipantRole Role { get; set; }

        /// <summary>
        /// Gets or sets External Job Application Id.
        /// </summary>
        [DataMember(Name = "externalJobApplicationID", IsRequired = false)]
        public string ExternalJobApplicationID { get; set; }

        /// <summary>
        /// Gets or sets External Job Opening Id.
        /// </summary>
        [DataMember(Name = "externalJobOpeningID", IsRequired = false)]
        public string ExternalJobOpeningID { get; set; }
    }
}
