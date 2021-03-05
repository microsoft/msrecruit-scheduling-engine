//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CommonDataModels
{
    /// <summary>
    /// Holds candidate source
    /// </summary>
    public class CandidateSource : CosmosBaseSubEntity
    {
        /// <summary>
        /// Gets or sets CandidateId.
        /// </summary>
        [JsonProperty("candidateId")]
        public int CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the source channel.
        /// </summary>
        /// <value>
        /// The source channel.
        /// </value>
        [JsonProperty("sourceChannel")]
        public string SourceChannel { get; set; }

        /// <summary>
        /// Gets or sets the source details.
        /// </summary>
        /// <value>
        /// The source details.
        /// </value>
        [JsonProperty("sourceDetails")]
        public string SourceDetails { get; set; }

        /// <summary>
        /// Gets or sets the source email.
        /// </summary>
        /// <value>
        /// The source email.
        /// </value>
        [JsonProperty("sourceEmail")]
        public string SourceEmail { get; set; }

        /// <summary>
        /// Gets or sets the source person.
        /// </summary>
        /// <value>
        /// The source person.
        /// </value>
        [JsonProperty("sourcePerson")]
        public string SourcePerson { get; set; }

        /// <summary>
        /// Gets or sets the source specifics.
        /// </summary>
        /// <value>
        /// The source specifics.
        /// </value>
        [JsonProperty("sourceSpecifics")]
        public string SourceSpecifics { get; set; }
    }
}
