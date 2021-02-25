//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Logging;
using MS.GTA.Common.Base.Configuration;
using MS.GTA.Common.Base.Utilities;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Azure.AAD;
using MS.GTA.ServicePlatform.Azure.Security.Activities;
using MS.GTA.ServicePlatform.Configuration;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Tracing;
using MS.GTA.ServicePlatform.Utils;
using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.Azure.Security
{
    public sealed class SecretManager : ISecretManager
    {
        private const int DefaultRetryCount = 3;
        private const int MinRetryCount = 1;
        private const int MaxRetryCount = 10;
        private const string SecretContentType = "application/octet-stream";
        private const string SecretNotFoundErrorCode = "SecretNotFound";
        private static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan MinRetryInterval = TimeSpan.Zero;
        private static readonly TimeSpan MaxRetryInterval = TimeSpan.FromMilliseconds(10000);
        private readonly string keyVaultUri;
        private static IKeyVaultClient keyVaultClient = null;
        private static ConcurrentDictionary<string, SecretBundle> DictionaryCache = new ConcurrentDictionary<string, SecretBundle>();
        private readonly IRetryPolicy retryPolicy;

        public SecretManager(IAzureActiveDirectoryClient azureActiveDirectoryClient, IConfigurationManager configurationManager)
            : this(azureActiveDirectoryClient, configurationManager.Get<KeyVaultConfiguration>())
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
        }

        public SecretManager(IAzureActiveDirectoryClient azureActiveDirectoryClient, KeyVaultConfiguration secretManagerConfiguration)
            : this(async (string authority, string resource, string scope) => (await azureActiveDirectoryClient.GetAppOnlyAccessTokenAsync(resource)).AccessToken, secretManagerConfiguration)
        {
            Contract.CheckValue(azureActiveDirectoryClient, nameof(azureActiveDirectoryClient));
        }

        public SecretManager(KeyVaultClient.AuthenticationCallback getAccessTokenAsync, KeyVaultConfiguration secretManagerConfiguration)
        {
            Contract.CheckValue(getAccessTokenAsync, nameof(getAccessTokenAsync));
            Contract.CheckValue(secretManagerConfiguration, nameof(secretManagerConfiguration));

            keyVaultUri = secretManagerConfiguration.KeyVaultUri;

            if (keyVaultClient is null)
            {
                keyVaultClient = new KeyVaultClient(getAccessTokenAsync, new HttpClient());
            }

            retryPolicy = new FixedIntervalRetryPolicy<Exception>(DefaultRetryInterval, DefaultRetryCount);
        }

        /// <summary>
        /// Internal constructor for testing which allows to supply an IKeyVaultClient instance.
        /// </summary>
        internal SecretManager(IKeyVaultClient keyVaultClient, KeyVaultConfiguration secretManagerConfiguration)
        {
            Contract.CheckValue(keyVaultClient, nameof(keyVaultClient));
            Contract.CheckValue(secretManagerConfiguration, nameof(secretManagerConfiguration));

            if (SecretManager.keyVaultClient is null)
            {
                SecretManager.keyVaultClient = keyVaultClient;
            }

            retryPolicy = new FixedIntervalRetryPolicy<Exception>(DefaultRetryInterval, DefaultRetryCount);
        }

        /// <summary>
        /// Read secret from key vault.
        /// </summary>
        public async Task<SecretBundle> ReadSecretAsync(string secretName)
        {
            Contract.CheckNonEmpty(secretName, nameof(secretName));

            return await ExecuteAsync(
                async () => await keyVaultClient.GetSecretAsync(keyVaultUri, secretName),
                SecretManagerReadSecretActivity.Instance,
                (exception) => $"Could not read secret. KeyVaultUri: {keyVaultUri}, SecretName: {secretName}, Exception: {exception}.");
        }

        /// <summary>
        /// Read secrets from key vault.
        /// </summary>
        public async Task<IReadOnlyDictionary<string, SecretBundle>> ReadSecretsAsync(IList<string> secretNames)
        {
            Contract.CheckAllNonEmpty(secretNames, nameof(secretNames));

            return await CommonLogger.Logger.ExecuteAsync(
                SecretManagerReadSecretsActivity.Instance,
                async () =>
                {
                    var secretTasks = secretNames
                        .Aggregate<string, IDictionary<string, Task<SecretBundle>>>(
                        new Dictionary<string, Task<SecretBundle>>(),
                        (dictionary, secretName) =>
                        {
                            var secret = ReadSecretAsync(secretName);
                            dictionary.Add(secretName, secret);
                            return dictionary;
                        });

                    await Task.WhenAll(secretTasks.Values);

                    return new ReadOnlyDictionary<string, SecretBundle>(secretTasks.ToDictionary(s => s.Key, s => s.Value.Result));
                });
        }

        /// <summary>
        /// Write secret to the key vault.
        /// </summary>
        public async Task<SecretBundle> WriteSecretAsync(string secretName, string secretValue)
        {
            Contract.CheckNonEmpty(secretName, nameof(secretName));
            Contract.CheckNonEmpty(secretValue, nameof(secretValue));

            return await ExecuteAsync(
                async () => await keyVaultClient.SetSecretAsync(keyVaultUri, secretName, secretValue, contentType: SecretContentType),
                SecretManagerWriteSecretActivity.Instance,
                (exception) => $"Could not write secret. KeyVaultUri: {keyVaultUri}, SecretName: {secretName}, Exception: {exception}.");
        }

        /// <summary>
        /// Delete secret from key vault.
        /// </summary>
        public async Task<SecretBundle> DeleteSecretAsync(string secretName)
        {
            Contract.CheckNonEmpty(secretName, nameof(secretName));

            return await ExecuteAsync(
                async () => await keyVaultClient.DeleteSecretAsync(keyVaultUri, secretName),
                SecretManagerDeleteSecretActivity.Instance,
                (exception) => $"Could not delete secret. KeyVaultUri: {keyVaultUri}, SecretName: {secretName}, Exception: {exception}.");
        }

        /// <summary>
        /// Check if a secret exists in the key vault.
        /// </summary>
        public async Task<TryGetResult<SecretBundle>> TryGetSecretAsync(string secretName)
        {
            Contract.CheckNonEmpty(secretName, nameof(secretName));
            SecretBundle secret = new SecretBundle();
            return await ExecuteAsync(
                async () =>
                {
                    try
                    {
                        if (!DictionaryCache.ContainsKey(secretName))
                        {
                            secret = await keyVaultClient.GetSecretAsync(keyVaultUri, secretName);
                        }
                        else
                        {
                            DictionaryCache.TryGetValue(secretName, out secret);
                        }
                    }
                    catch (KeyVaultErrorException ex)
                    {
                        var keyVaultError = ex.Body;
                        if (keyVaultError != null && string.Equals(keyVaultError.Error.Code, SecretNotFoundErrorCode, StringComparison.OrdinalIgnoreCase))
                        {
                            return TryGetResult.Failed<SecretBundle>();
                        }

                        SecretManagerTrace.Instance.TraceError($"Could not check secret. KeyVaultUri: {keyVaultUri}, name: {secretName} Exception: {ex.ToString()}.");
                        throw;
                    }

                    return TryGetResult.Create(secret);
                },
                SecretManagerTryGetSecretActivity.Instance,
                (exception) => $"Could not read secret. KeyVaultUri: {keyVaultUri}, SecretName: {secretName}, Exception: {exception}.");
        }

        /// <summary>
        /// Get secret 
        /// </summary>
        /// <param name="secretName"> secret name</param>
        /// <param name="logger">logger</param>
        /// <returns></returns>
        public async Task<string> GetSecretAsync(string secretName, Microsoft.Extensions.Logging.ILogger logger)
        {
            Contract.CheckNonEmpty(secretName, nameof(secretName));
            Contract.CheckValue(logger, nameof(logger));

            logger.LogInformation($"GetSecretAsync: Trying to read {secretName} from keyVault");

            TryGetResult<SecretBundle> secretBundle = null;
            var secretValue = string.Empty;

            if (DictionaryCache.ContainsKey(secretName))
            {
                DictionaryCache.TryGetValue(secretName, out SecretBundle secret);
                secretValue = secret?.Value;
            }
            else
            {
                secretBundle = await ExecuteAsync(
                   async () =>
                   {
                       return await this.TryGetSecretAsync(secretName);
                   },
                   SecretManagerTryGetSecretActivity.Instance,
                (exception) => $"Could not read secret. KeyVaultUri: {keyVaultUri}, SecretName: {secretName}, Exception: {exception}.");
                if (secretBundle.Succeeded)
                {
                    logger.LogInformation($"GetSecretAsync: Successfully read {secretName} from keyVault");
                    secretValue = secretBundle.Result.Value;
                }

                if (string.IsNullOrEmpty(secretValue))
                {
                    throw new System.Exception($"GetSecretAsync: Error getting secret {secretName} from the keyvault");
                    ////throw new KeyVaultAccessException($"GetSecretAsync: Error getting secret {secretKey} from the keyvault").EnsureTraced(trace);
                }
            }

            return secretValue;
        }

        /// <summary>
        /// Consolidate logic around activity execution, retry configuration, and tracing so it is guaranteed consistency and can be tested once.
        /// </summary>
        private async Task<TResult> ExecuteAsync<TResult, TActivityType>(Func<Task<TResult>> func, SingletonActivityType<TActivityType> activity, Func<Exception, string> failureMessage) where TActivityType : ActivityType, new()
        {
            return await CommonLogger.Logger.Execute(
                activity,
                async () =>
                {
                    try
                    {
                        return await retryPolicy.ExecuteAsync(func);
                    }
                    catch (KeyVaultErrorException exception)
                    {
                        SecretManagerTrace.Instance.TraceError(failureMessage(exception));
                        throw;
                    }
                });
        }
    }
}
