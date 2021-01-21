// <copyright file="NotificationRequest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using MS.GTA.Common.DocumentDB.V2;
using MS.GTA.ScheduleService.Contracts.V1;
using MS.GTA.ServicePlatform.Azure.Security;
using MS.GTA.ServicePlatform.Configuration;

namespace MS.GTA.ScheduleService.Contracts.V1
{
    /// <summary>
    /// Notification Request Contract
    /// </summary>
    public class NotificationRequest
    {
        /// <summary>
        /// Gets or sets Document DB instance for calendar event
        /// </summary>
        public IDocumentDBProvider<CalendarEvent> DocumentDBCalendarEvent { get; set; }

        /// <summary>
        /// Gets or sets Document DB instance for Subscription
        /// </summary>
        public IDocumentDBProvider<SubscriptionViewModel> DocumentDBSubscription { get; set; }

        /// <summary>
        /// Gets or sets Document DB instance for MeetingInfo
        /// </summary>
        public IDocumentDBProvider<MeetingInfo> DocumentDBMeetingInfo { get; set; }

        /// <summary>
        /// Gets or sets notification content
        /// </summary>
        public string NotificationContent { get; set; }

        /// <summary>
        /// Gets or sets graph base URL
        /// </summary>
        public string GraphBaseUrl { get; set; }

        /// <summary>Gets or sets the configuration manager. </summary>
        public IConfigurationManager ConfigurationManager { get; set; }

        /// <summary>
        /// Gets or sets the secret manager class.
        /// </summary>
        public ISecretManager SecretManager { get; set; }
    }
}
