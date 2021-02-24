//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Constants
{
    using Base;
    using CommonDataService.Common.Internal;
    using Configuration;
    using Contracts;
    using Exceptions;
    using ServicePlatform.Exceptions;
    using ServicePlatform.GlobalService.Contracts.Client;

    /// <summary> Global service environment settings and constants </summary>
    public static class GlobalServiceEnvironmentSettings
    {
        /// <summary>
        /// Gets the global service environment configuration based on the environment
        /// </summary>
        /// <param name="environmentName">The environment to use</param>
        /// <returns>The <see cref="GlobalServiceConfiguration"/></returns>
        public static GlobalServiceConfiguration GetGlobalServiceConfiguration(string environmentName)
        {
            Contract.CheckNonEmpty(environmentName, nameof(environmentName));

            var upperCasedEnvironmentName = environmentName.ToUpper();
            switch (upperCasedEnvironmentName)
            {
                case Constants.DevEnvironmentName:
                    return GetDevGlobalServiceConfiguration();
                case Constants.IntEnvironmentName:
                    return GetIntGlobalServiceConfiguration();
                case Constants.ProdEnvironmentName:
                    return GetProdGlobalServiceConfiguration();
                default:
                    throw new GlobalServiceInvalidOperationException($"GlobalServiceEnvironmentSettings.GetGlobalServiceConfiguration: Unknown environmentName - {environmentName}").EnsureTraced();
            }
        }

        /// <summary>
        /// Gets the global document db storage configuration for the matching environment
        /// </summary>
        /// <param name="environmentName">The environment to use</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        public static StorageConfiguration GetGlobalDocumentDbStorageConfiguration(string environmentName)
        {
            Contract.CheckNonEmpty(environmentName, nameof(environmentName));

            var keyVaultUri = GetKeyVaultUri(environmentName);
            return CreateGlobalDocumentDbStorageConfiguration(keyVaultUri);
        }

        /// <summary>
        /// Gets the DEV global service environment configuration
        /// </summary>
        /// <returns>The <see cref="GlobalServiceConfiguration"/></returns>
        private static GlobalServiceConfiguration GetDevGlobalServiceConfiguration()
        {
            return new GlobalServiceConfiguration()
            {
                AADTenantId = Constants.MicrosoftTenantId,
                ClusterUri = "https://d365-atm-hcm-dev.rsu.int.powerapps.com/GlobalService/GlobalService",
                HCMAppAuthorityId = "66832c40-f4dc-426a-8b91-d70cb1e587d0",
                Resource = "http://globalservice.test.dynamics365.com"
            };
        }

        /// <summary>
        /// Gets the INT global service environment configuration
        /// </summary>
        /// <returns>The <see cref="GlobalServiceConfiguration"/></returns>
        private static GlobalServiceConfiguration GetIntGlobalServiceConfiguration()
        {
            return new GlobalServiceConfiguration()
            {
                AADTenantId = Constants.MicrosoftTenantId,
                ClusterUri = "https://d365-atm-hcm-int.rsu.int.powerapps.com/GlobalService/GlobalService",
                HCMAppAuthorityId = "66832c40-f4dc-426a-8b91-d70cb1e587d0",
                Resource = "http://globalservice.dynamics365.com"
            };
        }

        /// <summary>
        /// Gets the PROD global service environment configuration
        /// </summary>
        /// <returns>The <see cref="GlobalServiceConfiguration"/></returns>
        private static GlobalServiceConfiguration GetProdGlobalServiceConfiguration()
        {
            return new GlobalServiceConfiguration()
            {
                AADTenantId = Constants.MicrosoftTenantId,
                ClusterUri = "https://d365-atm-hcm-prod.rsu.powerapps.com/GlobalService/GlobalService",
                HCMAppAuthorityId = "5712d3fd-8e22-4040-afbf-70fa18f63627",
                Resource = "http://globalservice.dynamics365.com"
            };
        }

        /// <summary>
        /// Gets the global document DB storage configuration
        /// </summary>
        /// <param name="keyVaultUri">The key vault uri</param>
        /// <returns>The <see cref="StorageConfiguration"/></returns>
        private static StorageConfiguration CreateGlobalDocumentDbStorageConfiguration(string keyVaultUri)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri));

            return new StorageConfiguration()
            {
                Name = StorageConfigurationSettings.HCMGlobalDocumentDbStorageConfigName,
                Type = StorageConfigurationResourceType.DocumentDb,
                ConnectionDetails = new ConnectionDetails()
                {
                    KeyVaultName = keyVaultUri,
                    Type = ConnectionDetailsType.AzureKeyVault,
                    KeyVaultSecretName = StorageConfigurationSettings.HCMGlobalDocumentDbSecretName,
                }
            };
        }

        /// <summary>
        /// Gets the key vault uri for the associated environment
        /// </summary>
        /// <param name="environmentName">The environment name</param>
        /// <returns>The key vault uri</returns>
        private static string GetKeyVaultUri(string environmentName)
        {
            Contract.CheckNonEmpty(environmentName, nameof(environmentName));

            var upperCasedEnvironmentName = environmentName.ToUpper();
            switch (upperCasedEnvironmentName)
            {
                case Constants.LocalEnvironmentName:
                case Constants.DevEnvironmentName:
                    return Constants.DevMasterKeyVaultUri;
                case Constants.IntEnvironmentName:
                    return Constants.IntMasterKeyVaultUri;
                case Constants.ProdEnvironmentName:
                    return Constants.ProdMasterKeyVaultUri;
                default:
                    throw new GlobalServiceInvalidOperationException($"GlobalServiceEnvironmentSettings.GetKeyVaultUri: Unknown environmentName - {environmentName}").EnsureTraced();
            }
        }
    }
}
