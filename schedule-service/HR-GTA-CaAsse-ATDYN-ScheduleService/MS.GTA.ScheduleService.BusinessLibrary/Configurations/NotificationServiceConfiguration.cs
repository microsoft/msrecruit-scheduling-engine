// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="NotificationServiceConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary
{
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>
    /// Graph Configuration
    /// </summary>
    [SettingsSection("NotificationServiceConfiguration")]
    public class NotificationServiceConfiguration
    {
        /// <summary>
        /// Gets or sets the root url of the Notification service.
        /// </summary>
        public string EndpointUrl { get; set; }

        /// <summary>
        /// Gets or sets the Uri for sending emails using Notification service.
        /// </summary>
        public string SendNotificationUri { get; set; }

        /// <summary>
        /// Gets or sets the Uri for re-sending emails using Notification service.
        /// </summary>
        public string ResendNotificationUri { get; set; }

        /// <summary>
        /// Gets or sets the Uri for queueing emails using Notification service.
        /// </summary>
        public string QueueNotificationUri { get; set; }

        /// <summary>
        /// Gets or sets the name of the Application in Notification Service configuration.
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
