// ----------------------------------------------------------------------------
// <copyright file="AutomatedFeedbackReminderConfiguration.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Configurations
{
    using System;
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>
    /// Automated feedback reminder configuration settings class
    /// </summary>
    [SettingsSection("AutomatedFeedbackReminderConfiguration")]
    public class AutomatedFeedbackReminderConfiguration
    {
        /// <summary>Gets or sets the feedback reminder offset duration in hours. </summary>
        public int FeedbackReminderOffsetDurationHours { get; set; }

        /// <summary>
        /// Gets or sets the feedback reminder window in minutes.
        /// </summary>
        public int FeedbackReminderWindowMinutes { get; set; }
    }
}
