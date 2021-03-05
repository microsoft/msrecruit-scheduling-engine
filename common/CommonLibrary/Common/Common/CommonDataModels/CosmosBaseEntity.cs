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
    /// Holds CosmosBaseEntity
    /// </summary>
    public class CosmosBaseEntity : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        [ExcludeFromResponse]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "for Cosmos")]
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the partition key of Document.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [ExcludeFromResponse]
        [JsonProperty("isDeleted")]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets CreatedBy.
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
        /// Gets or sets ODSCreatedBy.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("odsCreatedBy")]
        public string ODSCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets ODSCreatedOn.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("odsCreatedOn")]
        public DateTime? ODSCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets ODSUpdatedBy.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("odsUpdatedBy")]
        public string ODSUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets ODSUpdatedOn.
        /// </summary>
        [ExcludeFromResponse]
        [JsonProperty("odsUpdatedOn")]
        public DateTime? ODSUpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the e tag.
        /// </summary>
        [JsonProperty(PropertyName = "_etag")]
        [ExcludeFromResponse]
        public string ETag { get; set; }
    }
}
