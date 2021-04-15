//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.WebNotifications.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.WebNotifications.Models.Enums;
    using HR.TA.Common.WebNotifications.Models;

    /// <summary>
    /// The <see cref="WebNotificationRequest"/> class stores the web notification request data.
    /// </summary>
    [DataContract]
    public class WebNotificationRequest
    {
        /// <summary>
        /// Gets the notification identifier.
        /// </summary>
        /// <value>
        /// The notification identifier.
        /// </value>
        [DataMember(Name = "notificationId")]
        public string NotificationId { get; private set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the notification title.
        /// </summary>
        /// <value>
        /// The notification title.
        /// </value>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the notification body.
        /// </summary>
        /// <value>
        /// The notification body.
        /// </value>
        [DataMember(Name = "body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the application defined notification type.
        /// </summary>
        /// <value>
        /// The application defined notification type.
        /// </value>
        [DataMember(Name = "appNotificationType")]
        public string AppNotificationType { get; set; }

        /// <summary>
        /// Gets or sets the notification properties.
        /// </summary>
        /// <value>
        /// The notification properties.
        /// </value>
        [DataMember(Name = "properties")]
        public Dictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets or sets the notification priority.
        /// </summary>
        /// <value>
        /// The notification priority.
        /// </value>
        [DataMember(Name = "priority")]
        public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

        /// <summary>
        /// Gets or sets the notification recipient.
        /// </summary>
        /// <value>
        /// The notification recipient.
        /// </value>
        [DataMember(Name = "recipient")]
        public ParticipantData Recipient { get; set; }

        /// <summary>
        /// Gets or sets the notification sender.
        /// </summary>
        /// <value>
        /// The notification sender.
        /// </value>
        [DataMember(Name = "sender")]
        public ParticipantData Sender { get; set; }

        /// <summary>
        /// This is to denote if the current call is made from wob context.
        /// </summary>
        [DataMember(Name = "isWobContextUser")]
        public bool IsWobContextUser { get; set; }

        /// <summary>
        /// This is to get or set context user id in case of wob user.
        /// </summary>
        [DataMember(Name = "contextUserId")]
        public string ContextUserId { get; set; }

        /// <summary>
        /// Gets or sets the notification publish UTC date.
        /// </summary>
        /// <value>
        /// The notification publish UTC date.
        /// </value>
        [DataMember(Name = "publishOnUTCDate")]
        public DateTime PublishOnUTCDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the send/received on UTC date.
        /// </summary>
        [DataMember(Name = "sendOnUtcDate")]
        public DateTime SendOnUtcDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the notification expires on date (UTC).
        /// </summary>
        /// <value>
        /// The notification expires on date (UTC).
        /// </value>
        [DataMember(Name = "expiresOnUTCDate")]
        public DateTime? ExpiresOnUTCDate { get; set; } = DateTime.UtcNow.AddMonths(3);

        /// <summary>
        /// Gets the notification tracking identifier.
        /// </summary>
        /// <value>
        /// The notification tracking identifier.
        /// </value>
        [DataMember(Name = "trackingId")]
        public string TrackingId { get; private set; } = Guid.NewGuid().ToString();
    }
}
