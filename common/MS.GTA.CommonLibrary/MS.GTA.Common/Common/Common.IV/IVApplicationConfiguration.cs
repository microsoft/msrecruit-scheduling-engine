//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.IV.Configuration
{
    using MS.GTA.ServicePlatform.Configuration;

    [SettingsSection("IVApplicationConfiguration")]
    public class IVApplicationConfiguration
    {
        public int ApplicantsDefaultRows { get; set; }
    }
}
