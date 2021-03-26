//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.WebNotifications.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.Base.Utilities;
    using HR.TA.Common.WebNotifications;
    using HR.TA.Common.WebNotifications.Exceptions;
    using HR.TA.Common.WebNotifications.Interfaces;
    using HR.TA.Common.WebNotifications.Models;
    using HR.TA.CommonDataService.Common.Internal;

    /// <summary>
    /// The <see cref="DefaultWebNotificationBuilder"/> class builds the web ntifications.
    /// </summary>
    /// <seealso cref="IWebNotificationBuilder" />
    internal class DefaultWebNotificationBuilder : IWebNotificationBuilder
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<DefaultWebNotificationBuilder> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebNotificationBuilder"/> class.
        /// </summary>
        /// <param name="logger">The instance for <see cref="ILogger{DefaultWebNotificationBuilder}"/>.</param>
        /// <exception cref="ArgumentNullException">logger.</exception>
        public DefaultWebNotificationBuilder(ILogger<DefaultWebNotificationBuilder> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Builds the web notifications.
        /// </summary>
        /// <param name="notificationDataExtractor">The instance for <see cref="IWebNotificationDataExtractor"/>.</param>
        /// <param name="templateProvider">The instance for <see cref="IWebNotificationTemplateProvider"/>.</param>
        /// <returns>The instance of <see cref="IEnumerable{WebNotificationRequestContainer}"/> in <see cref="Task"/>.</returns>
        public async Task<IEnumerable<WebNotificationRequest>> Build(IWebNotificationDataExtractor notificationDataExtractor, IWebNotificationTemplateProvider templateProvider)
        {
            this.logger.LogInformation($"Started {nameof(this.Build)} method in {nameof(DefaultWebNotificationBuilder)}");
            Contract.CheckValue(notificationDataExtractor, nameof(notificationDataExtractor));
            Contract.CheckValue(templateProvider, nameof(templateProvider));
            IEnumerable<WebNotificationRequest> webNotificationRequests = await this.BuildRequests(notificationDataExtractor, templateProvider).ConfigureAwait(false);
            this.logger.LogInformation($"Finished {nameof(this.Build)} method in {nameof(DefaultWebNotificationBuilder)}");
            return webNotificationRequests;
        }

        /// <summary>
        /// Builds the web notification requests.
        /// </summary>
        /// <param name="notificationDataExtractor">The instance for <see cref="IWebNotificationDataExtractor"/>.</param>
        /// <param name="templateProvider">The instance for <see cref="IWebNotificationTemplateProvider"/>.</param>
        /// <returns>The collection of <see cref="WebNotificationRequest"/>.</returns>
        private async Task<IEnumerable<WebNotificationRequest>> BuildRequests(IWebNotificationDataExtractor notificationDataExtractor, IWebNotificationTemplateProvider templateProvider)
        {
            WebNotificationRequest webNotificationRequest;
            List<WebNotificationRequest> webNotificationRequests = new List<WebNotificationRequest>();
            Debug.Assert(notificationDataExtractor != null, "Missing Notification data extractor.");
            Debug.Assert(templateProvider != null, "Missing Notification Template Provider.");
            IEnumerable<Dictionary<string, string>> notificationsData = await notificationDataExtractor.Extract().ConfigureAwait(false);
            if (notificationsData.Any())
            {
                notificationsData.ForEach(notificationData =>
                {
                    try
                    {
                        webNotificationRequest = this.BuildNotification(notificationData, templateProvider);
                        if (webNotificationRequest != null)
                        {
                            webNotificationRequests.Add(webNotificationRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Do not rethrow to ensure execution progress to next web notification data.
                        this.logger.LogError(ex, "Error building web notification. Skipping to next...");
                    }
                });
            }

            return webNotificationRequests;
        }

        /// <summary>
        /// Builds the web notification.
        /// </summary>
        /// <param name="notificationData">The notification data.</param>
        /// <param name="templateProvider">The instance for <see cref="IWebNotificationTemplateProvider"/>.</param>
        /// <returns>The instance of <see cref="WebNotificationRequest"/>.</returns>
        private WebNotificationRequest BuildNotification(Dictionary<string, string> notificationData, IWebNotificationTemplateProvider templateProvider)
        {
            Debug.Assert(notificationData != null, "Missing notification data.");
            Debug.Assert(templateProvider != null, "Missing notification template provider.");
            string notificationBody;
            WebNotificationRequest webNotificationRequest = null;
            if (notificationData.Any())
            {
                notificationBody = this.PrepareBody(notificationData, templateProvider);
                if (!string.IsNullOrWhiteSpace(notificationBody))
                {
                    webNotificationRequest = new WebNotificationRequest
                    {
                        Body = notificationBody,
                        Sender = new ParticipantData
                        {
                            ObjectIdentifier = notificationData[NotificationConstants.SenderObjectId],
                            Name = notificationData[NotificationConstants.SenderName],
                            Email = notificationData[NotificationConstants.SenderEmail],
                        },
                        Recipient = new ParticipantData
                        {
                            ObjectIdentifier = notificationData[NotificationConstants.RecipientObjectId],
                            Name = notificationData[NotificationConstants.RecipientName],
                            Email = notificationData[NotificationConstants.RecipientEmail],
                        },
                        Title = notificationData[NotificationConstants.Title],
                        IsWobContextUser = Convert.ToBoolean(notificationData[NotificationConstants.IsWobContext]),
                        ContextUserId = Convert.ToBoolean(notificationData[NotificationConstants.IsWobContext]) ? 
                                        notificationData[NotificationConstants.ContextUserId] : null,
                        AppNotificationType = notificationData[NotificationConstants.AppNotificationType],
                    };

                    notificationData.Remove(NotificationConstants.SenderObjectId);
                    notificationData.Remove(NotificationConstants.SenderEmail);
                    notificationData.Remove(NotificationConstants.SenderName);
                    notificationData.Remove(NotificationConstants.RecipientObjectId);
                    notificationData.Remove(NotificationConstants.RecipientEmail);
                    notificationData.Remove(NotificationConstants.RecipientName);
                    notificationData.Remove(NotificationConstants.Title);
                    notificationData.Remove(NotificationConstants.AppNotificationType);
                    if (notificationData[NotificationConstants.DeepLink].IndexOf("?") == -1)
                    {
                        notificationData[NotificationConstants.DeepLink] = notificationData[NotificationConstants.DeepLink] + "?ref=webnotification";
                    }
                    else
                    {
                        notificationData[NotificationConstants.DeepLink] = notificationData[NotificationConstants.DeepLink] + "&ref=webnotification";
                    }

                    webNotificationRequest.Properties = notificationData;
                }
            }

            return webNotificationRequest;
        }

        /// <summary>
        /// Prepares the notification body.
        /// </summary>
        /// <param name="notificationData">The notification data.</param>
        /// <param name="templateProvider">The instance for <see cref="IWebNotificationTemplateProvider"/>.</param>
        /// <returns>The notification body text.</returns>
        private string PrepareBody(Dictionary<string, string> notificationData, IWebNotificationTemplateProvider templateProvider)
        {
            string notificationBody = null;
            string template = templateProvider.ProvideTemplate(notificationData);
            if (string.IsNullOrWhiteSpace(template))
            {
                throw new WebNotificationException("No suitable notification template specified by the template provider.");
            }

            notificationBody = template;
            foreach (string propertyKey in notificationData.Keys)
            {
                notificationBody = notificationBody.Replace($"#{propertyKey}#", notificationData[propertyKey]);
            }

            if (notificationBody.Contains("#"))
            {
                this.logger.LogError($"The notification with body '{notificationBody}' does not have placeholder value. Skipping the notification...");
                notificationBody = null;
            }

            return notificationBody;
        }
    }
}
