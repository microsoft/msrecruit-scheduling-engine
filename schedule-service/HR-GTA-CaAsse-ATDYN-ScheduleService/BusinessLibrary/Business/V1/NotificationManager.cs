// <copyright file="NotificationManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Business.V1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.Data.DataAccess;
    using Microsoft.Extensions.Logging;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.ScheduleService.Data.Models;
    using MS.GTA.ScheduleService.Contracts.V1.Flights;
    using MS.GTA.Common.WebNotifications.Interfaces;
    using MS.GTA.Common.WebNotifications.Models;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Providers;
    using MS.GTA.Common.Base.ServiceContext;

    /// <summary>
    /// Notification Manager
    /// </summary>
    public class NotificationManager : INotificationManager
    {
        private readonly IDocDbDataAccess docDbDataAccess;
        private readonly IEmailClient emailClient;
        private readonly IOutlookProvider outlookProvider;
        private readonly IScheduleQuery scheduleQuery;
        private readonly INotificationClient notificationClient;
        private readonly IConfigurationManager configurationManager;
        private readonly IEmailTemplateDataAccess emailTemplateDataAccess;
        private readonly IEmailManager emailManager;

        /// <summary>
        /// The instance for <see cref="ILogger{NotificationManager}"/>.
        /// </summary>
        private readonly ILogger<NotificationManager> logger;

        /// <summary>
        /// The web notification builder client.
        /// </summary>
        private readonly IWebNotificationBuilderClient webNotificationBuilderClient;

        /// <summary>
        /// The web notification internals provider.
        /// </summary>
        private readonly IWebNotificationInternalsProvider webNotificationInternalsProvider;

        private readonly string messageType = "#Microsoft.Graph.Message";

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationManager" /> class.
        /// </summary>
        /// <param name="docDbDataAccess">DocDbDataAcess Object</param>
        /// <param name="emailClient">Email Client Object</param>
        /// <param name="outlookProvider">Outlook provider</param>
        /// <param name="scheduleQuery">Schedule Query</param>
        /// <param name="configurationManager">The configuration manager</param>
        /// <param name="notificationClient">The notification client</param>
        /// <param name="emailTemplateDataAccess">The email template data access</param>
        /// <param name="emailManager">The email manager</param>
        /// <param name="webNotificationInternalsProvider">The instance for <see cref="IWebNotificationInternalsProvider"/>.</param>
        /// <param name="webNotificationBuilderClient">The instance for <see cref="IWebNotificationBuilderClient"/>.</param>
        /// <param name="logger">The instance for <see cref="ILogger{NotificationManager}"/>.</param>
        public NotificationManager(
            IDocDbDataAccess docDbDataAccess,
            IEmailClient emailClient,
            IOutlookProvider outlookProvider,
            IScheduleQuery scheduleQuery,
            INotificationClient notificationClient,
            IConfigurationManager configurationManager,
            IEmailTemplateDataAccess emailTemplateDataAccess,
            IEmailManager emailManager,
            IWebNotificationInternalsProvider webNotificationInternalsProvider,
            IWebNotificationBuilderClient webNotificationBuilderClient,
            ILogger<NotificationManager> logger)
        {
            this.docDbDataAccess = docDbDataAccess;
            this.emailClient = emailClient;
            this.outlookProvider = outlookProvider;
            this.scheduleQuery = scheduleQuery;
            this.notificationClient = notificationClient;
            this.configurationManager = configurationManager;
            this.emailTemplateDataAccess = emailTemplateDataAccess;
            this.emailManager = emailManager;
            this.logger = logger;
            this.webNotificationBuilderClient = webNotificationBuilderClient ?? throw new ArgumentNullException(nameof(webNotificationBuilderClient));
            this.webNotificationInternalsProvider = webNotificationInternalsProvider ?? throw new ArgumentNullException(nameof(webNotificationInternalsProvider));
        }

        /// <summary>
        /// Process notification requests
        /// </summary>
        /// <param name="notificationContent">notification content</param>
        /// <returns>Task for processing pending requests</returns>
        public async Task<bool> ProcessNotificationContent(NotificationContent notificationContent)
        {
            Task webNotificationsTask;
            List<Task> emailTasks;
            this.logger.LogInformation($"Started {nameof(this.ProcessNotificationContent)} method in {nameof(NotificationManager)}.");
            if (notificationContent == null)
            {
                throw new InvalidRequestDataValidationException("Invalid Notification Content").EnsureLogged(this.logger);
            }

            var subscriptionIds = new List<string>();
            var emailIdTokenMap = new Dictionary<string, Task<string>>();
            var messageTasks = new List<Task<Message>>();
            var updateScheduleResponseTasks = new List<Task<InterviewerInviteResponseInfo>>();

            notificationContent.Value?.ForEach(content =>
            {
                bool isMessageType = content?.ResourceData?.ODataType?.Equals(this.messageType, StringComparison.Ordinal) ?? false;
                if (isMessageType && !subscriptionIds.Contains(content.SubscriptionId))
                {
                    subscriptionIds.Add(content.SubscriptionId);
                }
                else
                {
                    this.logger.LogInformation($"ProcessNotificationContent: skipping notification with changeType {content?.ChangeType} and MessageType: {content?.ResourceData?.ODataType} and subscriptionIds: {content?.SubscriptionId} ");
                }
            });

            var subscriptions = await this.docDbDataAccess.GetSystemSubscriptionViewModelByIds(subscriptionIds);

            var subscriptionViewModels = subscriptions.ToList();
            var emailIds = subscriptionViewModels.Select(a => a.ServiceAccountEmail)?.Where(a => !string.IsNullOrEmpty(a))?.Distinct()?.ToList();
            emailIds?.ForEach(a =>
            {
                emailIdTokenMap.Add(a, this.emailClient.GetServiceAccountTokenByEmail(a));
            });

            if (notificationContent.Value != null)
            {
                foreach (var content in notificationContent.Value)
                {
                    var subscription = subscriptions.FirstOrDefault(a => a.Subscription != null && a.Subscription.Id == content.SubscriptionId);
                    if (subscription == null)
                    {
                        this.logger.LogError($"ProcessNotificationContent: Couldn't find subscription for subscriptionId: {content?.SubscriptionId}");
                        continue;
                    }

                    var token = emailIdTokenMap[subscription.ServiceAccountEmail]?.Result;
                    if (string.IsNullOrEmpty(token))
                    {
                        this.logger.LogError($"ProcessNotificationContent: Unable to generate token for emailId: {content.ServiceAccountEmail}");
                        continue;
                    }

                    this.logger.LogInformation($"ProcessNotificationContent: Adding request to retrive the message from serviceAccount: {content.ServiceAccountEmail}, subscriptionId: {content.SubscriptionId}");
                    messageTasks.Add(this.outlookProvider.GetMessageById(content?.ResourceData?.ODataId, token, content.ServiceAccountEmail));
                }
            }

            await Task.WhenAll(messageTasks);

            foreach (var message in messageTasks)
            {
                var messageContent = message?.Result;
                if (messageContent == null)
                {
                    this.logger.LogError($"ProcessNotificationContent: Failed to retrieve message");
                }

                // Pass the sender or the from address to graph along with the service account for which the token has to be generated
                var graphResponse = await this.outlookProvider.SearchUserByEmail(messageContent?.Sender?.EmailAddress?.Address, messageContent.ToRecipients.FirstOrDefault().EmailAddress.Address).ConfigureAwait(false);
                var graphUser = graphResponse?.Users?.FirstOrDefault();

                this.logger.LogInformation($"ProcessNotificationContent: Adding Request to update schedule response");
                updateScheduleResponseTasks.Add(this.scheduleQuery.UpdateScheduleWithResponse(messageContent, graphUser));
            }

            var interviewerInviteResponseInfos = await Task.WhenAll(updateScheduleResponseTasks);
            webNotificationsTask = this.SendInviteStatusUpdateWebNotificationsAsync(interviewerInviteResponseInfos);
            emailTasks = new List<Task>();
            foreach (var interviewerInviteResponseInfo in interviewerInviteResponseInfos)
            {
                if (interviewerInviteResponseInfo.ResponseNotification != null && interviewerInviteResponseInfo.ResponseNotification.ResponseStatus != InvitationResponseStatus.Accepted)
                {
                    emailTasks.Add(this.emailManager.SendDeclineEmailToScheduler(interviewerInviteResponseInfo.ResponseNotification, interviewerInviteResponseInfo.InterviewerMessage));
                }
            }

            await Task.WhenAll(emailTasks).ConfigureAwait(false);
            await webNotificationsTask.ConfigureAwait(false);
            this.logger.LogInformation($"Finished {nameof(this.ProcessNotificationContent)} method in {nameof(NotificationManager)}.");
            return true;
        }

        /// <summary>
        /// Process user responses for schedule
        /// </summary>
        /// <param name="scheduleId">Schedule Id content</param>
        /// <returns>Task for processing pending requests</returns>
        public async Task<bool> ProcessScheduleResponse(string scheduleId)
        {
            this.logger.LogInformation($"Started {nameof(this.ProcessScheduleResponse)} method in {nameof(NotificationManager)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new InvalidRequestDataValidationException("Schedule ID should not be null").EnsureLogged(this.logger);
            }

            this.logger.LogInformation("ProcessScheduleResponse: Invoked for scheduleId: " + scheduleId);
            var schedule = await this.scheduleQuery.GetScheduleByScheduleId(scheduleId);
            if (!string.IsNullOrEmpty(schedule?.MeetingDetails[0].CalendarEventId))
            {
                var token = await this.emailClient.GetServiceAccountTokenByEmail(schedule.MeetingDetails[0].SchedulerAccountEmail);

                this.logger.LogInformation($"ProcessScheduleResponse: get response from graph for calendareventId {schedule.MeetingDetails[0].CalendarEventId}");
                var graphMessage = this.outlookProvider.GetEventById("Users/" + schedule.MeetingDetails[0].SchedulerAccountEmail + "/Events/" + schedule.MeetingDetails[0].CalendarEventId, token, schedule.MeetingDetails[0].SchedulerAccountEmail);

                List<Microsoft.Graph.User> attendees = new List<Microsoft.Graph.User>();
                if (graphMessage?.Result != null && graphMessage.Result.Attendees.Any())
                {
                    foreach (var item in graphMessage.Result.Attendees)
                    {
                        string attendeeAddress = item.EmailAddress.Address;
                        ////Pass the sender or the from address to graph along with the service account for which the token has to be generated
                        var graphResponse = await this.outlookProvider.SearchUserByEmail(attendeeAddress, schedule.MeetingDetails[0].SchedulerAccountEmail).ConfigureAwait(false);
                        var graphUser = graphResponse?.Users?.FirstOrDefault();
                        if (graphUser != null)
                        {
                            attendees.Add(graphUser);
                        }
                    }
                }

                this.logger.LogInformation($"ProcessScheduleResponse: process response for calendareventId {schedule.MeetingDetails[0].CalendarEventId}");
                var interviewerResponseNotification = await this.scheduleQuery.UpdateScheduleWithCalendatEventResponse(graphMessage?.Result, attendees);
                if (interviewerResponseNotification != null)
                {
                    await this.emailManager.SendDeclineEmailToScheduler(interviewerResponseNotification);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.ProcessScheduleResponse)} method in {nameof(NotificationManager)}.");
            return true;
        }

        /// <summary>
        /// Process schedule responses for recovering responses lost during disaster.
        /// </summary>
        /// <returns>Task for processing pending requests</returns>
        public async Task ProcessScheduleResponse()
        {
            this.logger.LogInformation($"Started {nameof(this.ProcessScheduleResponse)} method in {nameof(NotificationManager)}.");

            // Get the pending schedules for checking response status.
            var schedules = await this.scheduleQuery.GetPendingSchedules();

            if (schedules != null && schedules.Any())
            {
                foreach (var jobApplicationSchedule in schedules)
                {
                    await this.ProcessScheduleResponse(jobApplicationSchedule.ScheduleID);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.ProcessScheduleResponse)} method in {nameof(NotificationManager)}.");
        }

        /// <summary>
        /// Sends the invite status update web notifications asynchronously.
        /// </summary>
        /// <param name="interviewerInviteResponseInfos">The array of <see cref="InterviewerInviteResponseInfo"/>.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        private async Task SendInviteStatusUpdateWebNotificationsAsync(InterviewerInviteResponseInfo[] interviewerInviteResponseInfos)
        {
            if (WebNotificationServiceFlight.IsEnabled && interviewerInviteResponseInfos.Length > 0)
            {
                try
                {
                    IWebNotificationDataExtractor notificationDataExtractor = this.webNotificationInternalsProvider.GetInviteStatusUpdateWebNotificationDataExtractor(interviewerInviteResponseInfos);
                    IWebNotificationTemplateProvider webNotificationTemplateProvider = this.webNotificationInternalsProvider.GetInviteStatusUpdateWebNotificationTemplateProvider();
                    IEnumerable<WebNotificationRequest> notificationRequests = await this.webNotificationBuilderClient.Builder.Build(notificationDataExtractor, webNotificationTemplateProvider).ConfigureAwait(false);
                    await this.webNotificationBuilderClient.Client.PostNotificationsAsync(notificationRequests).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Do not rethrow exception as the event notification failure does not translate as invoking API endpoint failure.
                    this.logger.LogError(ex, "The web notifications not completed successfully.");
                }
            }
        }
    }
}
