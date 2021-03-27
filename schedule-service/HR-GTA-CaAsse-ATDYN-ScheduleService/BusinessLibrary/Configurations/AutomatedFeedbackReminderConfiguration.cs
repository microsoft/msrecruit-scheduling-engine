//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Configurations
{
    using System;
    using HR.TA.ServicePlatform.Configuration;

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
