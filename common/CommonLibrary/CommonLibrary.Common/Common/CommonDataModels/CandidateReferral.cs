//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Common.CommonDataModels
{
    /// <summary>
    /// Holds Candidate Referral
    /// </summary>
    public class CandidateReferral : CosmosBaseSubEntity
    {
        /// <summary>
        /// Gets or sets CandidateId.
        /// </summary>
        [JsonProperty("candidateId")]
        public int CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the referring employee.
        /// </summary>
        /// <value>
        /// The referring employee.
        /// </value>
        [JsonProperty("referringEmployeeProfileId")]
        public string ReferringEmployeeProfileId { get; set; }

        /// <summary>
        /// Gets or sets the submission date.
        /// </summary>
        /// <value>
        /// The submission date.
        /// </value>
        [JsonProperty("submissionDate")]
        public DateTime? SubmissionDate { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the type of the relation.
        /// </summary>
        /// <value>
        /// The type of the relation.
        /// </value>
        [JsonProperty("relationType")]
        public Lookup RelationType { get; set; }

        /// <summary>
        /// Gets or sets the professional endorsement.
        /// </summary>
        /// <value>
        /// The professional endorsement.
        /// </value>
        [JsonProperty("professionalEndorsement")]
        public Lookup ProfessionalEndorsement { get; set; }

        /// <summary>
        /// Gets or sets the type of the job profile.
        /// </summary>
        /// <value>
        /// The type of the job profile.
        /// </value>
        [JsonProperty("jobProfileType")]
        public Lookup JobProfileType { get; set; }

        /// <summary>
        /// Gets or sets the current student indicator.
        /// </summary>
        /// <value>
        /// The current student indicator.
        /// </value>
        [JsonProperty("currentStudentIndicator")]
        public Lookup CurrentStudentIndicator { get; set; }

        /// <summary>
        /// Gets or sets the campaign code.
        /// </summary>
        /// <value>
        /// The campaign code.
        /// </value>
        [JsonProperty("campaignCode")]
        public string CampaignCode { get; set; }

        /// <summary>
        /// Gets or sets the recommendation text.
        /// </summary>
        /// <value>
        /// The recommendation text.
        /// </value>
        [JsonProperty("recommendationText")]
        public string RecommendationText { get; set; }

        /// <summary>
        /// Gets or sets the job information text.
        /// </summary>
        /// <value>
        /// The job information text.
        /// </value>
        [JsonProperty("jobInformationText")]
        public string JobInformationText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is referring candidate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is referring candidate; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("isReferringCandidate")]
        public Lookup IsReferringCandidate { get; set; }
    }
}
