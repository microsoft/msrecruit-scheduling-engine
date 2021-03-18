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
    /// Holds CandidateRegulatoryConsent
    /// </summary>
    public class CandidateRegulatoryConsent : CosmosBaseSubEntity
    {
        /// <summary>
        /// Gets or sets CandidateId.
        /// </summary>
        [JsonProperty("candidateId")]
        public int CandidateId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the is consent received.
        /// </summary>
        /// <value>
        /// The is consent received.
        /// </value>
        [JsonProperty("isConsentReceived")]
        public string IsConsentReceived { get; set; }

        /// <summary>
        /// Gets or sets the regulatory country.
        /// </summary>
        /// <value>
        /// The regulatory country.
        /// </value>
        [JsonProperty("regulatoryCountry")]
        public Lookup RegulatoryCountry { get; set; }

        /// <summary>
        /// Gets or sets the consent received date time.
        /// </summary>
        /// <value>
        /// The consent received date time.
        /// </value>
        [JsonProperty("consentReceivedDate")]
        public DateTime? ConsentReceivedDate { get; set; }

        /// <summary>
        /// Gets or sets the regulatory audit date time.
        /// </summary>
        /// <value>
        /// The regulatory audit date time.
        /// </value>
        [JsonProperty("regulatoryAuditDateTime")]
        public DateTime? RegulatoryAuditDateTime { get; set; }
    }
}
