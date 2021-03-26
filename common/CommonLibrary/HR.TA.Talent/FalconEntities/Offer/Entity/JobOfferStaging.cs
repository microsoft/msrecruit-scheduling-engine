//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Offer
{
    using System;
    using System.Runtime.Serialization;

    using HR.TA..Common.DocumentDB.Contracts;
    using System.Collections.Generic;
    using HR.TA..Common.Provisioning.Entities.FalconEntities.Attract;

    [DataContract]
    public class JobOfferStaging : DocDbEntity
    {
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
        /// Gets or sets Job Application Id
        /// </summary>
        [DataMember(Name = "jobApplicationId")]
        public string JobApplicationId { get; set; }

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
        /// Gets or sets Offer Participants
        /// </summary>
        [DataMember(Name = "offerParticipant")]
        public List<JobApplicationActivityParticipant> OfferParticipants { get; set; }

    }
}