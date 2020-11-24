//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SecretManagerProvider.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.KeyVault
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using Microsoft.Extensions.Logging;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Azure.Security;
    using ServicePlatform.Configuration;
    using ServicePlatform.Context;
    using ServicePlatform.Tracing;

    /// <summary>
    /// The secret manager provider
    /// </summary>
    public class SecretManagerProvider : ISecretManagerProvider
    {
        /// <summary>The logger</summary>
        private readonly ILogger<SecretManagerProvider> logger;

        /// <summary> Azure active directory client</summary>
        private readonly IAzureActiveDirectoryClient azureActiveDirectoryClient;

        /// <summary> Dictionary holding secret manager instances</summary>
        private ConcurrentDictionary<string, ISecretManager> secretManagerInstances;

        public SecretManagerProvider(
            IAzureActiveDirectoryClient azureActiveDirectoryClient,
            ILogger<SecretManagerProvider> logger)
        {
            Contract.CheckValue(azureActiveDirectoryClient, nameof(azureActiveDirectoryClient));
            Contract.CheckValue(logger, nameof(logger));

            this.azureActiveDirectoryClient = azureActiveDirectoryClient;
            this.logger = logger;
            this.secretManagerInstances = new ConcurrentDictionary<string, ISecretManager>();
        }

        /// <summary>
        /// Gets or creates a valid Secret Manager instance.
        /// </summary>
        /// <param name="keyVaultUri">The URI for the key vault resource managed by the secret manager</param>
        /// <returns>The <see cref="ISecretManager"/></returns>
        public ISecretManager GetOrCreateSecretManager(string keyVaultUri)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri));

            return this.logger.Execute(
                "GetOrCreateSecretManager",
                () => {
                    this.logger.LogInformation($"Trying to get or create a new secret manager using key: {keyVaultUri}");

                    var secretManager = this.secretManagerInstances.GetOrAdd(keyVaultUri, (key) =>
                    {
                        this.logger.LogInformation("Trying to create a new secret manager instance");
                        var newInstance = this.CreateManagerInstance(keyVaultUri);
                        this.logger.LogInformation($"Attempt to add or overwrite new secret manager completed");
                        return newInstance;
                    });
                    
                    this.logger.LogInformation("Secret manager instance found. Returning cached instance");

                    return secretManager;
                });
        }

        public async Task<string> GetVaultSecret(string keyVaultUri, string secretName)
        {
            var secretManager = GetOrCreateSecretManager(keyVaultUri);
            var secretValueResult = await secretManager.TryGetSecretAsync(secretName);

            this.logger.LogInformation($"Trying to get secret key for secret Name - {0}", secretName);
            if (secretValueResult.Succeeded)
            {
                this.logger.LogInformation($"Retrieved secret successfully for secret Name - {0}", secretName);
                return secretValueResult.Result.Value;
            }
            else
            {
                this.logger.LogInformation($"Unable to retrieve secret key for secret Name - {0}", secretName);
                return null;
            }
        }

        private static object GetOrCreateSecretManager()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a secret manager instance
        /// </summary>
        /// <param name="keyVaultUri">The URI of the key vault</param>
        /// <returns>The <see cref="SecretManager"/></returns>
        private SecretManager CreateManagerInstance(string keyVaultUri)
        {
            Contract.CheckNonEmpty(keyVaultUri, nameof(keyVaultUri));

            var secretManager = new SecretManager(this.azureActiveDirectoryClient, new KeyVaultConfiguration
            {
                KeyVaultUri = keyVaultUri
            });

            this.logger.LogInformation($"Successfully created secret manager instance with key : {keyVaultUri}");
            return secretManager;
        }
    }
}
