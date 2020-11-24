//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="DocumentDBStorageConfiguration.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Configuration;

namespace MS.GTA.Common.DocumentDB.Configuration
{
    /// <summary>
    /// Document database storage configuration settings class
    /// </summary>
    [SettingsSection("DocumentDBStorageConfiguration")]
    public class DocumentDBStorageConfiguration
    {
        /// <summary>
        /// Gets or sets the connection string secret name in the regional or global key vault
        /// "AccountEndpoint=documentDBUri;AccountKey=key;"
        /// </summary>
        public string ConnectionStringSecretName { get; set; }

        /// <summary>
        /// Gets or sets the document DB key vault URI
        /// </summary>
        public string KeyVaultUri { get; set; }

        /// <summary>
        /// Gets or sets the database id
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Gets or sets the number of documents returned from the document database
        /// </summary>
        public int NumberOfDocumentsReturned { get; set; }

        /// <summary>
        /// Gets or sets the overall throughput of the document database in response units per second
        /// </summary>
        public int ResponseUnitsPerSecond { get; set; }
    }
}
