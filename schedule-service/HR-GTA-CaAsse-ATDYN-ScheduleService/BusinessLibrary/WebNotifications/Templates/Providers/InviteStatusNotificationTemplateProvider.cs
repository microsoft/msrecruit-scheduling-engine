// <copyright file="InviteStatusNotificationTemplateProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Templates.Providers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.WebNotifications.Exceptions;
    using MS.GTA.CommonDataService.Common.Internal;

    /// <summary>
    /// The <see cref="InviteStatusNotificationTemplateProvider"/> class implements mechanism to provide the web notification template for invite status update event.
    /// </summary>
    /// <seealso cref="WebNotificationBaseTemplateProvider" />
    internal class InviteStatusNotificationTemplateProvider : WebNotificationBaseTemplateProvider
    {
        /// <summary>
        /// The template name. The value should match text in resource file.
        /// </summary>
        private const string TemplateName = "InviteStatusUpdateTemplate";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<InviteStatusNotificationTemplateProvider> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteStatusNotificationTemplateProvider"/> class.
        /// </summary>
        /// <param name="logger">The instance for <see cref="ILogger{InviteStatusNotificationTemplateProvider}"/>.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        public InviteStatusNotificationTemplateProvider(ILogger<InviteStatusNotificationTemplateProvider> logger)
            : base(TemplateName)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc cref="WebNotificationBaseTemplateProvider"/>
        public override string ProvideTemplate(Dictionary<string, string> notificationProperties)
        {
            string templateText = null;
            this.logger.LogInformation($"Started {nameof(this.ProvideTemplate)} method in {nameof(InviteStatusNotificationTemplateProvider)}");
            Contract.CheckValue(notificationProperties, nameof(notificationProperties));
            if (notificationProperties.Count == 0)
            {
                throw new WebNotificationException("Empty notification properties dictionary.");
            }

            if (notificationProperties.ContainsKey(WebNotificationConstants.InterviewerName))
            {
                if (notificationProperties.ContainsKey(WebNotificationConstants.MessageResponse)
                    && notificationProperties.ContainsKey(WebNotificationConstants.ProposedStartTime))
                {
                    templateText = this.TemplateObject.DefaultTemplateWithMessageAndNewTime;
                }
                else if (notificationProperties.ContainsKey(WebNotificationConstants.MessageResponse))
                {
                    templateText = this.TemplateObject.DefaultTemplateWithMessage;
                }
                else if (notificationProperties.ContainsKey(WebNotificationConstants.ProposedStartTime))
                {
                    templateText = this.TemplateObject.DefaultTemplateWithNewProposedTime;
                }
                else
                {
                    templateText = this.TemplateObject.DefaultTemplate;
                }
            }

            if (string.IsNullOrWhiteSpace(templateText))
            {
                throw new WebNotificationException("No matching notification template found in invite status update event.");
            }

            this.logger.LogInformation($"Finished {nameof(this.ProvideTemplate)} method in {nameof(InviteStatusNotificationTemplateProvider)}");
            return templateText;
        }
    }
}
