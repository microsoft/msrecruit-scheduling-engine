//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.IV.Configuration
{
    using ServicePlatform.Configuration;

    [SettingsSection("IVApplicationConfiguration")]
    public class IVApplicationConfiguration
    {
        public int ApplicantsDefaultRows { get; set; }
    }
}
