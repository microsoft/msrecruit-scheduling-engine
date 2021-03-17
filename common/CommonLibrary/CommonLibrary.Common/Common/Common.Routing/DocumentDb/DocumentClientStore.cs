//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Routing.DocumentDb
{
    using System.Collections.Concurrent;

    using Microsoft.Azure.Documents.Client;

    using Microsoft.Azure.Documents;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// The document client store. This is to get around the fact that our generator cannot be singleton.
    /// This store is the only reason we need that so it's ok to store it here instead.
    /// </summary>
    public class DocumentClientStore : IDocumentClientStore
    {
        /// <summary>Initializes a new instance of the <see cref="DocumentClientStore"/> class.</summary>
        /// <param name="memoryCache">The memory cache.</param>
        public DocumentClientStore(IMemoryCache memoryCache)
        {
            this.MemoryCache = memoryCache;
        }

        /// <summary>Gets or sets the memory cache.</summary>
        public IMemoryCache MemoryCache { get; set; }

        /// <summary>Gets the document DB connections.</summary>
        public ConcurrentDictionary<string, IDocumentClient> DocumentDBConnections { get; } = 
            new ConcurrentDictionary<string, IDocumentClient>();
    }
}
