//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2.Gdpr
{
    using System.Collections.Generic;
    using TA.CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// Contract for Job Application Details.
    /// </summary>
    public class JobApplicationDetail
    {
        /// <summary>
        /// Gets or sets Application Id.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Job Oepning Id.
        /// </summary>
        public string JobOpeningId { get; set; }

        /// <summary>
        /// Gets or sets Job Opening Title.
        /// </summary>
        public string JobOpeningTitle { get; set; }

        /// <summary>
        /// Gets or sets Hiring Team Role.
        /// </summary>
        public JobParticipantRole? Role { get; set; }

        /// <summary>
        /// Gets or sets the Offer Notes for the Job Application
        /// </summary>
        public List<string> OfferNotes { get; set; }

        /// <summary>
        /// Gets or sets the OfferDetails for the Job Application
        /// </summary>
        public List<JobOfferDetail> JobOfferDetails { get; set; }

        /// <summary>
        /// Gets or sets the Candidate for the Job Application
        /// </summary>
        public CandidateDetail CandidateDetail { get; set; }
    }
}
