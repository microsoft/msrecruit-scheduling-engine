//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.IV.Configuration
{
    using TA.CommonLibrary.ServicePlatform.Configuration;

    [SettingsSection("IVApplicationConfiguration")]
    public class IVApplicationConfiguration
    {
        public int ApplicantsDefaultRows { get; set; }
    }
}
