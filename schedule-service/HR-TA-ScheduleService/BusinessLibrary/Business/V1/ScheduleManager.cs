//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Business.V1
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using HR.TA.Common.Base.Security;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.Base.Utilities;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.CommonDataService.Common.Internal;
    using HR.TA.ScheduleService.BusinessLibrary.Configurations;
    using HR.TA.ScheduleService.BusinessLibrary.Exceptions;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.BusinessLibrary.NotifyCandidate;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.BusinessLibrary.Strings;
    using HR.TA.ScheduleService.BusinessLibrary.Utils;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.Enum;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.Data.Helper;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.Talent.EnumSetModel;
    using HR.TA.Talent.EnumSetModel.SchedulingService;
    using HR.TA.Talent.TalentContracts.ScheduleService;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.Common.Web.Contracts;
    using HR.TA.TalentEntities.Enum;
    using TimeZoneConverter;
    using HR.TA.ScheduleService.Contracts.V1.Flights;

    /// <summary>
    /// ScheduleManager implementation
    /// </summary>
    public class ScheduleManager : IScheduleManager
    {
        private readonly IEmailHelper emailHelper;

        private readonly IScheduleQuery scheduleQuery;

        /// <summary>Falcon query client.</summary>
        private readonly FalconQuery falconQuery;

        private readonly IServiceBusHelper serviceBusHelper;

        private readonly IGraphSubscriptionManager graphSubscriptionManager;

        private readonly IEmailClient emailClient;

        private readonly INotificationClient notificationClient;

        private readonly IHCMServiceContext serviceContext;

        /// <summary>
        /// The internals provider
        /// </summary>
        private readonly IInternalsProvider internalsProvider;

        private readonly IUserDetailsProvider userDetailsProvider;

        /// <summary>
        /// Defines the duration of a slot
        /// </summary>
        private readonly int slotDurationInMinutes = 15;

        private readonly string candidateFreeBusyTimeId = "candidate";

        private readonly string freeAvailability = "free";

        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// The instance for <see cref="ILogger{ScheduleManager}"/>.
        /// </summary>
        private readonly ILogger<ScheduleManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleManager"/> class
        /// </summary>
        /// <param name="serviceContext">The instance for <see cref="IHCMServiceContext"/>.</param>
        /// <param name="outlookProvider">outlook provider</param>
        /// <param name="query">schedule query instance</param>
        /// <param name="falconQuery">The falcon query client</param>
        /// <param name="serviceBus">serviceBus helper instance</param>
        /// <param name="graphSubscriptionManager">graph Subscription Manager</param>
        /// <param name="emailClient">Email client</param>
        /// <param name="notificationClient">notification client</param>
        /// <param name="email">email helper</param>
        /// <param name="internalsProvider">The instance for <see cref="IInternalsProvider"/>.</param>
        /// <param name="userDetailsProvider">The instance for <see cref="IUserDetailsProvider"/>.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="logger">The instance for <see cref="ILogger{ScheduleManager}"/>.</param>
        public ScheduleManager(
            IHCMServiceContext serviceContext,
            IOutlookProvider outlookProvider,
            IScheduleQuery query,
            FalconQuery falconQuery,
            IServiceBusHelper serviceBus,
            IGraphSubscriptionManager graphSubscriptionManager,
            IEmailClient emailClient,
            INotificationClient notificationClient,
            IEmailHelper email,
            IInternalsProvider internalsProvider,
            IUserDetailsProvider userDetailsProvider,
            IConfigurationManager configurationManager,
            ILogger<ScheduleManager> logger)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.OutlookProvider = outlookProvider;
            this.scheduleQuery = query;
            this.falconQuery = falconQuery;
            this.serviceBusHelper = serviceBus;
            this.graphSubscriptionManager = graphSubscriptionManager;
            this.notificationClient = notificationClient;
            this.internalsProvider = internalsProvider;
            this.userDetailsProvider = userDetailsProvider;
            this.configurationManager = configurationManager;
            this.emailHelper = email;
            this.serviceContext = serviceContext;
        }

        /// <summary>
        /// Gets or sets outlook Provider implementation
        /// </summary>
        public IOutlookProvider OutlookProvider { get; set; }

        /// <summary>
        /// Sends the interview schedule to applicant.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        /// <exception cref="BusinessRuleViolationException">The schedue is already shared.</exception>
        public async Task<bool> ValidateApplicationByApplicationId(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.ValidateApplicationByApplicationId)} method in {nameof(ScheduleManager)}.");
            Contract.CheckNonWhitespace(jobApplicationId, nameof(jobApplicationId));
            var jobApplication = await this.scheduleQuery.GetJobApplicationWithStatus(jobApplicationId);
            if (jobApplication != null)
            {
                if (jobApplication.Status == JobApplicationStatus.Closed)
                {
                    this.logger.LogWarning($"The job application {jobApplication?.JobApplicationID} is dispositioned in iCIMS , you can not perform this action !");
                    throw new BusinessRuleViolationException($"This candidate is dispositioned in iCIMS , you can not perform this action !").EnsureLogged(this.logger);
                }

                if (jobApplication.Status == JobApplicationStatus.Offered)
                {
                    this.logger.LogWarning($"The job application {jobApplication?.JobApplicationID} is offered in iCIMS , you can not perform this action !");
                    throw new BusinessRuleViolationException($"This candidate is offered in iCIMS , you can not perform this action !").EnsureLogged(this.logger);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.ValidateApplicationByApplicationId)} method in {nameof(ScheduleManager)}.");
            return true;
        }

        /// <summary>
        /// Is Valid Application By ScheduleID
        /// </summary>
        /// <param name="scheduleID">The job application identifier.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        /// <exception cref="BusinessRuleViolationException">The schedue is already shared.</exception>
        public async Task<bool> ValidateApplicationByScheduleId(string scheduleID)
        {
            this.logger.LogInformation($"Started {nameof(this.ValidateApplicationByScheduleId)} method in {nameof(ScheduleManager)}.");

            // All bussiness rules need to have in bussiness layer
            var applicationID = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID);
            if (applicationID != null)
            {
                // For the case of CT, this would return null.
                // Which doesn't affect the logic.
                var jobApplicationStatus = await this.scheduleQuery.GetJobApplicationWithStatus(applicationID);
                if (jobApplicationStatus != null)
                {
                    if (jobApplicationStatus.Status == JobApplicationStatus.Closed)
                    {
                        this.logger.LogWarning($"The job application {jobApplicationStatus?.JobApplicationID} is dispositioned in iCIMS , you can not perform this action !");
                        throw new BusinessRuleViolationException($"This candidate is dispositioned in iCIMS , you can not perform this action !").EnsureLogged(this.logger);
                    }

                    if (jobApplicationStatus.Status == JobApplicationStatus.Offered)
                    {
                        this.logger.LogInformation($"The job application {jobApplicationStatus?.JobApplicationID} is Offered in iCIMS , you can not perform this action !");
                        throw new BusinessRuleViolationException($"This candidate is offered in iCIMS , you can not perform this action !").EnsureLogged(this.logger);
                    }
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.ValidateApplicationByScheduleId)} method in {nameof(ScheduleManager)}.");
            return true;
        }

        /// <summary>
        /// V2: Gets a list of free busy schedule.
        /// </summary>
        /// <param name="freeBusyRequest">The free time request information.</param>
        /// <returns>List of Meeting infos.</returns>
        public async Task<List<MeetingInfo>> GetFreeBusySchedule(FreeBusyRequest freeBusyRequest)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetFreeBusySchedule)} method in {nameof(ScheduleManager)}.");
            if (freeBusyRequest == null)
            {
                this.logger.LogInformation("GetFreeBusySchedule: freeBusyRequest cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid free busy schedule info").EnsureTraced();
            }

            var principal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();
            var uberLevelUserGroup = new UserGroup
            {
                FreeBusyTimeId = "UberUserGroup",
                Users = new List<GraphPerson>()
            };

            foreach (var userGroup in freeBusyRequest.UserGroups)
            {
                if (userGroup?.Users != null)
                {
                    uberLevelUserGroup.Users = uberLevelUserGroup.Users.Concat(userGroup.Users).ToList();
                }
            }

            uberLevelUserGroup.Users = uberLevelUserGroup.Users.Distinct().ToList();

            try
            {
                this.logger.LogInformation("GetFreeBusySchedule: User group count : {0}", freeBusyRequest?.UserGroups?.Count);
                meetingInfos = await this.GetFreeBusyInternal(uberLevelUserGroup, freeBusyRequest, principal?.EncryptedUserToken, principal);
            }
            catch (Exception ex)
            {
                throw new SchedulerProcessingException($"GetFreeBusySchedule: Issue while processing schedule, error: {ex.Message}, stacktrace:{ex.StackTrace}, innerException:{ex.InnerException}").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.GetFreeBusySchedule)} method in {nameof(ScheduleManager)}.");
            return meetingInfos;
        }

        /// <summary>
        /// Send Calendar Event
        /// </summary>
        /// <param name="scheduleIds">schedule id to be processed</param>
        /// <param name="serviceAccountName">Service Account Name</param>
        /// <returns>Calendar Event reponse</returns>
        public async Task<IList<string>> SendCalendarEvent(IList<string> scheduleIds, string serviceAccountName)
        {
            this.logger.LogInformation($"Started {nameof(this.SendCalendarEvent)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(serviceAccountName))
            {
                this.logger.LogInformation("SendCalendarEvent: ServiceAccountName cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid service account").EnsureTraced();
            }

            if (!scheduleIds?.Any() ?? true)
            {
                this.logger.LogInformation("SendCalendarEvent: ScheduleIds cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule Id").EnsureTraced();
            }

            var scheduleIdCalendarEventIdPair = new Dictionary<string, CalendarEvent>();
            var processedScheduleIds = new List<string>();
            var meetingInfoDetails = await this.scheduleQuery.GetScheduleByScheduleIds(scheduleIds);

            foreach (var meetingInfo in meetingInfoDetails)
            {
                var calendarEventRequest = await this.MeetingDetailsToCalendarEvent(meetingInfo);

                try
                {
                    if (meetingInfo.ScheduleStatus == ScheduleStatus.Sent && calendarEventRequest.Id != null)
                    {
                        this.logger.LogInformation($"SendCalendarEvent: Skipping send calendar invite for scheduleId: {meetingInfo?.Id}. As invite is sent already.");
                        processedScheduleIds.Add(meetingInfo.Id);
                    }
                    else if (meetingInfo.ScheduleStatus == ScheduleStatus.Delete)
                    {
                        await this.OutlookProvider.DeleteCalendarEvent(meetingInfo.MeetingDetails[0].SchedulerAccountEmail, calendarEventRequest.Id);
                        this.logger.LogInformation($"SendCalendarEvent: Successfully sent calendar invite for scheduleId: {meetingInfo?.Id}");
                        processedScheduleIds.Add(meetingInfo.Id);
                    }
                    else if (calendarEventRequest.Id != null && !string.IsNullOrEmpty(meetingInfo.MeetingDetails[0].SchedulerAccountEmail))
                    {
                        var calendarEvent = await this.OutlookProvider.SendPatchEvent(meetingInfo.MeetingDetails[0].SchedulerAccountEmail, calendarEventRequest);
                        this.logger.LogInformation($"SendCalendarEvent: Successfully sent calendar invite for scheduleId: {meetingInfo?.Id}");
                        ////Sending update with the e-mail account that was used in the first invite.
                        serviceAccountName = meetingInfo.MeetingDetails[0].SchedulerAccountEmail;
                        scheduleIdCalendarEventIdPair.Add(meetingInfo.Id, calendarEvent);
                    }
                    else if (meetingInfo.ScheduleStatus != ScheduleStatus.Sent)
                    {
                        var calendarEvent = await this.OutlookProvider.SendPostEvent(serviceAccountName, calendarEventRequest);
                        this.logger.LogInformation($"SendCalendarEvent: Successfully sent calendar invite for scheduleId: {meetingInfo?.Id}");
                        scheduleIdCalendarEventIdPair.Add(meetingInfo.Id, calendarEvent);
                    }
                    else
                    {
                        this.logger.LogInformation($"SendCalendarEvent: Already calendar invite sent for scheduleId: {meetingInfo?.Id}");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError($"SendCalendarEvent: Failed to send calendar request for scheduleId: {meetingInfo?.Id}, StackTrace: {ex.StackTrace}, ErrorMessage: {ex.Message}, InnerException: {ex.InnerException}");
                }
            }

            if (scheduleIdCalendarEventIdPair != null && scheduleIdCalendarEventIdPair.Count() > 0)
            {
                await this.scheduleQuery.UpdateJobApplicationScheduleDetails(scheduleIdCalendarEventIdPair, ScheduleStatus.Sent, serviceAccountName).ConfigureAwait(false);
            }

            processedScheduleIds.AddRange(scheduleIdCalendarEventIdPair?.Select(a => a.Key).ToList());
            this.logger.LogInformation($"Finished {nameof(this.SendCalendarEvent)} method in {nameof(ScheduleManager)}.");
            return processedScheduleIds;
        }

        /// <summary>
        /// Resend schedule
        /// </summary>
        /// <param name="scheduleId">schedule id</param>
        /// <param name="serviceAccountName">service account</param>
        /// <returns>result</returns>
        public async Task<bool> ReSendSchedule(string scheduleId, string serviceAccountName = null)
        {
            if (string.IsNullOrWhiteSpace(scheduleId))
            {
                this.logger.LogInformation($"{nameof(this.ReSendSchedule)} method in {nameof(ScheduleManager)}: ScheduleID cannot be null");
                throw new InvalidRequestDataValidationException($"{nameof(this.ReSendSchedule)} method in {nameof(ScheduleManager)}: Invalid ScheduleID").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Started {nameof(this.ReSendSchedule)} method in {nameof(ScheduleManager)}.");
            MeetingInfo meetingInfo = await this.scheduleQuery.GetScheduleByScheduleId(scheduleId);
            var scheduleIdCalendarEventIdPair = new Dictionary<string, CalendarEvent>();

            if (meetingInfo.ScheduleStatus == ScheduleStatus.Queued || meetingInfo.ScheduleStatus == ScheduleStatus.Saved)
            {
                string servAccName = string.Empty;

                if (string.IsNullOrWhiteSpace(serviceAccountName))
                {
                    servAccName = this.configurationManager.Get<SchedulerConfiguration>()?.EmailAddress;
                }
                else
                {
                    servAccName = serviceAccountName;
                }

                var calendarEventRequest = await this.MeetingDetailsToCalendarEvent(meetingInfo);
                try
                {
                    if (calendarEventRequest.Id != null && !string.IsNullOrWhiteSpace(meetingInfo.MeetingDetails[0].SchedulerAccountEmail))
                    {
                        var calendarEvent = await this.OutlookProvider.SendPatchEvent(meetingInfo.MeetingDetails[0].SchedulerAccountEmail, calendarEventRequest);
                        this.logger.LogInformation($"ReSendSchedule: Successfully sent calendar invite for scheduleId: {meetingInfo?.Id}");
                        ////Sending update with the e-mail account that was used in the first invite.
                        servAccName = meetingInfo.MeetingDetails[0].SchedulerAccountEmail;

                        scheduleIdCalendarEventIdPair.Add(meetingInfo.Id, calendarEvent);
                    }
                    else
                    {
                        var calendarEvent = await this.OutlookProvider.SendPostEvent(servAccName, calendarEventRequest);
                        this.logger.LogInformation($"ReSendSchedule: Successfully sent calendar invite for scheduleId: {meetingInfo?.Id}");
                        scheduleIdCalendarEventIdPair.Add(meetingInfo.Id, calendarEvent);
                        await this.scheduleQuery.UpdateScheduleServiceAccount(scheduleId, servAccName);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError($"ReSendSchedule: Failed to send calendar request for scheduleId: {meetingInfo?.Id}, stackTrace: {ex.StackTrace}");
                    return false;
                }

                if (scheduleIdCalendarEventIdPair.Any())
                {
                    await this.scheduleQuery.UpdateJobApplicationScheduleDetails(scheduleIdCalendarEventIdPair, ScheduleStatus.Sent, servAccName).ConfigureAwait(false);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.ReSendSchedule)} method in {nameof(ScheduleManager)}.");
            return true;
        }

        /// <summary>
        /// Gets a list of free busy schedule .
        /// </summary>
        /// <param name="freeBusyRequest">The free time request information.</param>
        /// <param name="jobApplicationId">jobApplicationId</param>
        /// <returns>The free meeting time response.</returns>
        public async Task<List<MeetingInfo>> GetSchedulesByFreeBusyRequest(FreeBusyRequest freeBusyRequest, string jobApplicationId)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleManager)}.");
            if (freeBusyRequest == null)
            {
                this.logger.LogInformation($"{nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleManager)}: freeBusyRequest cannot be null");
                throw new InvalidRequestDataValidationException($"Input request does not contain a valid free busy info").EnsureLogged(this.logger);
            }

            var uberLevelUserGroup = new UserGroup
            {
                FreeBusyTimeId = "UberUserGroup",
                Users = new List<GraphPerson>()
            };

            foreach (var userGroup in freeBusyRequest.UserGroups)
            {
                if (userGroup?.Users != null)
                {
                    uberLevelUserGroup.Users = uberLevelUserGroup.Users.Concat(userGroup.Users).ToList();
                }
            }

            uberLevelUserGroup.Users = uberLevelUserGroup.Users.Distinct().ToList();

            var requestFreeBusy = this.GenerateFreeBusyScheduleRequest(freeBusyRequest, uberLevelUserGroup.Users);
            meetingInfos = await this.PopulateUserDetailsInMeetingInfo(await this.scheduleQuery.GetSchedules(requestFreeBusy, jobApplicationId));

            this.logger.LogInformation($"Finished {nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleManager)}.");
            return meetingInfos;
        }

        /// <summary>
        /// Create schedule
        /// </summary>
        /// <param name="schedule">Schedule object</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> CreateSchedule(MeetingInfo schedule, string jobApplicationId)
        {
            MeetingInfo meetingInfo;
            this.logger.LogInformation($"Started {nameof(this.CreateSchedule)} method in {nameof(ScheduleManager)}.");
            if (schedule == null)
            {
                this.logger.LogInformation("CreateSchedule: Schedule  cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule").EnsureTraced();
            }

            meetingInfo = this.PopulateUserDetailsInMeetingInfo(await this.scheduleQuery.CreateSchedule(schedule, jobApplicationId));
            this.logger.LogInformation($"CreateSchedule: Starting to send message to connector for {jobApplicationId}");
            ConnectorIntegrationDetails syncBackDetails = new ConnectorIntegrationDetails();
            syncBackDetails.ScheduleAction = ActionType.Create;
            syncBackDetails.ScheduleID = meetingInfo.Id;
            syncBackDetails.JobApplicationId = jobApplicationId;
            var rootActivityId = this.serviceContext.RootActivityId;
            await this.serviceBusHelper.QueueMessageToConnector("InterviewScheduling", syncBackDetails, rootActivityId, jobApplicationId);
            if (PublishScheduleFlight.IsEnabled)
            {
                await this.serviceBusHelper.QueueMessageToConnector("MSInterviewSchedule", syncBackDetails, rootActivityId, jobApplicationId);
            }

            this.logger.LogInformation("CreateSchedule: Ending to send message to connector.");
            this.logger.LogInformation($"Finished {nameof(this.CreateSchedule)} method in {nameof(ScheduleManager)}.");
            return meetingInfo;
        }

        /// <summary>
        /// Update schedule
        /// </summary>
        /// <param name="schedule">Schedule object</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> UpdateSchedule(MeetingInfo schedule)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateSchedule)} method in {nameof(ScheduleManager)}.");
            if (schedule == null || string.IsNullOrEmpty(schedule.Id))
            {
                this.logger.LogInformation("UpdateSchedule: Schedule  cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule").EnsureTraced();
            }

            var scheduleDetails = await this.scheduleQuery.GetScheduleByScheduleId(schedule.Id);

            MeetingInfo result = new MeetingInfo();

            if (scheduleDetails == null)
            {
                this.logger.LogError("UpdateSchedule: Schedule  object is not avaialble");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule").EnsureTraced();
            }

            var jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(schedule.Id);

            ////descption, subject, location, schedule status and meeting details.
            result = await this.scheduleQuery.UpdateScheduleDetails(schedule);

            result = this.PopulateUserDetailsInMeetingInfo(result);
            if (PublishScheduleFlight.IsEnabled)
            {
                ConnectorIntegrationDetails syncBackDetails = new ConnectorIntegrationDetails();
                syncBackDetails.ScheduleAction = ActionType.Update;
                syncBackDetails.ScheduleID = schedule.Id;
                var rootActivityId = this.serviceContext.RootActivityId;
                await this.serviceBusHelper.QueueMessageToConnector("MSInterviewSchedule", syncBackDetails, rootActivityId, jobApplicationId);
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateSchedule)} method in {nameof(ScheduleManager)}.");
            return result;
        }

        /// <summary>
        /// Update schedule
        /// </summary>
        /// <param name="scheduleID">Schedule identifier</param>
        /// <param name="participantOid">participant office graph identifier</param>
        /// <param name="responseStatus">response</param>
        /// <returns>The task response</returns>
        public async Task<bool> UpdateScheduleParticipantResponse(string scheduleID, string participantOid, InvitationResponseStatus responseStatus)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleParticipantResponse)} method in {nameof(ScheduleManager)}.");

            await this.scheduleQuery.UpdateScheduleParticipantResponseManualAsync(scheduleID, participantOid, responseStatus);

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleParticipantResponse)} method in {nameof(ScheduleManager)}.");
            return true;
        }

        /// <summary>
        /// Get schedule By Job ID
        /// </summary>
        /// <param name="jobApplicationId">jobApplicationId</param>
        /// <returns>The task response</returns>
        public async Task<List<MeetingInfo>> GetSchedulesByJobApplicationId(string jobApplicationId)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetSchedulesByJobApplicationId)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(jobApplicationId))
            {
                this.logger.LogWarning("GetSchedulesByJobApplicationId: JobApplicationId  cannot be null");
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureTraced();
            }

            this.logger.LogInformation("GetSchedulesByJobApplicationId: Getting Schedule details for scheduleId: {0}", jobApplicationId);

            meetingInfos = await this.PopulateUserDetailsInMeetingInfo(await this.scheduleQuery.GetSchedulesByJobApplicationId(jobApplicationId));
            this.logger.LogInformation($"Finished {nameof(this.GetSchedulesByJobApplicationId)} method in {nameof(ScheduleManager)}.");
            return meetingInfos;
        }

        /// <summary>
        /// Get schedule By Schedule ID
        /// </summary>
        /// <param name="scheduleID">Schedule id</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> GetScheduleByScheduleId(string scheduleID)
        {
            this.logger.LogInformation($"Started {nameof(this.GetScheduleByScheduleId)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(scheduleID))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid schedule id").EnsureTraced();
            }

            this.logger.LogInformation("GetSchedulesByScheduleId: Getting Schedule details for scheduleId: {0}", scheduleID);

            var meetingInfo = this.PopulateUserDetailsInMeetingInfo(await this.scheduleQuery.GetScheduleByScheduleId(scheduleID));

            this.logger.LogInformation($"Finished {nameof(this.GetScheduleByScheduleId)} method in {nameof(ScheduleManager)}.");
            return meetingInfo;
        }

        /// <summary>
        /// Queue schedule
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="status">Schedule status</param>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="rootActivityID">root ActivityID</param>
        /// <param name="contextUserId">Current Context User Office Graph Identifier</param>
        /// <param name="requesterEmail">Email Id of the requester</param>
        /// <param name="isWobUser">Status of WOB Authentication the current user</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> QueueSchedule(string scheduleID, ScheduleStatus status, EmailTemplate emailTemplate, string rootActivityID, string contextUserId = null, string requesterEmail = null, bool isWobUser = false)
        {
            MeetingInfo result;
            this.logger.LogInformation($"Started {nameof(this.QueueSchedule)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(scheduleID))
            {
                this.logger.LogInformation("QueueSchedule: ScheduleId cannot be null");
                throw new BusinessRuleViolationException($"Input request does not contain a valid schedule").EnsureLogged(this.logger);
            }

            this.logger.LogInformation("QueueSchedule: Updating Schedule details for scheduleId: {0}", scheduleID);

            // All bussiness rules need to have in bussiness layer and executed first , we should move from query layer
            await this.ValidateApplicationByScheduleId(scheduleID);

            if (isWobUser)
            {
                bool isUserPresent = (emailTemplate.PrimaryEmailRecipients != null ? emailTemplate.PrimaryEmailRecipients.Contains(requesterEmail, StringComparer.OrdinalIgnoreCase) : false) || (emailTemplate.CcEmailAddressList != null ? emailTemplate.CcEmailAddressList.Contains(requesterEmail, StringComparer.OrdinalIgnoreCase) : false);
                if (isUserPresent)
                {
                    this.logger.LogInformation($"The WOB parent user is present in the recipient list. Adding WOB Participants in CC.");

                    var wobUsers = await this.scheduleQuery.GetWobUsersDelegation(contextUserId).ConfigureAwait(false);
                    emailTemplate.CcEmailAddressList.AddRange(wobUsers.Select(wob => wob.To.EmailPrimary));
                    this.logger.LogInformation($"Added {wobUsers.Count} WOB Participants in CC.");
                }
            }

            result = await this.scheduleQuery.UpdateScheduleEmailStatus(scheduleID, status, emailTemplate);

            if (result.ScheduleStatus == status)
            {
                ConnectorIntegrationDetails connectorIntegrationDetails = new ConnectorIntegrationDetails();
                connectorIntegrationDetails.ScheduleAction = ActionType.Update;
                connectorIntegrationDetails.ScheduleID = scheduleID;
                await this.serviceBusHelper.QueueMessageToSendInvitationWorker(ActionType.Update.ToString(), connectorIntegrationDetails, rootActivityID, scheduleID);
                if (PublishScheduleFlight.IsEnabled)
                {
                    ConnectorIntegrationDetails syncBackDetails = new ConnectorIntegrationDetails();
                    syncBackDetails.ScheduleAction = ActionType.Update;
                    syncBackDetails.ScheduleID = scheduleID;
                    var rootActivityId = this.serviceContext.RootActivityId;
                    var jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID).ConfigureAwait(false);
                    await this.serviceBusHelper.QueueMessageToConnector("MSInterviewSchedule", syncBackDetails, rootActivityId, jobApplicationId);
                }
            }

            result = this.PopulateUserDetailsInMeetingInfo(result);
            this.logger.LogInformation($"Finished {nameof(this.QueueSchedule)} method in {nameof(ScheduleManager)}.");
            return result;
        }

        /// <summary>
        /// Update schedule status
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="status">Schedule status</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> UpdateScheduleStatus(string scheduleID, ScheduleStatus status)
        {
            MeetingInfo meetingInfo;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleStatus)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(scheduleID))
            {
                this.logger.LogInformation("UpdateScheduleStatus: Schedule  cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule id").EnsureTraced();
            }

            this.logger.LogInformation("UpdateScheduleStatus: Updating Schedule details for scheduleId: {0}", scheduleID);

            meetingInfo = this.PopulateUserDetailsInMeetingInfo(await this.scheduleQuery.UpdateScheduleStatus(scheduleID, status));
            if (PublishScheduleFlight.IsEnabled)
            {
                ConnectorIntegrationDetails syncBackDetails = new ConnectorIntegrationDetails();
                syncBackDetails.ScheduleAction = ActionType.Update;
                syncBackDetails.ScheduleID = scheduleID;
                var rootActivityId = this.serviceContext.RootActivityId;
                var jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID).ConfigureAwait(false);
                await this.serviceBusHelper.QueueMessageToConnector("MSInterviewSchedule", syncBackDetails, rootActivityId, jobApplicationId);
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleStatus)} method in {nameof(ScheduleManager)}.");
            return meetingInfo;
        }

        /// <summary>
        /// Update schedule service account
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="serviceAccountName">service Account Name</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> UpdateScheduleServiceAccountAsync(string scheduleID, string serviceAccountName)
        {
            MeetingInfo meetingInfo;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleServiceAccountAsync)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(scheduleID) || string.IsNullOrEmpty(serviceAccountName))
            {
                this.logger.LogInformation("UpdateScheduleServiceAccountAsync: Schedule or ServiceAccountName cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid service account").EnsureTraced();
            }

            this.logger.LogInformation("UpdateScheduleServiceAccountAsync: Updating Schedule service account details for scheduleId: {0}", scheduleID);

            meetingInfo = this.PopulateUserDetailsInMeetingInfo(await this.scheduleQuery.UpdateScheduleServiceAccount(scheduleID, serviceAccountName));

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleServiceAccountAsync)} method in {nameof(ScheduleManager)}.");
            return meetingInfo;
        }

        /// <summary>
        /// Delete schedule
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="rootActivityID">root ActivityID</param>
        /// <returns>The task response</returns>
        public async Task DeleteSchedule(string scheduleID, string rootActivityID)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteSchedule)} method in {nameof(ScheduleManager)}.");
            if (string.IsNullOrEmpty(scheduleID))
            {
                this.logger.LogInformation("DeleteSchedule: ScheduleId  cannot be null");
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule id").EnsureTraced();
            }

            this.logger.LogInformation("DeleteSchedule: Delete Schedule for scheduleId: {0}", scheduleID);

            ////Change the schedule status to delete
            var scheduleDetails = await this.scheduleQuery.UpdateScheduleStatus(scheduleID, ScheduleStatus.Delete);

            if (scheduleDetails != null && scheduleDetails.MeetingDetails?[0].CalendarEventId != null)
            {
                this.logger.LogInformation("DeleteSchedule:  Queue the message of scheduleId: {0} for cancel notification", scheduleID);

                ////Queue the message in the case of e-mail notifications triggered to participants
                ConnectorIntegrationDetails connectorIntegrationDetails = new ConnectorIntegrationDetails();
                connectorIntegrationDetails.ScheduleAction = ActionType.Delete;
                connectorIntegrationDetails.ScheduleID = scheduleID;

                await this.serviceBusHelper.QueueMessageToSendInvitationWorker(ActionType.Delete.ToString(), connectorIntegrationDetails, rootActivityID, scheduleID);
                if (PublishScheduleFlight.IsEnabled)
                {
                    ConnectorIntegrationDetails syncBackDetails = new ConnectorIntegrationDetails();
                    syncBackDetails.ScheduleAction = ActionType.Delete;
                    syncBackDetails.ScheduleID = scheduleID;
                    var rootActivityId = this.serviceContext.RootActivityId;
                    var jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID).ConfigureAwait(false);
                    await this.serviceBusHelper.QueueMessageToConnector("MSInterviewSchedule", syncBackDetails, rootActivityId, jobApplicationId);
                }

                this.logger.LogInformation("DeleteSchedule: Successfully Queued message of scheduleId: {0} for cancel notification", scheduleID);
            }
            else
            {
                this.logger.LogInformation("DeleteSchedule: Schedule " + scheduleID + " object is not avaialble");
            }

            this.logger.LogInformation($"Finished {nameof(this.DeleteSchedule)} method in {nameof(ScheduleManager)}.");
        }

        /// <summary>
        /// Sends the interview schedule to applicant.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest" />.</param>
        /// <param name="requesterEmail">The requester email address.</param>
        /// <param name="requesterOid">The requestor Office Graph Identifier</param>
        /// <param name="isWobUser"> Flag to get the status of the Wob Authentication</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        /// <exception cref="InvalidRequestDataValidationException">The schedue is already shared.</exception>
        public async Task SendInterviewScheduleToApplicantAsync(string jobApplicationId, ScheduleInvitationRequest scheduleInvitationRequest, string requesterEmail, string requesterOid = null, bool isWobUser = false)
        {
            this.logger.LogInformation($"Started {nameof(this.SendInterviewScheduleToApplicantAsync)} method in {nameof(ScheduleManager)}.");
            Contract.CheckNonWhitespace(jobApplicationId, nameof(jobApplicationId));
            Contract.CheckValue(scheduleInvitationRequest, nameof(scheduleInvitationRequest));

            var schedulesTask = this.scheduleQuery.GetSchedulesByJobApplicationId(jobApplicationId);

            ICandidateCommunicator candidateCommunicator = this.internalsProvider.GetCandidateCommunicator(requesterEmail);
            var jobApplication = await this.scheduleQuery.GetJobApplication(jobApplicationId);
            if (jobApplication != null)
            {
                var schedules = await schedulesTask.ConfigureAwait(false);
                if (schedules?.Any() ?? false)
                {
                    if (isWobUser)
                    {
                        bool isUserPresent = scheduleInvitationRequest.CcEmailAddressList != null ? scheduleInvitationRequest.CcEmailAddressList.Contains(requesterEmail, StringComparer.OrdinalIgnoreCase) : false;
                        if (isUserPresent)
                        {
                            this.logger.LogInformation($"The WOB parent user is present in the recipient list. Adding WOB Participants in CC.");

                            var wobUsers = await this.scheduleQuery.GetWobUsersDelegation(requesterOid).ConfigureAwait(false);
                            scheduleInvitationRequest.CcEmailAddressList.AddRange(wobUsers.Select(wob => wob.To.EmailPrimary));
                            this.logger.LogInformation($"Added {wobUsers.Count} WOB Participants in CC.");
                        }
                    }

                    bool responseStatus = await candidateCommunicator.SendInvitationAsync(jobApplication, scheduleInvitationRequest, schedules).ConfigureAwait(false);
                    if (responseStatus)
                    {
                        var updateSharedSchedulesTask = this.UpdateSchedulesSharingStatus(scheduleInvitationRequest);
                        await this.scheduleQuery.UpdateJobApplicationStatusHistoryAsync(jobApplicationId, JobApplicationActionType.SendToCandidate).ConfigureAwait(false);
                        await this.UpdateConnectorIntegration(jobApplicationId, this.serviceContext.RootActivityId).ConfigureAwait(false);
                        jobApplication.IsScheduleSentToCandidate = true;
                        bool updatedJobApplication = await this.scheduleQuery.UpdateJobApplicaton(jobApplication);
                        if (!updatedJobApplication)
                        {
                            this.logger.LogError($"Failed setting the flag of send to candidate {nameof(this.SendInterviewScheduleToApplicantAsync)} method in {nameof(ScheduleManager)}.");
                        }

                        _ = await updateSharedSchedulesTask.ConfigureAwait(false);
                    }
                }
                else
                {
                    throw new BusinessRuleViolationException($"The job application has no interviews schedule associated").EnsureLogged(this.logger);
                }
            }
            else
            {
                throw new BusinessRuleViolationException($"The job application is dispositioned or offered in iCIMS , you can not perform this action !").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendInterviewScheduleToApplicantAsync)} method in {nameof(ScheduleManager)}.");
        }

        /// <summary>
        /// Create or update timezone for job application
        /// </summary>
        /// <param name="jobApplicationId">jobapplicationid</param>
        /// <param name="timezone">timezone information</param>
        /// <returns>task result</returns>
        public async Task UpdateTimezoneForJobApplication(string jobApplicationId, Timezone timezone)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateTimezoneForJobApplication)} method in {nameof(ScheduleManager)}.");

            await this.scheduleQuery.AddOrUpdateTimezoneForJobApplication(jobApplicationId, timezone);

            this.logger.LogInformation($"Finished {nameof(this.UpdateTimezoneForJobApplication)} method in {nameof(ScheduleManager)}.");
        }

        /// <summary>
        /// Get timezone using job application id
        /// </summary>
        /// <param name="jobApplicationId">jobapplicationid</param>
        /// <returns>task result</returns>
        public async Task<Timezone> GetTimezoneByJobApplicationId(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetTimezoneByJobApplicationId)} method in {nameof(ScheduleManager)}.");

            return await this.scheduleQuery.GetTimezoneForJobApplication(jobApplicationId);
        }

        /// <summary>
        /// Get Meeting Suggestions
        /// </summary>
        /// <param name="suggestMeetingsRequest">The meeting suggestion request object.</param>
        /// <returns>List of suggested meetings</returns>
        public async Task<IList<MeetingInfo>> GetMeetingSuggestions(SuggestMeetingsRequest suggestMeetingsRequest)
        {
            this.logger.LogInformation($"Started {nameof(this.GetMeetingSuggestions)} method in {nameof(ScheduleManager)}.");

            if (suggestMeetingsRequest == null)
            {
                throw new InvalidRequestDataValidationException($"Input request does not contain a valid meeting info").EnsureLogged(this.logger);
            }

            var meetingSuggestions = new List<MeetingInfo>();

            if (suggestMeetingsRequest.InterviewStartDateSuggestion < DateTime.UtcNow)
            {
                suggestMeetingsRequest.InterviewStartDateSuggestion = this.TimeRoundUp(DateTime.UtcNow, TimeSpan.FromMinutes(this.slotDurationInMinutes));
            }

            if (suggestMeetingsRequest.PanelInterview)
            {
                var findMeetingTimeResponse = await this.OutlookProvider.FindMeetingTimes(this.CreateFindMeetingTimeRequest(suggestMeetingsRequest));

                if (findMeetingTimeResponse.MeetingTimeSuggestions != null)
                {
                    foreach (var suggestion in findMeetingTimeResponse.MeetingTimeSuggestions)
                    {
                        if (suggestion.AttendeeAvailability?.TrueForAll(a => a.Availability == this.freeAvailability) ?? false)
                        {
                            meetingSuggestions.Add(this.FindMeetingTimeResponseToMeetingInfo(findMeetingTimeResponse, suggestMeetingsRequest));
                            break;
                        }
                    }
                }
            }
            else
            {
                var freeBusyRequest = this.CreateFreeBusyRequest(suggestMeetingsRequest);
                var emptySlots = freeBusyRequest?.UserGroups?.Where(fbr => fbr.Users?.Count == 0)?.ToList();
                freeBusyRequest.UserGroups = freeBusyRequest?.UserGroups?.Where(fbr => fbr.Users?.FirstOrDefault()?.Id != null)?.ToList();

                var freeBusyResponse = await this.GetFreeBusySchedule(freeBusyRequest);

                emptySlots?.ForEach(emptySlot =>
                {
                    freeBusyResponse.Add(this.GetWorkingHoursDetailsForTimeZone(suggestMeetingsRequest, emptySlot));
                });

                if (suggestMeetingsRequest.Candidate?.Id == null)
                {
                    var candidateUserGroup = new UserGroup
                    {
                        FreeBusyTimeId = this.candidateFreeBusyTimeId,
                        Users = new List<GraphPerson> { suggestMeetingsRequest.Candidate },
                    };

                    freeBusyResponse.Add(this.GetWorkingHoursDetailsForTimeZone(suggestMeetingsRequest, candidateUserGroup));
                }

                var suggestions = this.ConstructFreeTimeMap(freeBusyResponse, suggestMeetingsRequest);
                if (suggestions != null)
                {
                    meetingSuggestions.AddRange(suggestions);
                }

                meetingSuggestions = (await this.UpdateLocationDetailsInMeetingInfo(meetingSuggestions).ConfigureAwait(false))?.ToList();
            }

            return meetingSuggestions;
        }

        private async Task<IList<MeetingInfo>> UpdateLocationDetailsInMeetingInfo(IList<MeetingInfo> meetingSuggestions)
        {
            IList<Task<Microsoft.Graph.User>> getUserDetailsTasks = new List<Task<Microsoft.Graph.User>>();
            IList<Microsoft.Graph.User> userDetails = new List<Microsoft.Graph.User>();

            if (meetingSuggestions?.Any() ?? false)
            {
                meetingSuggestions.ForEach(meetingSuggestion =>
                {
                    var oid = meetingSuggestion.UserGroups?.Users?.Select(user => user.Id)?.FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(oid))
                    {
                        getUserDetailsTasks.Add(this.userDetailsProvider.GetUserAsync(oid));
                    }
                });

                userDetails = (await Task.WhenAll(getUserDetailsTasks).ConfigureAwait(false))?
                                .Where(user => user != null && !string.IsNullOrWhiteSpace(user.OfficeLocation))?
                                .ToList();

                meetingSuggestions.ForEach(meetingSuggestion =>
                {
                    var oid = meetingSuggestion.UserGroups?.Users?.Select(u => u.Id)?.FirstOrDefault();
                    var user = userDetails?.FirstOrDefault(ud => ud.Id == oid);
                    var officeLocation = user?.OfficeLocation?.Split('/');

                    if (officeLocation?.Length == 2)
                    {
                        meetingSuggestion.MeetingDetails.FirstOrDefault().MeetingLocation = new InterviewMeetingLocation
                        {
                            RoomList = new Room { Name = officeLocation[0] },
                            Room = new Room { Name = officeLocation[1] },
                        };
                    }
                });
            }

            return meetingSuggestions;
        }

        private IList<MeetingInfo> ConstructFreeTimeMap(IList<MeetingInfo> freeBusyList, SuggestMeetingsRequest suggestMeetingsRequest)
        {
            var freeSlotsMap = new Dictionary<string, List<int>>();
            var candidateFreeSlotsList = new List<int>();

            var meetingDurationInMinutes = int.Parse(suggestMeetingsRequest.MeetingDurationInMinutes);
            var suggestEndToSlot = (int)(suggestMeetingsRequest.InterviewEndDateSuggestion.Subtract(suggestMeetingsRequest.InterviewStartDateSuggestion).TotalMinutes
                / this.slotDurationInMinutes);
            var slotsNeededForMeeting = meetingDurationInMinutes / this.slotDurationInMinutes;

            foreach (var freeBusy in freeBusyList)
            {
                var freeTimeMap = Enumerable.Repeat(true, suggestEndToSlot).ToArray();

                foreach (var meetingDetail in freeBusy.MeetingDetails)
                {
                    if (!(meetingDetail.Status == FreeBusyScheduleStatus.Free ||
                        meetingDetail.Status == FreeBusyScheduleStatus.WorkingElsewhere))
                    {
                        var startMoment = meetingDetail.UtcStart;
                        var endMoment = meetingDetail.UtcEnd;
                        var startMomentToSlotNumber = (int)(startMoment.Subtract(suggestMeetingsRequest.InterviewStartDateSuggestion).TotalMinutes
                            / this.slotDurationInMinutes);
                        var endMomentToSlotNumber = (int)(endMoment.Subtract(suggestMeetingsRequest.InterviewStartDateSuggestion).TotalMinutes
                            / this.slotDurationInMinutes);

                        if (startMomentToSlotNumber >= suggestEndToSlot || endMomentToSlotNumber <= 0)
                        {
                            continue;
                        }

                        var currIndex = Math.Max(startMomentToSlotNumber, 0);
                        var endIndex = Math.Min(endMomentToSlotNumber, suggestEndToSlot);

                        while (currIndex < endIndex)
                        {
                            freeTimeMap[currIndex++] = false;
                        }
                    }
                }

                if (freeBusy.UserGroups?.FreeBusyTimeId?.Equals(this.candidateFreeBusyTimeId, StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    candidateFreeSlotsList = this.GetFreeTimeSlots(suggestMeetingsRequest, freeTimeMap)?.ToList();
                }
                else
                {
                    freeSlotsMap.Add(freeBusy?.UserGroups?.FreeBusyTimeId, this.GetFreeTimeSlots(suggestMeetingsRequest, freeTimeMap)?.ToList());
                }
            }

            freeSlotsMap = freeSlotsMap.OrderBy(pair => pair.Value.Count).ToDictionary(pair => pair.Key, pair => pair.Value);
            return this.GetSuggestedSlots(freeSlotsMap, candidateFreeSlotsList, suggestMeetingsRequest);
        }

        private MeetingInfo GetWorkingHoursDetailsForTimeZone(SuggestMeetingsRequest suggestMeetingsRequest, UserGroup user)
        {
            if (suggestMeetingsRequest == null)
            {
                return null;
            }

            var timezone = TZConvert.IanaToWindows(suggestMeetingsRequest.Timezone.TimezoneName);
            var freeBusyResponse = new FindFreeBusyScheduleResponse
            {
                WorkingHours = new WorkingHours
                {
                    DaysOfWeek = new List<string>
                        {
                            DayOfWeek.Monday.ToString(), DayOfWeek.Tuesday.ToString(), DayOfWeek.Wednesday.ToString(),
                            DayOfWeek.Thursday.ToString(), DayOfWeek.Friday.ToString(),
                        },
                    StartTime = new Microsoft.Graph.TimeOfDay(8, 0, 0),
                    EndTime = new Microsoft.Graph.TimeOfDay(17, 0, 0),
                    TimeZone = new Microsoft.Graph.TimeZoneBase { Name = timezone },
                },
            };

            var meetingDetails = this.GetWorkingHourMeetingDetails(new List<FindFreeBusyScheduleResponse> { freeBusyResponse }, suggestMeetingsRequest.InterviewStartDateSuggestion, suggestMeetingsRequest.InterviewEndDateSuggestion);

            return new MeetingInfo
            {
                InterviewerTimeSlotId = user.FreeBusyTimeId,
                UserGroups = user,
                MeetingDetails = meetingDetails,
            };
        }

        private IList<MeetingInfo> GetSuggestedSlots(Dictionary<string, List<int>> freeSlotsMap, IList<int> candidateFreeSlots, SuggestMeetingsRequest suggestMeetingsRequest)
        {
            IList<MeetingInfo> suggestedMeetingInfos = new List<MeetingInfo>();
            var meetingDurationInMinutes = int.Parse(suggestMeetingsRequest.MeetingDurationInMinutes);
            var slotsNeededForMeeting = meetingDurationInMinutes / this.slotDurationInMinutes;

            foreach (var freeBusyTimeId in freeSlotsMap.Keys)
            {
                var interviewerFreeSlots = freeSlotsMap[freeBusyTimeId].Intersect(candidateFreeSlots)?.ToList();
                if (interviewerFreeSlots?.Any() ?? false)
                {
                    // Scheduling a meeting for first available slot
                    var slot = interviewerFreeSlots[0];
                    suggestedMeetingInfos.Add(this.SuggestionToMeetingInfo(slot, freeBusyTimeId, suggestMeetingsRequest));
                    candidateFreeSlots = candidateFreeSlots?.Where(s => !(s < (slot + slotsNeededForMeeting)
                    && s > (slot - slotsNeededForMeeting)))?.ToList();
                }
            }

            return suggestedMeetingInfos;
        }

        private DateTime SlotIndexToDateTime(int slotIndex, SuggestMeetingsRequest suggestMeetingsRequest)
        {
            var startDateTime = suggestMeetingsRequest.InterviewStartDateSuggestion;

            if (slotIndex < 0)
            {
                return startDateTime;
            }

            return startDateTime.AddMinutes((slotIndex + 1) * this.slotDurationInMinutes);
        }

        private IList<int> GetFreeTimeSlots(SuggestMeetingsRequest suggestMeetingsRequest, bool[] freeTimeMap)
        {
            var meetingDuration = int.Parse(suggestMeetingsRequest.MeetingDurationInMinutes);
            var suggestEndToSlot = (int)(suggestMeetingsRequest.InterviewEndDateSuggestion.Subtract(suggestMeetingsRequest.InterviewStartDateSuggestion).TotalMinutes
                / this.slotDurationInMinutes);

            var freeTimeSlotIndices = new List<int>();

            var slotsNeededForMeeting = meetingDuration / this.slotDurationInMinutes;

            var currentFreeSlots = 0;
            for (var slotIndex = 0; slotIndex < suggestEndToSlot; slotIndex++)
            {
                if (freeTimeMap[slotIndex])
                {
                    currentFreeSlots++;
                    if (currentFreeSlots >= slotsNeededForMeeting)
                    {
                        freeTimeSlotIndices.Add(slotIndex);
                    }
                }
                else
                {
                    currentFreeSlots = 0;
                }
            }

            return freeTimeSlotIndices;
        }

        /// <summary>
        /// Verify the user is Hiring Manger or Contributor or Recruiter
        /// </summary>
        /// <param name="userObjectId">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>user present status</returns>
        private async Task<bool> IsUserHMorRecOrContributor(string userObjectId, string jobApplicationId)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(userObjectId) && !string.IsNullOrWhiteSpace(jobApplicationId))
            {
                try
                {
                    var jobApplicationDetails = await this.scheduleQuery.GetJobApplication(jobApplicationId);
                    if (jobApplicationDetails != null)
                    {
                        IEnumerable<JobApplicationParticipant> participants = jobApplicationDetails.JobApplicationParticipants?.Where(a => a.OID.ToLower().Equals(userObjectId.ToLower()) &&
                        (a.Role == TalentEntities.Enum.JobParticipantRole.HiringManager || a.Role == TalentEntities.Enum.JobParticipantRole.Contributor || a.Role == TalentEntities.Enum.JobParticipantRole.Recruiter));
                        if (participants != null && participants.Count() > 0)
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError("IsUserHMorRecOrContributor failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// Verify the user present in the job application participants list
        /// </summary>
        /// <param name="userObjectId">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>user present status</returns>
        private async Task<bool> IsUserInJobApplicationParticipants(string userObjectId, string jobApplicationId)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(userObjectId) && !string.IsNullOrWhiteSpace(jobApplicationId))
            {
                try
                {
                    var jobApplicationDetails = await this.scheduleQuery.GetJobApplication(jobApplicationId);
                    if (jobApplicationDetails != null)
                    {
                        result = jobApplicationDetails.JobApplicationParticipants.Any(a => a.OID.ToLower().Equals(userObjectId.ToLower()));
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError("IsUserInJobApplicationParticipants failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// Verify if the user has read only admin role
        /// </summary>
        /// <param name="userOid">user object id</param>
        /// <returns>user read only admin status</returns>
        private async Task<bool> IsReadOnlyRole(string userOid)
        {
            if (!string.IsNullOrWhiteSpace(userOid))
            {
                try
                {
                    var roles = await this.falconQuery.GetRoleAssignment(userOid);
                    return roles?.Contains(IVApplicationRole.IVReadOnly) ?? false;
                }
                catch (Exception ex)
                {
                    this.logger.LogError("IsReadOnlyRole failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
                }
            }

            return false;
        }

        /// <summary>
        /// Converting a list of GraphPerson to a list of MeetingAttendee
        /// </summary>
        /// <param name="meetingInfo">Meeting Info</param>
        /// <param name="scheduleMailResult">scheduleMail Result</param>
        /// <returns>List of MeetingAttendee</returns>
        private List<MeetingAttendee> GraphPersonsToMeetingAttendees(MeetingInfo meetingInfo, JobApplicationScheduleMailDetails scheduleMailResult)
        {
            IList<GraphPerson> persons = meetingInfo.UserGroups?.Users;
            var attendees = new List<MeetingAttendee>();
            if (persons != null && persons.Count > 0)
            {
                foreach (var person in persons)
                {
                    attendees.Add(new MeetingAttendee()
                    {
                        EmailAddress = new MeetingAttendeeEmailAddress() { Address = person.Email },
                        Type = SchedulerConstants.AttendeeTypeRequired
                    });
                }
            }

            if (scheduleMailResult != null && scheduleMailResult.CCEmailAddressList?.Count > 0)
            {
                foreach (var person in scheduleMailResult.CCEmailAddressList)
                {
                    attendees.Add(new MeetingAttendee()
                    {
                        EmailAddress = new MeetingAttendeeEmailAddress() { Address = person },

                        // TODO: add optional for bcc list
                        Type = SchedulerConstants.AttendeeTypeOptional
                    });
                }
            }

            return attendees;
        }

        /// <summary>
        /// Populate user full details in meeting info object
        /// </summary>
        /// <param name="meetingInfos">meetingInfo</param>
        /// <returns>meeting info</returns>
        private async Task<List<MeetingInfo>> PopulateUserDetailsInMeetingInfo(List<MeetingInfo> meetingInfos)
        {
            if (meetingInfos != null && meetingInfos.Any())
            {
                List<string> ids = new List<string>();
                foreach (var meetingInfo in meetingInfos)
                {
                    if (meetingInfo.UserGroups?.Users != null)
                    {
                        foreach (var userGroup in meetingInfo.UserGroups.Users)
                        {
                            if (!string.IsNullOrEmpty(userGroup.Id) && !ids.Contains(userGroup.Id))
                            {
                                ids.Add(userGroup.Id.ToLower());
                            }
                        }
                    }

                    if (meetingInfo.MeetingDetails != null)
                    {
                        foreach (var item in meetingInfo.MeetingDetails?[0].Attendees)
                        {
                            if (!string.IsNullOrEmpty(item.User.Id) && !ids.Contains(item.User.Id))
                            {
                                ids.Add(item.User.Id.ToLower());
                            }
                        }
                    }

                    if (meetingInfo.Requester != null)
                    {
                        if (!string.IsNullOrEmpty(meetingInfo.Requester.Id) && !ids.Contains(meetingInfo.Requester.Id))
                        {
                            ids.Add(meetingInfo.Requester.Id.ToLower());
                        }
                    }
                }

                List<Worker> workers = await this.scheduleQuery.GetWorkers(ids);
                if (workers != null)
                {
                    foreach (var meetingInfo in meetingInfos)
                    {
                        if (meetingInfo.UserGroups?.Users != null)
                        {
                            foreach (var userGroup in meetingInfo.UserGroups.Users)
                            {
                                var result = workers.FirstOrDefault(a => a.OfficeGraphIdentifier.ToLower().Equals(userGroup.Id.ToLower()));
                                if (result != null)
                                {
                                    userGroup.Name = result.FullName;
                                    userGroup.Surname = result.Name?.Surname;
                                    userGroup.UserPrincipalName = result.Alias;
                                    userGroup.GivenName = result.Name?.GivenName;
                                    userGroup.Title = result.Profession;
                                }
                            }
                        }

                        if (meetingInfo.MeetingDetails?[0].Attendees != null)
                        {
                            foreach (var item in meetingInfo.MeetingDetails[0].Attendees)
                            {
                                var result = workers.FirstOrDefault(a => a.OfficeGraphIdentifier.ToLower().Equals(item.User?.Id?.ToLower()));
                                if (result != null)
                                {
                                    item.User.Name = result.FullName;
                                    item.User.Surname = result.Name?.Surname;
                                    item.User.UserPrincipalName = result.Alias;
                                    item.User.GivenName = result.Name?.GivenName;
                                    item.User.Title = result.Profession;
                                }
                            }
                        }

                        if (meetingInfo.Requester?.Id != null)
                        {
                            var result = workers.FirstOrDefault(a => a.OfficeGraphIdentifier.ToLower().Equals(meetingInfo.Requester.Id.ToLower()));
                            if (result != null)
                            {
                                meetingInfo.Requester.Name = result.FullName;
                                meetingInfo.Requester.Title = result.Profession;
                                meetingInfo.Requester.UserPrincipalName = result.Alias;
                                meetingInfo.Requester.Surname = result.Name?.Surname;
                                meetingInfo.Requester.GivenName = result.Name?.GivenName;
                            }
                        }
                    }
                }
            }

            return meetingInfos;
        }

        /// <summary>
        /// Populate user full details in meeting info object
        /// </summary>
        /// <param name="meetingInfo">meetingInfo</param>
        /// <returns>meeting info</returns>
        private MeetingInfo PopulateUserDetailsInMeetingInfo(MeetingInfo meetingInfo)
        {
            if (meetingInfo != null)
            {
                List<string> ids = new List<string>();

                if (meetingInfo.Requester != null)
                {
                    if (!string.IsNullOrEmpty(meetingInfo.Requester.Id) && !ids.Contains(meetingInfo.Requester.Id))
                    {
                        ids.Add(meetingInfo.Requester.Id.ToLower());
                    }
                }

                if (meetingInfo.UserGroups?.Users != null)
                {
                    foreach (var userGroup in meetingInfo.UserGroups.Users)
                    {
                        if (!string.IsNullOrEmpty(userGroup.Id) && !ids.Contains(userGroup.Id))
                        {
                            ids.Add(userGroup.Id.ToLower());
                        }
                    }
                }

                if (meetingInfo.MeetingDetails != null)
                {
                    foreach (var item in meetingInfo.MeetingDetails[0].Attendees)
                    {
                        if (!string.IsNullOrEmpty(item.User?.Id) && !ids.Contains(item.User?.Id))
                        {
                            ids.Add(item.User.Id.ToLower());
                        }
                    }
                }

                List<Worker> workers = this.scheduleQuery.GetWorkers(ids).Result;
                if (workers != null)
                {
                    if (meetingInfo.UserGroups?.Users != null)
                    {
                        foreach (var userGroup in meetingInfo.UserGroups.Users)
                        {
                            var result = workers.FirstOrDefault(a => a.OfficeGraphIdentifier.ToLower().Equals(userGroup?.Id.ToLower()));
                            if (result != null)
                            {
                                userGroup.Name = result.FullName;
                                userGroup.Surname = result.Name.Surname;
                                userGroup.UserPrincipalName = result.Alias;
                                userGroup.GivenName = result.Name.GivenName;
                                userGroup.Title = result.Profession;
                            }
                        }
                    }

                    if (meetingInfo.MeetingDetails?[0].Attendees != null)
                    {
                        foreach (var item in meetingInfo.MeetingDetails[0].Attendees)
                        {
                            var result = workers.FirstOrDefault(a => a.OfficeGraphIdentifier.ToLower().Equals(item.User?.Id?.ToLower()));
                            if (result != null)
                            {
                                item.User.Name = result.FullName;
                                item.User.Surname = result.Name.Surname;
                                item.User.UserPrincipalName = result.Alias;
                                item.User.GivenName = result.Name.GivenName;
                                item.User.Title = result.Profession;
                            }
                        }
                    }

                    if (meetingInfo.Requester?.Id != null)
                    {
                        var result = workers.FirstOrDefault(a => a.OfficeGraphIdentifier.ToLower().Equals(meetingInfo.Requester.Id.ToLower()));
                        if (result != null)
                        {
                            meetingInfo.Requester.Name = result.FullName;
                            meetingInfo.Requester.Title = result.Profession;
                            meetingInfo.Requester.UserPrincipalName = result.Alias;
                            meetingInfo.Requester.Surname = result.Name?.Surname;
                            meetingInfo.Requester.GivenName = result.Name?.GivenName;
                        }
                    }
                }
            }

            return meetingInfo;
        }

        /// <summary>
        /// Converting a location to Meeting attendee
        /// </summary>
        /// <param name="locationAddress">Room address</param>
        /// <returns>List of MeetingAttendee</returns>
        private MeetingAttendee MeetingLocationToMeetingAttendee(string locationAddress)
        {
            if (locationAddress != null)
            {
                return new MeetingAttendee()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress() { Address = locationAddress },
                    Type = SchedulerConstants.AttendeeTypeResource
                };
            }

            return null;
        }

        /// <summary>
        /// Create an outlook calendar event from a meeting details
        /// </summary>
        /// <param name="meetingInfo">Meeting details</param>
        /// <returns>Outlook calendar event</returns>
        private async Task<CalendarEvent> MeetingDetailsToCalendarEvent(MeetingInfo meetingInfo)
        {
            var eventRequest = new CalendarEvent();

            var scheduleMailResult = await this.scheduleQuery.GetScheduleMailDetails(meetingInfo.Id);

            string mailContent = this.GetMailContent(scheduleMailResult, meetingInfo);

            eventRequest.Attendees = this.GraphPersonsToMeetingAttendees(meetingInfo, scheduleMailResult);

            // If location is an object add it to list of attendees for booking room
            if (meetingInfo.MeetingDetails.FirstOrDefault()?.MeetingLocation != null)
            {
                var meetingLocation = meetingInfo.MeetingDetails.FirstOrDefault()?.MeetingLocation;
                if (meetingLocation?.Room?.Address != null)
                {
                    eventRequest.Attendees.Add(this.MeetingLocationToMeetingAttendee(meetingInfo.MeetingDetails.FirstOrDefault()?.MeetingLocation.Room.Address));
                }

                // Set location that will show up in meeting invite
                var location = string.Empty;
                if (!string.IsNullOrEmpty(meetingLocation?.RoomList?.Name))
                {
                    location = meetingLocation.RoomList.Name;
                }

                if (!string.IsNullOrEmpty(meetingLocation?.Room?.Name))
                {
                    location += !string.IsNullOrEmpty(location) ? "/" + meetingLocation.Room.Name : meetingLocation.Room.Name;
                }

                eventRequest.Location = new MeetingLocation()
                {
                    DisplayName = location,
                };
            }

            eventRequest.Start = new MeetingDateTime() { DateTime = meetingInfo.MeetingDetails.FirstOrDefault()?.UtcStart.ToString(SchedulerConstants.RoundTripDateTimePattern), TimeZone = SchedulerConstants.UTCTimezone };
            eventRequest.End = new MeetingDateTime() { DateTime = meetingInfo.MeetingDetails.FirstOrDefault()?.UtcEnd.ToString(SchedulerConstants.RoundTripDateTimePattern), TimeZone = SchedulerConstants.UTCTimezone };

            eventRequest.Subject = scheduleMailResult?.ScheduleMailSubject;

            ResourceManager rm = new ResourceManager(typeof(ScheduleServiceEmailTemplate).Namespace + ".ScheduleServiceEmailTemplate", typeof(ScheduleServiceEmailTemplate).Assembly);
            var emailStyleTemplate = rm.GetString(BusinessConstants.InterviewerTemplateName);

            var templateParams = new Dictionary<string, string>
            {
                { "EmailHeaderUrl", BusinessConstants.MicrosoftLogoUrl },
                { "EmailBodyContent", mailContent },
                { "CompanyInfoFooter", BusinessConstants.MicrosoftInfoFooter },
                { "Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl },
                { "Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl },
            };

            eventRequest.Body = new CalendarBody()
            {
                Content = this.ParseTemplate(emailStyleTemplate, templateParams),
                ContentType = "html"
            };

            eventRequest.IsOrganizer = false;
            MeetingDetails first = meetingInfo.MeetingDetails.FirstOrDefault();
            eventRequest.Sensitivity = first != null && first.IsPrivateMeeting ? SchedulerConstants.PrivateMeetingText : SchedulerConstants.NormalMeetingText;

            eventRequest.IsOnlineMeeting = meetingInfo.MeetingDetails[0].OnlineMeetingRequired;
            if (eventRequest.IsOnlineMeeting)
            {
                eventRequest.OnlineMeetingProvider = "teamsForBusiness";
            }

            eventRequest.AllowNewTimeProposals = WebNotificationServiceFlight.IsEnabled;
            eventRequest.ResponseRequested = true;
            eventRequest.Id = meetingInfo.MeetingDetails.FirstOrDefault()?.CalendarEventId;

            return eventRequest;
        }

        private string GetMailContent(JobApplicationScheduleMailDetails scheduleMailResult, MeetingInfo meetingInfo)
        {
            string mailContent = string.Empty;

            mailContent = scheduleMailResult?.ScheduleMailDescription;

            if (meetingInfo.MeetingDetails?[0].OnlineMeetingRequired == true && !string.IsNullOrWhiteSpace(meetingInfo.MeetingDetails?[0].Conference?.JoinInfo))
            {
                mailContent = mailContent + meetingInfo.MeetingDetails?[0].Conference?.JoinInfo;
            }

            return mailContent;
        }

        /// <summary>Parses the subject and body templates.</summary>
        /// <param name="templateContent">The message Subject Template.</param>
        /// <param name="templateParams">The message Body Template.</param>
        /// <returns>returns parsed template</returns>
        private string ParseTemplate(string templateContent, Dictionary<string, string> templateParams)
        {
            foreach (var key in templateParams.Keys)
            {
                var value = templateParams[key] ?? string.Empty;
                templateContent = templateContent.Replace($"{{{key}}}", value, StringComparison.InvariantCultureIgnoreCase);
            }

            return templateContent;
        }

        /// <summary>
        /// Get free busy schedule
        /// </summary>
        /// <param name="uberUserGroup">Uber User group</param>
        /// <param name="freeBusyRequest">Free busy request</param>
        /// <param name="userAccessToken">User access token</param>
        /// <param name="principal">User principal</param>
        /// <returns>A list of meeting infos</returns>
        private async Task<List<MeetingInfo>> GetFreeBusyInternal(
            UserGroup uberUserGroup,
            FreeBusyRequest freeBusyRequest,
            string userAccessToken,
            HCMApplicationPrincipal principal)
        {
            var request = new FindMeetingTimeRequest();
            var meetingInfos = new List<MeetingInfo>();
            this.logger.LogInformation("GetFreeBusySchedule: Generating FreeBusyScheduleRequest");
            var requestFreeBusy = this.GenerateFreeBusyScheduleRequest(freeBusyRequest, uberUserGroup.Users);
            var graphEmailIdToSchedule = new Dictionary<string, FindFreeBusyScheduleResponse>();
            this.logger.LogInformation("GetFreeBusySchedule: FindFreeBusySchedule");
            var responsesFreeBusyRaw = await this.OutlookProvider.SendPostFindFreeBusySchedule(requestFreeBusy) ?? new List<FindFreeBusyScheduleResponse>();
            var responseCount = responsesFreeBusyRaw.Count();

            if (responseCount == 0)
            {
                return meetingInfos;
            }

            for (int i = 0; i < responseCount; i++)
            {
                graphEmailIdToSchedule[responsesFreeBusyRaw[i].ScheduleId] = responsesFreeBusyRaw[i];
            }

            foreach (var userGroup in freeBusyRequest.UserGroups)
            {
                var userGroupResponses = new List<FindFreeBusyScheduleResponse>();
                var unconsolidatedMeetingDetails = new List<MeetingDetails>();

                foreach (var user in userGroup.Users)
                {
                    userGroupResponses.Add(graphEmailIdToSchedule[user.Email]);
                }

                for (int i = 0; i < userGroupResponses.Count(); i++)
                {
                    var userMeetingInfo = this.FreeBusyScheduleResponseToMeetingInfo(freeBusyRequest, userGroup, userGroupResponses[i], i);
                    unconsolidatedMeetingDetails = unconsolidatedMeetingDetails.Concat(userMeetingInfo.MeetingDetails).ToList();
                }

                var consolidatedMeetingDetailsMeetingInfo = this.ConsolidateMeetingsAccrossFreBusyUsers(userGroup, freeBusyRequest, unconsolidatedMeetingDetails);

                if (!freeBusyRequest.IsRoom)
                {
                    var workingHourMeetingDetails = this.GetWorkingHourMeetingDetails(userGroupResponses, freeBusyRequest.UtcStart, freeBusyRequest.UtcEnd);
                    consolidatedMeetingDetailsMeetingInfo.MeetingDetails = consolidatedMeetingDetailsMeetingInfo.MeetingDetails?.Concat(workingHourMeetingDetails)?.ToList();
                }

                meetingInfos.Add(consolidatedMeetingDetailsMeetingInfo);
            }

            if (!freeBusyRequest.IsRoom)
            {
                meetingInfos.ForEach(mi => mi.MeetingDetails.RemoveAll(md => md.Status == FreeBusyScheduleStatus.Free));
            }

            return meetingInfos;
        }

        /// <summary>
        /// Generates meeting details based on working hours of the user group
        /// </summary>
        /// <param name="userResponses">user group</param>
        /// <param name="utcStart">request start time</param>
        /// <param name="utcEnd">request end time</param>
        /// <returns>list of consolidated meeting details for working hours</returns>
        private List<MeetingDetails> GetWorkingHourMeetingDetails(List<FindFreeBusyScheduleResponse> userResponses, DateTime utcStart, DateTime utcEnd)
        {
            var unconsolidatedMeetingDetailsAccrossTimeZones = new List<MeetingDetails>();
            Dictionary<string, List<FindFreeBusyScheduleResponse>> timeZoneToUserResponse = new Dictionary<string, List<FindFreeBusyScheduleResponse>>();

            foreach (var userResponse in userResponses)
            {
                if (userResponse.WorkingHours != null)
                {
                    timeZoneToUserResponse.TryGetValue(userResponse.WorkingHours.TimeZone?.Name?.ToLower(), out var responses);
                    responses = responses ?? new List<FindFreeBusyScheduleResponse>();
                    responses.Add(userResponse);
                    timeZoneToUserResponse[userResponse.WorkingHours.TimeZone?.Name?.ToLower()] = responses;
                }
            }

            foreach (var timeZone in timeZoneToUserResponse.Keys)
            {
                DateTime latestStartTime = utcStart;
                DateTime earliestEndTime = utcEnd;
                DateTime utcStartParsed = default(DateTime);
                DateTime utcEndParsed = default(DateTime);
                var preUtcStartTimes = new List<DateTime>();

                timeZoneToUserResponse.TryGetValue(timeZone, out var timeZoneUserResponses);
                foreach (var userResponse in timeZoneUserResponses ?? new List<FindFreeBusyScheduleResponse>())
                {
                    var workingHours = userResponse.WorkingHours;
                    TimeZoneInfo userTimeZone = null;

                    if (this.ValidWorkingHours(workingHours))
                    {
                        // Using Flag to avoid using continue in try catch
                        var processingFlag = true;

                        try
                        {
                            userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(workingHours.TimeZone.Name);
                        }
                        catch
                        {
                            processingFlag = false;
                        }

                        if (processingFlag)
                        {
                            var dayOfWeekSchedule = workingHours.DaysOfWeek.Select(day => this.DayStringToDayOfWeek(day.ToLower()))?.ToList() ?? new List<DayOfWeek>();

                            if (dayOfWeekSchedule.Contains(utcStart.DayOfWeek))
                            {
                                var parseFlagStart = DateTime.TryParse(workingHours.StartTime.ToString(), out var startParsed);
                                var parseFlagEnd = DateTime.TryParse(workingHours.EndTime.ToString(), out var endParsed);
                                if (!parseFlagEnd || !parseFlagStart)
                                {
                                    this.logger.LogError("SendCalendarEvent-GetFreeBusySchedule: dating parsing failed");
                                }
                                else
                                {
                                    utcStartParsed = TimeZoneInfo.ConvertTimeToUtc(startParsed, userTimeZone);
                                    utcEndParsed = TimeZoneInfo.ConvertTimeToUtc(endParsed, userTimeZone);

                                    if (utcStartParsed.TimeOfDay < utcStart.TimeOfDay)
                                    {
                                        preUtcStartTimes.Add(new DateTime(utcStart.Year, utcStart.Month, utcStart.Day, utcStartParsed.Hour, utcStartParsed.Minute, utcStartParsed.Second, DateTimeKind.Utc));
                                    }

                                    if (utcStartParsed.TimeOfDay > latestStartTime.TimeOfDay)
                                    {
                                        latestStartTime = new DateTime(utcStart.Year, utcStart.Month, utcStart.Day, utcStartParsed.Hour, utcStartParsed.Minute, utcStartParsed.Second, DateTimeKind.Utc);
                                    }

                                    if (utcEndParsed.TimeOfDay < earliestEndTime.TimeOfDay || ((utcStartParsed.TimeOfDay < latestStartTime.TimeOfDay) && earliestEndTime == utcEnd))
                                    {
                                        earliestEndTime = new DateTime(utcEnd.Year, utcEnd.Month, utcEnd.Day, utcEndParsed.Hour, utcEndParsed.Minute, utcEndParsed.Second, DateTimeKind.Utc);
                                    }
                                }
                            }
                            else
                            {
                                return new List<MeetingDetails>
                                {
                                    new MeetingDetails
                                    {
                                        UtcStart = utcStart,
                                        UtcEnd = utcEnd,
                                        Status = FreeBusyScheduleStatus.NonWorkingHour
                                    }
                                };
                            }
                        }
                    }
                }

                var nonWorkingHourMeetingDetail = new MeetingDetails
                {
                    UtcStart = earliestEndTime,
                    UtcEnd = latestStartTime,
                    Status = FreeBusyScheduleStatus.NonWorkingHour
                };

                if (latestStartTime <= earliestEndTime)
                {
                    var nonWorkingHourMeetingDetailPre = new MeetingDetails
                    {
                        UtcStart = utcStart,
                        UtcEnd = latestStartTime,
                        Status = FreeBusyScheduleStatus.NonWorkingHour
                    };
                    var nonWorkingHourMeetingDetailPost = new MeetingDetails
                    {
                        UtcStart = earliestEndTime,
                        UtcEnd = utcEnd,
                        Status = FreeBusyScheduleStatus.NonWorkingHour
                    };

                    if (nonWorkingHourMeetingDetailPre.UtcStart == nonWorkingHourMeetingDetailPre.UtcEnd && nonWorkingHourMeetingDetailPost.UtcStart != nonWorkingHourMeetingDetailPost.UtcEnd)
                    {
                        var maxPreStartTime = preUtcStartTimes.OrderByDescending(time => time.TimeOfDay).FirstOrDefault();
                        var utcEndProcessed = new DateTime(utcEnd.Year, utcEnd.Month, utcEnd.Day, maxPreStartTime.Hour, maxPreStartTime.Minute, maxPreStartTime.Second, DateTimeKind.Utc);
                        if (utcStartParsed.Date == utcEndParsed.Date)
                        {
                            if (utcStart < utcEndProcessed)
                            {
                                unconsolidatedMeetingDetailsAccrossTimeZones.Add(new MeetingDetails { Status = FreeBusyScheduleStatus.NonWorkingHour, UtcStart = utcStart, UtcEnd = utcEndProcessed });
                            }

                            if (earliestEndTime < utcEnd)
                            {
                                unconsolidatedMeetingDetailsAccrossTimeZones.Add(new MeetingDetails { Status = FreeBusyScheduleStatus.NonWorkingHour, UtcStart = earliestEndTime, UtcEnd = utcEnd });
                            }
                        }
                        else
                        {
                            unconsolidatedMeetingDetailsAccrossTimeZones.Add(new MeetingDetails { Status = FreeBusyScheduleStatus.NonWorkingHour, UtcStart = earliestEndTime, UtcEnd = new DateTime(utcEnd.Year, utcEnd.Month, utcEnd.Day, maxPreStartTime.Hour, maxPreStartTime.Minute, maxPreStartTime.Second, DateTimeKind.Utc) });
                        }
                    }
                    else if (nonWorkingHourMeetingDetailPre.UtcStart == nonWorkingHourMeetingDetailPre.UtcEnd && nonWorkingHourMeetingDetailPost.UtcStart == nonWorkingHourMeetingDetailPost.UtcEnd)
                    {
                        nonWorkingHourMeetingDetail.Status = FreeBusyScheduleStatus.Free;
                        unconsolidatedMeetingDetailsAccrossTimeZones.Add(nonWorkingHourMeetingDetail);
                    }
                    else if (nonWorkingHourMeetingDetailPost.UtcStart == nonWorkingHourMeetingDetailPost.UtcEnd && nonWorkingHourMeetingDetailPre.UtcStart != nonWorkingHourMeetingDetailPre.UtcEnd)
                    {
                        nonWorkingHourMeetingDetailPost.UtcStart = utcEndParsed;
                    }
                    else
                    {
                        unconsolidatedMeetingDetailsAccrossTimeZones = unconsolidatedMeetingDetailsAccrossTimeZones.Concat(new List<MeetingDetails> { nonWorkingHourMeetingDetailPre, nonWorkingHourMeetingDetailPost }).ToList();
                    }
                }
                else
                {
                    unconsolidatedMeetingDetailsAccrossTimeZones.Add(nonWorkingHourMeetingDetail);
                }
            }

            return this.MergeTimeSlots(unconsolidatedMeetingDetailsAccrossTimeZones);
        }

        private DayOfWeek DayStringToDayOfWeek(string day)
        {
            switch (day)
            {
                case "sunday":
                    return DayOfWeek.Sunday;

                case "monday":
                    return DayOfWeek.Monday;

                case "tuesday":
                    return DayOfWeek.Tuesday;

                case "wednesday":
                    return DayOfWeek.Wednesday;

                case "thursday":
                    return DayOfWeek.Thursday;

                case "friday":
                    return DayOfWeek.Friday;

                case "saturday":
                    return DayOfWeek.Saturday;

                default:
                    return DayOfWeek.Sunday;
            }
        }

        /// <summary>
        /// Merging overlapping time intervals using Stack logic
        /// </summary>
        /// <param name="unconsolidatedMeetingDetailsAccrossTimeZones"> overlaping meeting details</param>
        /// <returns>consolidated meeting details</returns>
        private List<MeetingDetails> MergeTimeSlots(List<MeetingDetails> unconsolidatedMeetingDetailsAccrossTimeZones)
        {
            this.logger.LogInformation("SendCalendarEvent: Merging timeslots");
            var sortedOverlappingIntervals = unconsolidatedMeetingDetailsAccrossTimeZones?.OrderBy(md => md.UtcStart)?.ToList();
            var intervalStack = new Stack<MeetingDetails>();

            foreach (var interval in sortedOverlappingIntervals)
            {
                if (intervalStack.Count == 0)
                {
                    intervalStack.Push(interval);
                }
                else
                {
                    var stackTop = intervalStack.Peek();
                    if (stackTop.UtcEnd < interval.UtcStart)
                    {
                        intervalStack.Push(interval);
                    }
                    else if (stackTop.UtcEnd < interval.UtcEnd)
                    {
                        stackTop.UtcEnd = interval.UtcEnd;
                        intervalStack.Pop();
                        intervalStack.Push(stackTop);
                    }
                }
            }

            return intervalStack.ToList();
        }

        /// <summary>
        /// Consolidates meetings accross users
        /// </summary>
        /// <param name="userGroup">User group</param>
        /// <param name="freeBusyRequest">Free busy request</param>
        /// <param name="meetingDetailsToConsolidate">Meeting details to consolidate</param>
        /// <returns>Consolidated meeting info</returns>
        private MeetingInfo ConsolidateMeetingsAccrossFreBusyUsers(
            UserGroup userGroup,
            FreeBusyRequest freeBusyRequest,
            List<MeetingDetails> meetingDetailsToConsolidate)
        {
            var meetingInfo = new MeetingInfo
            {
                Id = userGroup.FreeBusyTimeId,
                UserGroups = userGroup,
                MeetingDetails = new List<MeetingDetails>()
            };

            var granularMeetingDetails = this.GetGranularMeetingDetails(freeBusyRequest.UtcStart, freeBusyRequest.UtcEnd);

            foreach (var meetingSlot in meetingDetailsToConsolidate)
            {
                var meetingSlotTicker = meetingSlot.UtcStart;
                while (meetingSlotTicker < meetingSlot.UtcEnd)
                {
                    var granularMeetingIndex = granularMeetingDetails.FindIndex(gm => gm.UtcStart == meetingSlotTicker);

                    if (granularMeetingIndex == -1)
                    {
                        this.logger.LogError("SendCalendarEvent-ConsolidateMeetingsAccrossFreBusyUsers:Index of granular not found");
                        granularMeetingIndex = 0;
                    }

                    granularMeetingDetails[granularMeetingIndex].Status = this.GetStatusByPrecedence(granularMeetingDetails[granularMeetingIndex].Status, meetingSlot.Status);

                    meetingSlotTicker = meetingSlotTicker.AddMinutes(30);
                }
            }

            return this.GranularMeetingsToConsolidatedMeetings(granularMeetingDetails, meetingInfo, null);
        }

        /// <summary>
        /// consolidates granular meetings
        /// </summary>
        /// <param name="granularMeetingDetails"> list of granular meeting details</param>
        /// <param name="meetingInfo"> Meeting info for consolidated meeting details</param>
        /// <param name="findMeetingTimeResponse">Optional param to set if consolidating find meeting times meetings</param>
        /// <returns>onsolidated meeting details</returns>
        private MeetingInfo GranularMeetingsToConsolidatedMeetings(List<MeetingDetails> granularMeetingDetails, MeetingInfo meetingInfo, FindMeetingTimeResponse findMeetingTimeResponse = null)
        {
            MeetingDetails workingMeetingDetails = null;
            MeetingDetails lastMeetingDetails = null;
            foreach (var m in granularMeetingDetails)
            {
                if (workingMeetingDetails == null)
                {
                    workingMeetingDetails = new MeetingDetails
                    {
                        UtcStart = m.UtcStart,
                        UtcEnd = m.UtcEnd,
                        Status = m.Status,
                        IsTentative = m.IsTentative,
                    };
                }
                else
                {
                    var findMeetingSlotMergeFlag = workingMeetingDetails.IsTentative == m.IsTentative && findMeetingTimeResponse != null;
                    var freeBusyScheduleSlotMergeFlag = workingMeetingDetails.Status == m.Status
                                            && findMeetingTimeResponse == null;

                    // Meetings are contiguous, collapse
                    if (workingMeetingDetails.UtcEnd == m.UtcStart && (findMeetingSlotMergeFlag || freeBusyScheduleSlotMergeFlag))
                    {
                        workingMeetingDetails.UtcEnd = m.UtcEnd;
                    }
                    else
                    {
                        meetingInfo.MeetingDetails.Add(workingMeetingDetails);
                        workingMeetingDetails = new MeetingDetails();
                        lastMeetingDetails = new MeetingDetails();
                        workingMeetingDetails.UtcStart = lastMeetingDetails.UtcStart = m.UtcStart;
                        workingMeetingDetails.UtcEnd = lastMeetingDetails.UtcEnd = m.UtcEnd;
                        workingMeetingDetails.Status = m.Status;
                        workingMeetingDetails.IsTentative = m.IsTentative;
                    }
                }
            }

            if ((workingMeetingDetails != null && lastMeetingDetails == null) || // This means there are no free times, so collapse all busy times
                (workingMeetingDetails != null && (workingMeetingDetails.UtcStart != lastMeetingDetails.UtcStart || workingMeetingDetails.UtcEnd != lastMeetingDetails.UtcEnd)))
            {
                if (findMeetingTimeResponse != null && findMeetingTimeResponse.EmptySuggestionsReason == SchedulerConstants.UnknownFreeBusy)
                {
                    workingMeetingDetails.UnknownFreeBusyTime = true;
                }

                // If the last working meeting detail doesn't equal the last meeting detail it means it hasn't been added yet, so add to the list
                meetingInfo.MeetingDetails.Add(workingMeetingDetails);
            }

            return meetingInfo;
        }

        /// <summary>
        /// Converts a FreeBusyScheduleResponse to a MeetingInfo
        /// </summary>
        /// <param name="freeBusyRequest">Free busy schedule request</param>
        /// <param name="userGroup">User group the free times are for</param>
        /// <param name="freeBusyScheduleResponse">Response to convert</param>
        /// <param name="userIndex"> Index of the user for the status</param>
        /// <returns>Free busy time meeting info</returns>
        private MeetingInfo FreeBusyScheduleResponseToMeetingInfo(
            FreeBusyRequest freeBusyRequest,
            UserGroup userGroup,
            FindFreeBusyScheduleResponse freeBusyScheduleResponse,
            int userIndex)
        {
            var meetingInfo = new MeetingInfo
            {
                Id = userGroup.FreeBusyTimeId,
                UserGroups = userGroup ?? new UserGroup(),
                MeetingDetails = new List<MeetingDetails>()
            };

            if (freeBusyScheduleResponse.Error?.ResponseCode == "ErrorNoFreeBusyAccess")
            {
                this.logger.LogWarning("SendCalendarEvent-FreeBusyScheduleResponseToMeetingInfo :No free busy access for resource:" + freeBusyScheduleResponse.ScheduleId);

                // Consider switching the status to Unknown instead of Busy
                meetingInfo.MeetingDetails.Add(new MeetingDetails
                {
                    Status = FreeBusyScheduleStatus.Busy,
                    UtcStart = freeBusyRequest.UtcStart,
                    UtcEnd = freeBusyRequest.UtcEnd,
                });

                return meetingInfo;
            }

            var utcStart = freeBusyRequest.UtcStart;
            var granularMeetingDetails = this.GetGranularMeetingDetails(freeBusyRequest.UtcStart, freeBusyRequest.UtcEnd);

            foreach (var slot in freeBusyScheduleResponse.ScheduleItems ?? new List<ScheduleItem>())
            {
                // This is the unknown response. Consider a better way to convey this to client
                if (slot.Status == null)
                {
                    var unknownMeetingDetails = new MeetingDetails
                    {
                        UtcStart = freeBusyRequest.UtcStart,
                        UtcEnd = freeBusyRequest.UtcEnd,
                        UnknownFreeBusyTime = true
                    };
                    meetingInfo.MeetingDetails.Add(unknownMeetingDetails);

                    return meetingInfo;
                }

                // TODO: Consider checking to make sure timezone is "UTC" here before appending "Z"
                DateTime.TryParse(slot.Start.DateTime + "Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime utcStartFree);
                DateTime.TryParse(slot.End.DateTime + "Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime utcEndFree);

                // Rounding down start time and rounding up end time
                (utcStartFree, utcEndFree) = this.RoundTime(utcStartFree, utcEndFree);
                if (utcEndFree > utcStart)
                {
                    var scheduleIndex = Math.Max(0, granularMeetingDetails.FindIndex(x => x.UtcStart == utcStartFree));
                    var durationInMinutes = (utcEndFree - utcStartFree).TotalMinutes;
                    var rounding = (int)durationInMinutes % 30 != 0 ? 1 : 0;
                    var granularMeetingsToUpdate = rounding + (durationInMinutes / 30);
                    var freeBusyStatus = this.FreeBusyStatusMap(slot.Status);

                    if (freeBusyStatus != FreeBusyScheduleStatus.Free)
                    {
                        var iterator = scheduleIndex;
                        while (iterator < Math.Min(granularMeetingsToUpdate + scheduleIndex, granularMeetingDetails.Count()))
                        {
                            granularMeetingDetails[iterator].Status = this.GetStatusByPrecedence(granularMeetingDetails[iterator].Status, freeBusyStatus);
                            iterator += 1;
                        }
                    }
                }
            }

            meetingInfo.MeetingDetails = granularMeetingDetails;
            return meetingInfo;
        }

        /// <summary>
        /// Creates granular meeting details for a given measure
        /// </summary>
        /// <param name="utcStart">start of meeting details</param>
        /// <param name="utcEnd">end of meeting details</param>
        /// <param name="granularityMeasure">granularity measure</param>
        /// <returns>list of meeting details</returns>
        private List<MeetingDetails> GetGranularMeetingDetails(DateTime utcStart, DateTime utcEnd, int granularityMeasure = 30)
        {
            var granularMeetingDetails = new List<MeetingDetails>();
            while (utcStart < utcEnd)
            {
                var utcEndResult = utcStart.AddMinutes(granularityMeasure);

                granularMeetingDetails.Add(
                    new MeetingDetails()
                    {
                        UtcStart = utcStart,
                        UtcEnd = utcEndResult,
                        Status = FreeBusyScheduleStatus.Free,
                    });

                utcStart = utcEndResult;
            }

            return granularMeetingDetails;
        }

        private FreeBusyScheduleStatus FreeBusyStatusMap(string status)
        {
            switch (status?.ToLower() ?? string.Empty)
            {
                case "free":
                    return FreeBusyScheduleStatus.Free;

                case "busy":
                    return FreeBusyScheduleStatus.Busy;

                case "oof":
                    return FreeBusyScheduleStatus.Oof;

                case "workingelsewhere":
                    return FreeBusyScheduleStatus.WorkingElsewhere;

                case "unknown":
                    return FreeBusyScheduleStatus.Unknown;

                case "tentative":
                    return FreeBusyScheduleStatus.Tentative;

                default:
                    return FreeBusyScheduleStatus.Unknown;
            }
        }

        private FreeBusyScheduleStatus GetStatusByPrecedence(FreeBusyScheduleStatus statusA, FreeBusyScheduleStatus statusB)
        {
            var statusPrecedenceDictionary = new Dictionary<FreeBusyScheduleStatus, int> { { FreeBusyScheduleStatus.Oof, 1 }, { FreeBusyScheduleStatus.Busy, 2 }, { FreeBusyScheduleStatus.WorkingElsewhere, 3 }, { FreeBusyScheduleStatus.Tentative, 4 }, { FreeBusyScheduleStatus.Unknown, 5 }, { FreeBusyScheduleStatus.Free, 6 } };
            return statusPrecedenceDictionary[statusA] <= statusPrecedenceDictionary[statusB] ? statusA : statusB;
        }

        /// <summary>
        /// Function to round down start and round up end time
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <returns>Rounded times</returns>
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
        private (DateTime, DateTime) RoundTime(DateTime startTime, DateTime endTime)
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            return (new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0).AddMinutes(-startTime.Minute % 30),
                    new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, 0).AddMinutes(endTime.Minute % 30 == 0 ? 0 : 30 - (endTime.Minute % 30)));
        }

        /// <summary>
        /// Creates a new request object for the outlook provider from a schedule event and interviewers to get individual free/busy time.
        /// </summary>
        /// <param name="freeBusyRequest">The free times request.</param>
        /// <param name="interviewers">List of interviewers to get free time for</param>
        /// <returns>The free meeting time request object</returns>
        private FindFreeBusyScheduleRequest GenerateFreeBusyScheduleRequest(FreeBusyRequest freeBusyRequest, List<GraphPerson> interviewers)
        {
            var request = new FindFreeBusyScheduleRequest()
            {
                Schedules = interviewers.Select(a => a.Email).Where(a => !string.IsNullOrEmpty(a)).ToList(),
                AvailabilityViewInterval = SchedulerConstants.ThirtyMinuteFreeBusy,
                StartTime = new MeetingDateTime { DateTime = freeBusyRequest.UtcStart.ToString(SchedulerConstants.RoundTripDateTimePattern), TimeZone = SchedulerConstants.UTCTimezone },
                EndTime = new MeetingDateTime { DateTime = freeBusyRequest.UtcEnd.ToString(SchedulerConstants.RoundTripDateTimePattern), TimeZone = SchedulerConstants.UTCTimezone },
            };

            return request;
        }

        /// <summary>
        /// Check if working hours are valid
        /// </summary>
        /// <param name="workingHours">working hours of a person</param>
        /// <returns>boolean denoting if working hours valid</returns>
        private bool ValidWorkingHours(WorkingHours workingHours)
        {
            return workingHours != null && workingHours.DaysOfWeek != null && workingHours.TimeZone != null && workingHours.StartTime != null && workingHours.EndTime != null && workingHours.TimeZone.Name != null;
        }

        /// <summary>
        /// Updates the schedules sharing status.
        /// </summary>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> where <c>true</c> when all schedule updates are successful; otherwise <c>false</c>.</returns>
        private async Task<bool> UpdateSchedulesSharingStatus(ScheduleInvitationRequest scheduleInvitationRequest)
        {
            bool updatedSharedSchedules = true;
            bool updated = false;
            ConcurrentBag<string> updatedSchedules = new ConcurrentBag<string>();
            var tasks = scheduleInvitationRequest.SharedSchedules.Select(async (sch) =>
            {
                updated = await this.scheduleQuery.UpdateScheduleWithSharingStatusAsync(sch).ConfigureAwait(false);
                updatedSchedules.Add(sch.ScheduleId);
            });

            await Task.WhenAll(tasks);
            foreach (var schedule in scheduleInvitationRequest.SharedSchedules)
            {
                if (!updatedSchedules.Contains(schedule.ScheduleId, StringComparer.OrdinalIgnoreCase))
                {
                    updatedSharedSchedules = false;
                    this.logger.LogError($"Failed to update schedule sharing details with Id '{schedule.ScheduleId}'");
                }
                else if (PublishScheduleFlight.IsEnabled)
                {
                    ConnectorIntegrationDetails syncBackDetails = new ConnectorIntegrationDetails();
                    syncBackDetails.ScheduleAction = ActionType.Update;
                    syncBackDetails.ScheduleID = schedule.ScheduleId;
                    var rootActivityId = this.serviceContext.RootActivityId;
                    var jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(schedule.ScheduleId).ConfigureAwait(false);
                    await this.serviceBusHelper.QueueMessageToConnector("MSInterviewSchedule", syncBackDetails, rootActivityId, jobApplicationId);
                }
            }

            return updatedSharedSchedules;
        }

        /// <summary>
        /// Updates the connector integration.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="rootActivityId">The root activity identifier.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        private async Task UpdateConnectorIntegration(string jobApplicationId, string rootActivityId)
        {
            ConnectorIntegrationDetails connectorIntegrationDetails = new ConnectorIntegrationDetails();
            connectorIntegrationDetails.ScheduleAction = ActionType.Create;
            connectorIntegrationDetails.ScheduleID = jobApplicationId;

            await this.serviceBusHelper.QueueMessageToConnector("InterviewScheduled", connectorIntegrationDetails, rootActivityId, jobApplicationId);
        }

        private DateTime TimeRoundUp(DateTime dt, TimeSpan span)
        {
            return new DateTime((dt.Ticks + span.Ticks - 1) / span.Ticks * span.Ticks, dt.Kind);
        }

        private MeetingInfo FindMeetingTimeResponseToMeetingInfo(FindMeetingTimeResponse findMeetingTimeResponse, SuggestMeetingsRequest suggestMeetingsRequest)
        {
            if (suggestMeetingsRequest == null)
            {
                return null;
            }

            var users = new List<GraphPerson>();
            var attendees = new List<Attendee>();
            var principal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();
            var meetingDetailsId = Guid.NewGuid().ToString();

            findMeetingTimeResponse.MeetingTimeSuggestions?.FirstOrDefault()?.AttendeeAvailability?.ForEach(attendee =>
            {
                if (attendee.Attendee.EmailAddress.Address != suggestMeetingsRequest.Candidate?.Email)
                {
                    var userDetails = suggestMeetingsRequest.InterviewersList?.FirstOrDefault(i => i.FreeBusyTimeId != this.candidateFreeBusyTimeId)
                    ?.Users?.FirstOrDefault(u => u.Email == attendee.Attendee.EmailAddress.Address);

                    var user = userDetails != null ? new GraphPerson
                    {
                        Id = userDetails.Id,
                        Email = userDetails.Email,
                        Name = userDetails.Name,
                        GivenName = userDetails.GivenName,
                        Surname = userDetails.Surname,
                        Title = userDetails.Title,
                        UserPrincipalName = userDetails.UserPrincipalName,
                    }
                    : new GraphPerson();

                    users.Add(user);

                    attendees.Add(new Attendee
                    {
                        User = user,
                        ResponseStatus = InvitationResponseStatus.None,
                    });
                }
            });

            return new MeetingInfo
            {
                InterviewerTimeSlotId = suggestMeetingsRequest.InterviewersList?.FirstOrDefault(i => i.FreeBusyTimeId != this.candidateFreeBusyTimeId)?.FreeBusyTimeId,
                Id = meetingDetailsId,
                UserGroups = new UserGroup
                {
                    FreeBusyTimeId = suggestMeetingsRequest.InterviewersList?.FirstOrDefault(i => i.FreeBusyTimeId != this.candidateFreeBusyTimeId)?.FreeBusyTimeId,
                    Users = users,
                },
                Requester = new GraphPerson
                {
                    Id = principal?.UserObjectId,
                    Email = principal?.EmailAddress,
                    Name = principal?.GivenName + principal?.FamilyName,
                    GivenName = principal?.GivenName,
                    Surname = principal?.FamilyName,
                },
                MeetingDetails = new List<MeetingDetails>
                {
                    new MeetingDetails
                    {
                        Id = meetingDetailsId,
                        IsPrivateMeeting = suggestMeetingsRequest.PrivateMeeting,
                        Subject = $"Interview with {suggestMeetingsRequest.Candidate?.Name}",
                        OnlineMeetingRequired = suggestMeetingsRequest.TeamsMeeting,
                        Attendees = attendees,
                        UtcStart = DateTime.Parse(findMeetingTimeResponse.MeetingTimeSuggestions?.FirstOrDefault()?.MeetingTimeSlot?.Start.DateTime),
                        UtcEnd = DateTime.Parse(findMeetingTimeResponse.MeetingTimeSuggestions?.FirstOrDefault()?.MeetingTimeSlot?.End.DateTime),
                        IsDirty = true,
                    },
                },
                ScheduleStatus = ScheduleStatus.NotScheduled,
            };
        }

        private FreeBusyRequest CreateFreeBusyRequest(SuggestMeetingsRequest suggestMeetingsRequest)
        {
            if (suggestMeetingsRequest == null)
            {
                return null;
            }

            var userGroups = suggestMeetingsRequest.InterviewersList != null ? suggestMeetingsRequest.InterviewersList : new List<UserGroup>();

            if (!string.IsNullOrWhiteSpace(suggestMeetingsRequest.Candidate?.Id))
            {
                userGroups.Add(new UserGroup
                {
                    FreeBusyTimeId = this.candidateFreeBusyTimeId,
                    Users = new List<GraphPerson>
                    {
                        new GraphPerson
                        {
                            Id = suggestMeetingsRequest.Candidate.Id,
                            Email = suggestMeetingsRequest.Candidate.Email,
                            Name = suggestMeetingsRequest.Candidate.Name,
                            GivenName = suggestMeetingsRequest.Candidate.GivenName,
                            Surname = suggestMeetingsRequest.Candidate.Surname,
                            Title = suggestMeetingsRequest.Candidate.Title,
                            UserPrincipalName = suggestMeetingsRequest.Candidate.UserPrincipalName,
                        }
                    }
                });
            }

            return new FreeBusyRequest
            {
                UserGroups = userGroups?.ToList(),
                UtcStart = suggestMeetingsRequest.InterviewStartDateSuggestion,
                UtcEnd = suggestMeetingsRequest.InterviewEndDateSuggestion,
            };
        }

        private FindMeetingTimeRequest CreateFindMeetingTimeRequest(SuggestMeetingsRequest suggestMeetingsRequest)
        {
            if (suggestMeetingsRequest == null)
            {
                return null;
            }

            var attendes = new List<MeetingAttendee>();

            suggestMeetingsRequest.InterviewersList?.FirstOrDefault()?.Users?.ForEach(interviewer =>
            {
                attendes.Add(new MeetingAttendee
                {
                    EmailAddress = new MeetingAttendeeEmailAddress
                    {
                        Address = interviewer.Email,
                        ObjectId = interviewer.Id,
                        Name = interviewer.GivenName,
                    }
                });
            });

            if (!string.IsNullOrWhiteSpace(suggestMeetingsRequest.Candidate?.Id))
            {
                attendes.Add(new MeetingAttendee
                {
                    EmailAddress = new MeetingAttendeeEmailAddress
                    {
                        Address = suggestMeetingsRequest.Candidate.Email,
                        ObjectId = suggestMeetingsRequest.Candidate.Id,
                        Name = suggestMeetingsRequest.Candidate.GivenName,
                    }
                });
            }

            return new FindMeetingTimeRequest
            {
                Attendees = attendes,
                TimeConstraint = new MeetingTimeConstraint
                {
                    Timeslots = new List<MeetingTimeSlot>
                        {
                            new MeetingTimeSlot
                            {
                                Start = new MeetingDateTime { DateTime = suggestMeetingsRequest.InterviewStartDateSuggestion.ToString(), TimeZone = "Pacific Standard Time" },
                                End = new MeetingDateTime { DateTime = suggestMeetingsRequest.InterviewEndDateSuggestion.ToString(), TimeZone = "Pacific Standard Time" },
                            }
                        }
                },
                MeetingDuration = "PT" + suggestMeetingsRequest.MeetingDurationInMinutes + "M",
                IsOrganizerOptional = true,
                MaxCandidates = 3,
            };
        }

        private MeetingInfo SuggestionToMeetingInfo(int intervalIndex, string freeBusyTimeId, SuggestMeetingsRequest suggestMeetingsRequest)
        {
            if (suggestMeetingsRequest == null || string.IsNullOrWhiteSpace(freeBusyTimeId))
            {
                return null;
            }

            var meetingDurationInMinutes = int.Parse(suggestMeetingsRequest.MeetingDurationInMinutes);
            var slotsNeededForMeeting = meetingDurationInMinutes / this.slotDurationInMinutes;
            var suggestedMeetingStartTime = this.SlotIndexToDateTime(intervalIndex - slotsNeededForMeeting, suggestMeetingsRequest);
            var suggestedMeetingEndTime = this.SlotIndexToDateTime(intervalIndex, suggestMeetingsRequest);
            var principal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();
            var interviewer = suggestMeetingsRequest.InterviewersList?.FirstOrDefault(i => i.FreeBusyTimeId == freeBusyTimeId);
            var meetingDetailsId = Guid.NewGuid().ToString();

            return new MeetingInfo
            {
                InterviewerTimeSlotId = interviewer?.FreeBusyTimeId,
                Id = meetingDetailsId,
                UserGroups = interviewer,
                Requester = new GraphPerson
                {
                    Id = principal?.UserObjectId,
                    Email = principal?.EmailAddress,
                    Name = principal?.GivenName + principal?.FamilyName,
                    GivenName = principal?.GivenName,
                    Surname = principal?.FamilyName,
                },
                MeetingDetails = new List<MeetingDetails>
                {
                    new MeetingDetails
                    {
                        Id = meetingDetailsId,
                        IsPrivateMeeting = suggestMeetingsRequest.PrivateMeeting,
                        Attendees = interviewer != null ? new List<Attendee>
                        {
                            new Attendee
                            {
                                User = interviewer?.Users?.FirstOrDefault(),
                                ResponseStatus = InvitationResponseStatus.None,
                            },
                        }
                        : new List<Attendee>(),
                        UtcStart = suggestedMeetingStartTime,
                        UtcEnd = suggestedMeetingEndTime,
                        Subject = $"Interview with {suggestMeetingsRequest.Candidate?.Name}",
                        OnlineMeetingRequired = suggestMeetingsRequest.TeamsMeeting,
                        IsDirty = true,
                    },
                },
                ScheduleStatus = ScheduleStatus.NotScheduled,
            };
        }
    }
}
