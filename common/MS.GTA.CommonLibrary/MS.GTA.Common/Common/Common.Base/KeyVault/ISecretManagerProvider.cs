﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ISecretManagerProvider.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.KeyVault
{
    using System.Threading.Tasks;
    using ServicePlatform.Azure.Security;

    /// <summary>
    /// The secret manager provider
    /// </summary>
    public interface ISecretManagerProvider
    {
        /// <summary>
        /// Gets or creates a valid Secret Manager instance.
        /// </summary>
        /// <param name="keyVaultUri">The URI for the key vault resource managed by the secret manager</param>
        /// <returns>The <see cref="ISecretManager"/></returns>
        ISecretManager GetOrCreateSecretManager(string keyVaultUri);

        Task<string> GetVaultSecret(string keyVaultUri, string secretName);
    }
}
