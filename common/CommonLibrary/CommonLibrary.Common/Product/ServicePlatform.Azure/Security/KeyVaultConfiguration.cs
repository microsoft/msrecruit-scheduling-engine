//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Configuration;

namespace CommonLibrary.ServicePlatform.Azure.Security
{
    [SettingsSection("KeyVaultConfiguration")]
    public sealed class KeyVaultConfiguration
    {
        public string KeyVaultUri { get; set; }
    }
}
