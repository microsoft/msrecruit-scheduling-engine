//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Routing.SqlDB.Configuration 
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// Sql database configuration settings class
    /// </summary>
    [SettingsSection(nameof(SqlDbConfiguration))]
    public class SqlDbConfiguration
    {
        /// <summary>
        /// Gets or sets the connection string secret name in the regional key vault
        /// </summary>
        public string ConnectionStringSecretName { get; set; }

        /// <summary>
        /// Gets or sets the sql DB key vault URI
        /// </summary>
        public string KeyVaultUri { get; set; }
    }
}
