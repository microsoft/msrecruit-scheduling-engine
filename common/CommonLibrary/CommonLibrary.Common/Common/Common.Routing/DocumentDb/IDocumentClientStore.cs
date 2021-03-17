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

    /// <summary>The DocumentClientStore interface.</summary>
    public interface IDocumentClientStore
    {
        /// <summary>Gets the document DB connections.</summary>
        ConcurrentDictionary<string, IDocumentClient> DocumentDBConnections { get; }

        /// <summary>Gets the memory cache.</summary>
        IMemoryCache MemoryCache { get; }
    }
}
