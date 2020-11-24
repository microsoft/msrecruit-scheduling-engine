//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Configuration;

namespace MS.GTA.ServicePlatform.Azure.Security
{
    [SettingsSection("KeyVaultConfiguration")]
    public sealed class KeyVaultConfiguration
    {
        public string KeyVaultUri { get; set; }
    }
}
