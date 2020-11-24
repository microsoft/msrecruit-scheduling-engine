//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="StorageConfigurationSettings.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Constants
{
    /// <summary>Storage configuration settings and constants.</summary>
    public sealed class StorageConfigurationSettings
    {
        /// <summary>The HCM global document DB storage configuration name</summary>
        public const string HCMGlobalDocumentDbStorageConfigName = "HCMGlobalDocumentDb";

        /// <summary>The HCM regional document DB storage configuration name</summary>
        public const string HCMRegionalDocumentDbStorageConfigName = "HCMRegionalDocumentDb";

        /// <summary>The HCM regional sql DB storage configuration name</summary>
        public const string HCMRegionalSqlDbStorageConfigName = "HCMRegionalSqlDb";

        /// <summary>The HCM regional REDIS storage configuration name</summary>
        public const string HCMRegionalRedisCacheStorageConfigName = "HCMRegionalRedisCache";

        /// <summary>The HCM regional Blob storage configuration name</summary>
        public const string HCMPrimaryBlobStorageConfigName = "BlobPrimaryConnectionString";

        /// <summary>The HCM regional blob storage configuration name</summary>
        public const string HCMSecondaryBlobStorageConfigName = "BlobSecondaryConnectionString";

        /// <summary>The HCM regional storage configuration name</summary>
        public const string HCMStorageAccountPrimaryConnectionStringConfigName = "HCMRegionalStorageAccountPrimaryConnectionString";

        /// <summary>The HCM regional storage configuration name</summary>
        public const string HCMStorageAccountSecondaryConnectionStringConfigName = "HCMRegionalStorageAccountSecondaryConnectionString";

        /// <summary>The HCM global document DB key vault secret name</summary>
        public const string HCMGlobalDocumentDbSecretName = "GlobalDocumentDbPrimaryConnectionString";

        /// <summary>The HCM regional document DB key vault secret name</summary>
        public const string HCMRegionalDocumentDbSecretName = "DocumentDbPrimaryConnectionString";

        /// <summary>The HCM regional sql DB key vault secret name</summary>
        public const string HCMRegionalSqlDbSecretName = "sqlDbConnectionString";

        /// <summary>The HCM regional REDIS key vault secret name</summary>
        public const string HCMRegionalRedisSecretName = "RedisCachePrimaryKey";

        /// <summary>The HCM regional Blob primary key vault secret name</summary>
        public const string HCMRegionalBlobPrimarySecretName = "AppTeamStoragePrimaryConnectionString";

        /// <summary>The HCM regional Blob secondary key vault secret name</summary>
        public const string HCMRegionalBlobSecondarySecretName = "AppTeamStorageSecondaryConnectionString";

        /// <summary>The HCM regional storage account primary key vault secret name</summary>
        public const string HCMRegionalStorageAccountPrimarySecretName = "StorageAccountPrimaryConnectionString";

        /// <summary>The HCM regional storage account secondary key vault secret name</summary>
        public const string HCMRegionalStorageAccountSecondarySecretName = "StorageAccountSecondaryConnectionString";

        /// <summary>The number of documents returned from document db.</summary>
        public const int DocumentDbNumberOfDocumentsReturned = 10;

        /// <summary>The document db response units per second.</summary>
        public const int DocumentDbResponseUnitsPerSecond = 400;

        /// <summary>The user settings database id.</summary>
        public const string UserSettingsDatabaseId = "UserSettingsDatabase";

        /// <summary>The HCM database id.</summary>
        public const string HCMDatabaseId = "HCMDatabase";

        /// <summary>The Blob storage Container name.</summary>
        public const string UserDirectoryBlobStorageContainerName = "userdirectory";

        /// <summary>The HCM database id.</summary>
        public const string GlobalRoutingDatabaseId = "RoutingDatabase";

        /// <summary> The document DB key for the HCM cluster to BAP location mappings information</summary>
        public const string HCMClusterToBAPLocationMappingsDocumentDbKey = "hcmClusterToBapLocationMappings";

        /// <summary>The HCM fall back BAP location: unitedstates.</summary>
        public const string HCMDefaultBAPLocation = "unitedstates";
    }
}
