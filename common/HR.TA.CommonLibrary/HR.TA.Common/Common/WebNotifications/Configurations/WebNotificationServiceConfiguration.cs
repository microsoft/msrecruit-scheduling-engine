//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.Common.WebNotifications.Configurations
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// The <see cref="WebNotificationServiceConfiguration"/> class holds configuration information to connect to web notifications service.
    /// </summary>
    [SettingsSection("WebNotificationServiceConfiguration")]
    public class WebNotificationServiceConfiguration
    {
        /// <summary>
        /// Gets or sets the base url of the web notification service.
        /// </summary>
        public string BaseServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the post notifications endpoint relative URL.
        /// </summary>
        /// <value>
        /// The post notifications endpoint relative URL.
        /// </value>
        public string PostWebNotificationsRelativeUrl { get; set; }
    }
}
