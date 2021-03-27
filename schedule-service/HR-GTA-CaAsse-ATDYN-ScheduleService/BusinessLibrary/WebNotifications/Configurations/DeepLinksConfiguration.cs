//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Configurations
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// The <see cref="DeepLinksConfiguration"/> class stores the deep links templates.
    /// </summary>
    [SettingsSection("DeepLinksConfiguration")]
    public class DeepLinksConfiguration
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the schedule summary template URL.
        /// </summary>
        /// <value>
        /// The schedule summary template URL.
        /// </value>
        public string ScheduleSummaryUrl { get; set; }
    }
}
