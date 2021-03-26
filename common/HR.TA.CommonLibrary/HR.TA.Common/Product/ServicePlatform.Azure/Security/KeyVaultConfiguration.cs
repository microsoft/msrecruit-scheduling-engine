//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.ServicePlatform.Configuration;

namespace HR.TA.ServicePlatform.Azure.Security
{
    [SettingsSection("KeyVaultConfiguration")]
    public sealed class KeyVaultConfiguration
    {
        public string KeyVaultUri { get; set; }
    }
}
