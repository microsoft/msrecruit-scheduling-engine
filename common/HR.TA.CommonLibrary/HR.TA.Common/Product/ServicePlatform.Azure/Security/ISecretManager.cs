//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Logging;
using HR.TA.CommonDataService.Common.Internal;

namespace HR.TA.ServicePlatform.Azure.Security
{
    public interface ISecretManager
    {
        Task<SecretBundle> ReadSecretAsync(string secretName);

        Task<IReadOnlyDictionary<string, SecretBundle>> ReadSecretsAsync(IList<string> secretNames);

        Task<SecretBundle> WriteSecretAsync(string secretName, string secretValue);

        Task<SecretBundle> DeleteSecretAsync(string secretName);

        Task<TryGetResult<SecretBundle>> TryGetSecretAsync(string secretName);

        Task<string> GetSecretAsync(string secretName, ILogger logger);
    }
}
