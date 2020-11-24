//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// The connection details for a storage configuration.
    /// </summary>
    public class ConnectionDetails
    {
        /// <summary>
        /// Gets or sets the name of the key vault.
        /// </summary>
        [JsonProperty(PropertyName = "keyVaultName")]
        public string KeyVaultName { get; set; }

        /// <summary>
        /// Gets or sets the name of the key vault secret.
        /// </summary>
        [JsonProperty(PropertyName = "keyVaultSecretName")]
        public string KeyVaultSecretName { get; set; }

        /// <summary>
        /// Gets or sets the secret version.
        /// </summary>
        [JsonProperty(PropertyName = "secretVersion")]
        public string SecretVersion { get; set; }

        /// <summary>
        /// Gets or sets the connection details type, such as KeyVault.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
