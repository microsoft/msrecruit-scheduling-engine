//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.IV.Configuration
{
    using CommonLibrary.ServicePlatform.Configuration;

    [SettingsSection("IVApplicationConfiguration")]
    public class IVApplicationConfiguration
    {
        public int ApplicantsDefaultRows { get; set; }
    }
}
