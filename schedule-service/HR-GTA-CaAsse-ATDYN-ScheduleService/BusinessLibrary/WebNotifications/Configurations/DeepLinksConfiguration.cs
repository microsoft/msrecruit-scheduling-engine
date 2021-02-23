//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Configurations
{
    using MS.GTA.ServicePlatform.Configuration;

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
