//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.ServicePlatform.Configuration;

namespace TA.CommonLibrary.ServicePlatform.Azure.Security
{
    [SettingsSection("KeyVaultConfiguration")]
    public sealed class KeyVaultConfiguration
    {
        public string KeyVaultUri { get; set; }
    }
}
