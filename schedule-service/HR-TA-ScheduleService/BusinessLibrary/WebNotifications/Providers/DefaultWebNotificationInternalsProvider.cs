//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Providers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using HR.TA.Common.WebNotifications.Interfaces;
    using HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Configurations;
    using HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Extractors;
    using HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Templates.Providers;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.Data.Models;
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// The <see cref="DefaultWebNotificationInternalsProvider"/> class implements internal objects factory.
    /// </summary>
    /// <seealso cref="IWebNotificationInternalsProvider" />
    public class DefaultWebNotificationInternalsProvider : IWebNotificationInternalsProvider
    {
        /// <summary>
        /// The single time objects.
        /// </summary>
        private readonly Dictionary<Type, object> singleTimeObjects = new Dictionary<Type, object>();

        /// <summary>
        /// The lock object.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The configuration manager.
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// The logger factory.
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The Schedule query.
        /// </summary>
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebNotificationInternalsProvider"/> class.
        /// </summary>
        /// <param name="configurationManager">The instance for <see cref="IConfigurationManager"/>..</param>
        /// <param name="scheduleQuery">The instance for <see cref="IScheduleQuery"/>..</param>
        /// <param name="loggerFactory">The instance for <see cref="ILoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// configurationManager
        /// or
        /// loggerFactory.
        /// </exception>
        public DefaultWebNotificationInternalsProvider(IConfigurationManager configurationManager, IScheduleQuery scheduleQuery, ILoggerFactory loggerFactory)
        {
            this.configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.scheduleQuery = scheduleQuery ?? throw new ArgumentNullException(nameof(scheduleQuery));
        }

        /// <inheritdoc/>
        public IWebNotificationDataExtractor GetInviteStatusUpdateWebNotificationDataExtractor(IEnumerable<InterviewerInviteResponseInfo> interviewerInviteResponseInfos)
        {
            DeepLinksConfiguration deepLinksConfiguration = this.configurationManager.Get<DeepLinksConfiguration>();
            return new InviteStatusNotificationDataExtractor(interviewerInviteResponseInfos, deepLinksConfiguration, this.scheduleQuery, this.loggerFactory.CreateLogger<InviteStatusNotificationDataExtractor>());
        }

        /// <inheritdoc/>
        public IWebNotificationTemplateProvider GetInviteStatusUpdateWebNotificationTemplateProvider()
        {
            InviteStatusNotificationTemplateProvider inviteStatusNotificationTemplateProvider;
            lock (this.lockObject)
            {
                if (this.singleTimeObjects.ContainsKey(typeof(InviteStatusNotificationTemplateProvider)))
                {
                    inviteStatusNotificationTemplateProvider = this.singleTimeObjects[typeof(InviteStatusNotificationTemplateProvider)] as InviteStatusNotificationTemplateProvider;
                }
                else
                {
                    inviteStatusNotificationTemplateProvider = new InviteStatusNotificationTemplateProvider(this.loggerFactory.CreateLogger<InviteStatusNotificationTemplateProvider>());
                    this.singleTimeObjects[typeof(InviteStatusNotificationTemplateProvider)] = inviteStatusNotificationTemplateProvider;
                }
            }

            return inviteStatusNotificationTemplateProvider;
        }
    }
}
