//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.DocumentDB.V2
{
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents.Client;
    using Configuration;

    /// <summary>
    /// The document database interface.
    /// </summary>
    public interface IDocumentDB
    {
        /// <summary>
        /// Get document client based on configuration
        /// </summary>
        /// <param name="documentDbConfigurations">DB configurations</param>
        /// <returns>Document Client</returns>
        Task<DocumentClient> GetDocumentClient(DocumentDBClientConfiguration documentDbConfigurations);

        /// <summary>
        /// Get document client based on configuration
        /// </summary>
        /// <param name="documentDbStorageConfiguration">DB configurations</param>
        /// <returns>Document Client</returns>
        Task<DocumentClient> GetDocumentClient(DocumentDBStorageConfiguration documentDbStorageConfiguration);
    }
}
