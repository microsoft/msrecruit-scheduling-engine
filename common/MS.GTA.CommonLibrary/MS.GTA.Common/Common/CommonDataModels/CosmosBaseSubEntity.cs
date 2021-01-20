using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.GTA.Common.CommonDataModels
{
    /// <summary>
    /// Stores the Generic Schema for all Cosmos DB Entities.
    /// </summary>
    public class CosmosBaseSubEntity
    {
        /// <summary>
        /// Gets or sets SourceCreatedBy.
        /// </summary>
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets SourceCreatedOn.
        /// </summary>
        [JsonProperty("createdOn")]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets ModifiedBy.
        /// </summary>
        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets ModifiedOn.
        /// </summary>
        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }
    }
}
