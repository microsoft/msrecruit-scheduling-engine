//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using ServicePlatform.Configuration;

namespace ServicePlatform.Azure.Security
{
    [SettingsSection("KeyVaultConfiguration")]
    public sealed class KeyVaultConfiguration
    {
        public string KeyVaultUri { get; set; }
    }
}
