//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// Type class for BearerTokenAuthentication production 
    /// </summary>
    [SettingsSection("BearerTokenAuthenticationProd")]
    public sealed class BearerTokenAuthenticationProd : BearerTokenAuthentication
    {
    }
}
