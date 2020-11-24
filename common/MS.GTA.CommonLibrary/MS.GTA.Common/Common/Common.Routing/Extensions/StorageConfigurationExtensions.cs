// //----------------------------------------------------------------------------
// // <copyright company="Microsoft Corporation" file="StorageConfigurationExtensions.cs">
// // Copyright (c) Microsoft Corporation. All rights reserved.
// // </copyright>
// //----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Extensions
{
    using CommonDataService.Common.Internal;
    using Constants;
    using DocumentDB.Configuration;
    using Exceptions;
    using MS.GTA.Common.BlobStore.Configuration;
    using ServicePlatform.GlobalService.Contracts.Client;
    using SqlDB.Configuration;
    using StorageAccount.Configuration;

    /// <summary> Storage configurations extensions class</summary>
    public static class StorageConfigurationExtensions
    {
        public static DocumentDBStorageConfiguration ToGlobalRoutingDocumentDBStorageConfiguration(this StorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            return ToDocumentDBStorageConfiguration(storageConfiguration, StorageConfigurationSettings.GlobalRoutingDatabaseId);
        }

        /// <summary>Converts a global service storage configuration to a HCM database document DB storage configuration.</summary>
        /// <param name="storageConfiguration">The global service storage configuration.</param>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        public static DocumentDBStorageConfiguration ToHCMDocumentDBStorageConfiguration(this StorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            return ToDocumentDBStorageConfiguration(storageConfiguration, StorageConfigurationSettings.HCMDatabaseId);
        }

        /// <summary>Converts a global service storage configuration to a user settings document DB storage configuration.</summary>
        /// <param name="storageConfiguration">The global service storage configuration.</param>
        /// <returns>The <see cref="DocumentDBStorageConfiguration"/></returns>
        public static DocumentDBStorageConfiguration ToUserSettingsDocumentDBStorageConfiguration(this StorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            return ToDocumentDBStorageConfiguration(storageConfiguration, StorageConfigurationSettings.UserSettingsDatabaseId);
        }

        public static DocumentDBStorageConfiguration ToDocumentDBStorageConfiguration(
            this StorageConfiguration storageConfiguration,
            string databaseId,
            int numberOfDocumentsReturned = 0,
            int responseUnitsPerSecond = 0)
        {
            Contract.CheckNonEmpty(databaseId, nameof(databaseId));

            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            var connectionDetails = storageConfiguration.ConnectionDetails;
            var documentsReturned = numberOfDocumentsReturned == 0 ?
                StorageConfigurationSettings.DocumentDbNumberOfDocumentsReturned : numberOfDocumentsReturned;
            var documentResponseUnitsPerSecond = responseUnitsPerSecond == 0 ?
                StorageConfigurationSettings.DocumentDbResponseUnitsPerSecond : responseUnitsPerSecond;

            return new DocumentDBStorageConfiguration
            {
                ConnectionStringSecretName = connectionDetails.KeyVaultSecretName,
                KeyVaultUri = connectionDetails.KeyVaultName,
                DatabaseId = databaseId,
                NumberOfDocumentsReturned = documentsReturned,
                ResponseUnitsPerSecond = documentResponseUnitsPerSecond
            };
        }

        /// <summary>Converts a global service storage configuration to a Blob storage configuration.</summary>
        /// <param name="storageConfiguration">The global service storage configuration.</param>
        /// <param name="blobContainerName">the blob storage container name</param>
        /// <returns>The <see cref="BlobStoreSettings"/></returns>
        public static BlobStoreSettings ToBlobStorageConfiguration(
            this StorageConfiguration storageConfiguration,
            string blobContainerName)
        {
            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            Contract.CheckValue(blobContainerName, nameof(blobContainerName));

            var connectionDetails = storageConfiguration.ConnectionDetails;
            return new BlobStoreSettings
            {
                KeyVaultSecretNameForPrimaryConnectionString = connectionDetails.KeyVaultSecretName,
                BlobKeyVaultUri = connectionDetails.KeyVaultName,
                BlobStoreContainerName = blobContainerName
            };
        }

        /// <summary>Converts a global service storage configuration to a Sql Db configuration.</summary>
        /// <param name="storageConfiguration">The global service storage configuration.</param>
        /// <returns>The <see cref="SqlDbConfiguration"/></returns>
        public static SqlDbConfiguration ToSqlDbStorageConfiguration(
            this StorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            var connectionDetails = storageConfiguration.ConnectionDetails;
            return new SqlDbConfiguration
            {
                ConnectionStringSecretName = connectionDetails.KeyVaultSecretName,
                KeyVaultUri = connectionDetails.KeyVaultName,
            };
        }

        /// <summary>Converts a global service storage configuration to a storage account configuration.</summary>
        /// <param name="storageConfiguration">The global service storage configuration.</param>
        /// <returns>The <see cref="StorageAccountSettings"/></returns>
        public static StorageAccountConfiguration ToStorageAccountStorageConfiguration(
            this StorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
            {
                throw new GlobalServiceInvalidOperationException($"Storage configuration is null; please verify that the caller has been pinned to a cluster");
            }

            var connectionDetails = storageConfiguration.ConnectionDetails;
            return new StorageAccountConfiguration
            {
                KeyVaultSecretNameForPrimaryConnectionString = connectionDetails.KeyVaultSecretName,
                StorageAccountKeyVaultUri = connectionDetails.KeyVaultName,
            };
        }
    }
}
