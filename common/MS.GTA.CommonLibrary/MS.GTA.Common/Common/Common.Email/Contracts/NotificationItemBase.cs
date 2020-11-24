//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="NotificationItemBase.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Common.Common.Email.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class NotificationItemBase
    {
        /// <summary>
        /// Gets or sets the type of the notify.
        /// </summary>
        public abstract FrameworkNotificationType NotifyType { get; }

        /// <summary>
        /// Gets or sets the send on UTC date.
        /// </summary>
        public DateTime SendOnUtcDate { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NotificationItemBase()
        {
            SendOnUtcDate = DateTime.UtcNow;
            Category = string.Empty;
        }
        /// <summary>
        /// Gets or sets the TrackingId.
        /// </summary>
        public string TrackingId { get; set; }
        /// <summary>
        /// Gets or sets the Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Gets or sets the ReminderRuleId associated to the Notification Item.
        /// </summary>
        public string ReminderRuleId { get; set; }
        /// <summary>
        /// Boolean flag to indicate if this notification item is a Reminder to another parent notification item.
        /// </summary>
        public bool IsReminder { get; set; }
        /// <summary>
        /// Parent Notification Id corresponding to a Reminder type notification.
        /// </summary>
        public string ParentNotificationId { get; set; }
    }
}
