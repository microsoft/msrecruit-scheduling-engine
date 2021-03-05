//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.CommonDataModels
{
    /// <summary>
    /// Holds Candidate 
    /// </summary>
    public class Candidate : CosmosBaseEntity
    {
        /// <summary>
        /// Gets or sets CandidateId.
        /// </summary>
        [Key]
        [JsonProperty("candidateId")]
        public int CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the candidate.
        /// </summary>
        /// <value>
        /// The first name of the candidate.
        /// </value>
        [JsonProperty("candidateFirstName")]
        public string CandidateFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the candidate.
        /// </summary>
        /// <value>
        /// The last name of the candidate.
        /// </value>
        [JsonProperty("candidateLastName")]
        public string CandidateLastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the candidate middle.
        /// </summary>
        /// <value>
        /// The name of the candidate middle.
        /// </value>
        [JsonProperty("candidateMiddleName")]
        public string CandidateMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the first name of the candidate preferred.
        /// </summary>
        /// <value>
        /// The first name of the candidate preferred.
        /// </value>
        [JsonProperty("candidatePreferredFirstName")]
        public string CandidatePreferredFirstName { get; set; }

        /// <summary>
        /// Gets or sets the candidate email.
        /// </summary>
        /// <value>
        /// The candidate email.
        /// </value>
        [JsonProperty("candidateEmail")]
        public string CandidateEmail { get; set; }

        /// <summary>
        /// Gets or sets the custom properties.
        /// </summary>
        /// <value>
        /// The custom properties.
        /// </value>
        [JsonProperty("customProperties")]
        public IEnumerable<CustomProperty> CustomProperties { get; set; }

        /// <summary>
        /// Gets or sets the candidate source.
        /// </summary>
        /// <value>
        /// The candidate source.
        /// </value>
        [JsonProperty("candidateSource")]
        public CandidateSource CandidateSource { get; set; }

        /// <summary>
        /// Gets or sets the candidate position.
        /// </summary>
        /// <value>
        /// The candidate position.
        [JsonProperty("candidatePosition")]
        public CandidatePosition CandidatePosition { get; set; }

        /// <summary>
        /// Gets or sets the candidate referral.
        /// </summary>
        /// <value>
        /// The candidate referral.
        /// </value>
        [JsonProperty("candidateReferral")]
        public CandidateReferral CandidateReferral { get; set; }

        /// <summary>
        /// Gets or sets the candidate regulatory consent.
        /// </summary>
        /// <value>
        /// The candidate regulatory consent.
        /// </value>
        [JsonProperty("candidateRegulatoryConsent")]
        public CandidateRegulatoryConsent CandidateRegulatoryConsent { get; set; }
    }
}
