//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.DocumentDB.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// Document database client configuration settings class
    /// </summary>
    [SettingsSection("DocumentDbClientConfiguration")]
    public class DocumentDBClientConfiguration
    {
        /// <summary>
        /// Gets or sets the document database uri
        /// </summary>
        public string DatabaseUri { get; set; }

        /// <summary>
        /// Gets or sets the document secret name in the regional or global key vault
        /// </summary>
        public string DatabaseSecretName { get; set; }

        /// <summary>
        /// Gets or sets the document DB key vault URI
        /// </summary>
        public string DatabaseKeyVaultUri { get; set; }

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
