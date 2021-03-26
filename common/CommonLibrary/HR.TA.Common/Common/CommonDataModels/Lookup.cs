//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.TA.Common.CommonDataModels
{
    /// <summary>
    /// Taxonomy for metadata.
    /// </summary>
    public class Lookup
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        [ExcludeFromResponse]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value
        {
            get { return this.Name; }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        [ExcludeFromResponse]
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the type of the taxonomy.
        /// </summary>
        /// <value>
        /// The type of the taxonomy.
        /// </value>
        [ExcludeFromResponse]
        [JsonProperty("taxonomy")]
        public string TaxonomyType { get; set; }
    }
}
