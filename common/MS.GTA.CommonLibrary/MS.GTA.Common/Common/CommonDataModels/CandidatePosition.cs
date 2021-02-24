//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.GTA.Common.CommonDataModels
{
    /// <summary>
    /// Holds candidate position
    /// </summary>
    public class CandidatePosition : CosmosBaseSubEntity
    {
        /// <summary>
        /// Gets or sets CandidateId.
        /// </summary>
        [JsonProperty("candidateId")]
        public int CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the position number.
        /// </summary>
        /// <value>
        /// The position number.
        /// </value>
        [JsonProperty("positionNumber")]
        public int? PositionNumber { get; set; }
    }
}
