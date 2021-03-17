//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLibrary.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// The Paged list class.
    /// </summary>
    /// <typeparam name="T">The type that is paged.</typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the structure that contains extra data.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> ExtensionData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        public PagedList()
        {
            ExtensionData = new Dictionary<string, JToken>();
        }
    }
}
