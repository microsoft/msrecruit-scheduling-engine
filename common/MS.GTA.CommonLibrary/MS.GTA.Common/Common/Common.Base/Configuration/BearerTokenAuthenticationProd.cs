//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="BearerTokenAuthenticationProd.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Configuration
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
