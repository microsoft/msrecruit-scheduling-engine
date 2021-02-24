//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Common.Common.Email.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NotificationItem : NotificationItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationItem"/> class.
        /// </summary>
        public NotificationItem()
        {
            Priority = FrameworkNotificationPriority.Normal;
            Attachments = new NotificationAttachment[] { };
            SendOnUtcDate = DateTime.UtcNow;
            TrackingId = "";
        }

        /// <summary>
        /// Gets the type of the notify.
        /// </summary>
        public override FrameworkNotificationType NotifyType
        {
            get { return FrameworkNotificationType.Mail; }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public FrameworkNotificationPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        public string CC { get; set; }
        /// <summary>
        /// Gets or sets the BCC.
        /// </summary>
        public string BCC { get; set; }
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// Gets or sets the footer.
        /// </summary>
        public string Footer { get; set; }
        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public IEnumerable<NotificationAttachment> Attachments { get; set; }
        /// <summary>
        /// Gets or sets the TemplateId.
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// Gets or sets the Template Content Arguments.
        /// </summary>
        public string TemplateData { get; set; }
        /// <summary>
        /// Gets or sets the Template Content Arguments.
        /// </summary>
        public FrameworkMailSensitivity Sensitivity { get; set; }
        /// <summary>
        /// Gets or sets the Template Content Arguments.
        /// </summary>
        public string ReplyTo { get; set; }
    }

    public enum FrameworkNotificationType
    {
        /// <summary>
        /// Mail
        /// </summary>
        Mail,
        /// <summary>
        /// Push
        /// </summary>
        Push,
        /// <summary>
        /// Meet
        /// </summary>
        Meet

    }
    public class NotificationAttachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAttachment"/> class.
        /// </summary>
        public NotificationAttachment()
        {
            IsInline = false;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the file base64.
        /// </summary>
        public string FileBase64 { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is inline.
        /// </summary>
        public bool IsInline { get; set; }
    }
    public enum FrameworkNotificationPriority
    {
        /// <summary>
        /// High
        /// </summary>
        High,
        /// <summary>
        /// Log
        /// </summary>
        Low,
        /// <summary>
        /// Normal
        /// </summary>
        Normal
    }

    public enum FrameworkMailSensitivity
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Personal
        /// </summary>
        Personal = 1,
        /// <summary>
        /// Private
        /// </summary>
        Private = 2,
        /// <summary>
        /// Confidential
        /// </summary>
        Confidential = 3
    }
}
