//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Configurations
{
        using System;
        using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// Scheduler user configuration settings class
    /// </summary>
    [SettingsSection("SchedulerConfiguration")]
    public class SchedulerConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the secret in the key vault to retrieve the scheduler service account user name.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user account to get the user profile details from graph
        /// </summary>
        public string UserProfileEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the free busy details of the users from graph
        /// </summary>
        public string FreeBusyEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets native client app id.
        /// </summary>
        public string NativeClientAppId { get; set; }

        /// <summary>
        /// Gets or sets scheduler email password secret. The secret value is json of user emails and password list.
        /// </summary>
        public string SchedulerEmailPasswordSecret { get; set; }
    }
}
