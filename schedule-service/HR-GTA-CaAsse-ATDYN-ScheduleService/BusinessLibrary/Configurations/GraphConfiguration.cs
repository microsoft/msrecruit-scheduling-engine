//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// Graph Configuration
    /// </summary>
    [SettingsSection("GraphConfiguration")]
    public class GraphConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the secret in the key vault to retrieve the scheduler service account user name.
        /// </summary>
        public string ScheduleEmailSecrets { get; set; }

        /// <summary>
        /// Gets or sets the name of the secret in the key vault to retrieve the scheduler service account password.
        /// </summary>
        public string ScheduleEmailPasswordSecrets { get; set; }

        /// <summary>
        /// Gets or sets native client app id.
        /// </summary>
        public string NativeClientAppId { get; set; }
    }
}
