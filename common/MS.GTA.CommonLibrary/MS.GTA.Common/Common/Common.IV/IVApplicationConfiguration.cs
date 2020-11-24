//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IVApplicationConfiguration.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.IV.Configuration
{
    using MS.GTA.ServicePlatform.Configuration;

    [SettingsSection("IVApplicationConfiguration")]
    public class IVApplicationConfiguration
    {
        public int ApplicantsDefaultRows { get; set; }
    }
}
