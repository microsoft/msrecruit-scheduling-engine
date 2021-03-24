//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;
    using CommonLibrary.Common.Base.ServiceContext;
    using CommonLibrary.Common.Common.Common.Web.Extensions;
    using CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract;
    using CommonLibrary.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;

    // TODO: Enable authentication for the APIs.

    /// <summary>
    /// Interface to perform Interview Scheduling operations in Microsoft Recruit.
    /// </summary>
    [Route("v1/schedule")]
    public class ScheduleServiceController : ControllerBase
    {
        /// <summary>
        /// holds read only schedule value
        /// </summary>
        private readonly IScheduleManager schedule;

        /// <summary>
        /// Holds read only role manager instance
        /// </summary>
        private readonly IRoleManager roleManager;

        /// <summary>
        /// holds servicecontext
        /// </summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary>
        /// The instance for <see cref="ILogger{ScheduleServiceController}"/>.
        /// </summary>
        private readonly ILogger<ScheduleServiceController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleServiceController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="serviceContext">service context</param>
        /// <param name="scheduler"> Scheduler interface </param>
        /// <param name="roleMananger">role manager</param>
        /// <param name="logger">The instance for <see cref="ILogger{ScheduleServiceController}"/>.</param>
        public ScheduleServiceController(
            IHttpContextAccessor httpContextAccessor,
            IHCMServiceContext serviceContext,
            IScheduleManager scheduler,
            IRoleManager roleMananger,
            ILogger<ScheduleServiceController> logger)
                : base()
        {
            this.schedule = scheduler;
            this.roleManager = roleMananger;
            this.hcmServiceContext = serviceContext;
            this.logger = logger;
        }

        /// <summary>
        /// Add or update activity time zone.
        /// </summary>
        /// <param name="jobApplicationId">Job application id to update time zone</param>
        /// <param name="timezone">time zone to update.</param>
        /// <returns>Task</returns>
        [HttpPut("timezone/{jobApplicationId}")]
        [MonitorWith("GTAV1AddUpdatetimezone")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task AddOrUpdateTimezone(string jobApplicationId, [FromBody] Timezone timezone)
        {
            this.logger.LogInformation($"Started {nameof(this.AddOrUpdateTimezone)} method in {nameof(ScheduleServiceController)}.");

            if (string.IsNullOrWhiteSpace(jobApplicationId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            if (timezone == null)
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid timezone info").EnsureLogged(this.logger);
            }

            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, jobApplicationId, string.Empty).ConfigureAwait(false))
            {
                await this.schedule.UpdateTimezoneForJobApplication(jobApplicationId, timezone);
                this.logger.LogInformation($"Finished {nameof(this.AddOrUpdateTimezone)} method in {nameof(ScheduleServiceController)}.");
            }
            else
            {
                throw new UnauthorizedStatusException($"You are unauthorized to add timezone").EnsureLogged(this.logger);
            }
        }

        /// <summary>
        /// Get Timezone for a JobApplicationId.
        /// </summary>
        /// <param name="jobApplicationId">Job application id to get time zone</param>
        /// <returns>Task</returns>
        [HttpGet("timezone/{jobApplicationId}")]
        [MonitorWith("GTAV1GetTimezoneByJobApplicationId")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Timezone> GetTimezoneByJobApplicationId(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetTimezoneByJobApplicationId)} method in {nameof(ScheduleServiceController)}.");

            if (string.IsNullOrWhiteSpace(jobApplicationId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            var result = await this.schedule.GetTimezoneByJobApplicationId(jobApplicationId);
            this.logger.LogInformation($"Finished {nameof(this.GetTimezoneByJobApplicationId)} method in {nameof(ScheduleServiceController)}.");
            return result;
        }

        /// <summary>
        /// Fetches the free/busy calender information of the given user(s) based on their availability and work hours settings.
        /// </summary>
        /// <param name="freeBusyRequest">An instance of <see cref="FreeBusyRequest"/> with details of user(s) whose free/busy data is required with date filter.</param>
        /// <returns>An instance of <see cref="Task{T}" /> where <c>T</c> being <see cref="IList{MeetingInfo}" />.</returns>
        [HttpPost("getfreebusyschedule")]
        [MonitorWith("GTAV1FreeBusySchedule")]
        [ProducesResponseType(typeof(List<MeetingInfo>), 200)]
        [ProducesResponseType(typeof(InvalidRequestDataValidationException), 400)]
        [ProducesResponseType(typeof(SchedulerProcessingException), 500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<List<MeetingInfo>> GetFreeBusySchedule([FromBody] FreeBusyRequest freeBusyRequest)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetFreeBusySchedule)} method in {nameof(ScheduleServiceController)}.");
            if (freeBusyRequest == null)
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid free busy schedule info").EnsureLogged(this.logger);
            }

            meetingInfos = await this.schedule.GetFreeBusySchedule(freeBusyRequest);
            this.logger.LogInformation($"Finished {nameof(this.GetFreeBusySchedule)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfos;
        }

        /// <summary>
        /// Create schedule api
        /// </summary>
        /// <param name="meetingInfo">The schedule object.</param>
        /// <param name="sourceExternalId">SourceExternalId</param>
        /// <returns>The response.</returns>
        [HttpPost("createschedule/{sourceExternalId}")]
        [MonitorWith("GTAV1CreateSchedule")]
        public async Task<MeetingInfo> CreateSchedule([FromBody] MeetingInfo meetingInfo, string sourceExternalId)
        {
            MeetingInfo meetingInfoResult;
            this.logger.LogInformation($"Started {nameof(this.CreateSchedule)} method in {nameof(ScheduleServiceController)}.");
            if (meetingInfo == null || string.IsNullOrEmpty(sourceExternalId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid meeting info").EnsureLogged(this.logger);
            }

            meetingInfoResult = await this.schedule.CreateSchedule(meetingInfo, sourceExternalId);

            // await this.schedule.ValidateApplicationByApplicationId(sourceExternalId);
            // var hcmContextUserId = this.hcmServiceContext.UserId;
            // if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, sourceExternalId, string.Empty))
            // {
            //    if (meetingInfo.UserGroups?.Users?.Count == 0 || await this.roleManager.AreParticipantsInterviewer(meetingInfo.UserGroups?.Users?.Select(user => user.Id).ToList(), jobApplicationId))
            //    {
            //        ////Schedule status as saved
            //        meetingInfoResult = await this.schedule.CreateSchedule(meetingInfo, sourceExternalId);
            //    }
            //    else
            //    {
            //        throw new BusinessRuleViolationException($"Input request does not contain a valid schedule participant").EnsureLogged(this.logger);
            //    }
            // }
            // else
            // {
            //    throw new UnauthorizedStatusException("You are unauthorized to access application").EnsureLogged(this.logger);
            // }
            this.logger.LogInformation($"Finished {nameof(this.CreateSchedule)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfoResult;
        }

        /// <summary>
        /// Suggest and Create schedules api
        /// </summary>
        /// <param name="suggestMeetingsRequest">The meeting suggestion request object.</param>
        /// <param name="jobApplicationId">JobApplicationId</param>
        /// <returns>List of suggested schedules.</returns>
        [HttpPost("meetingsuggestions/{jobApplicationId}")]
        [MonitorWith("GTAV1SuggestAndCreateSchedule")]
        public async Task<IList<MeetingInfo>> SuggestAndCreateSchedules([FromBody] SuggestMeetingsRequest suggestMeetingsRequest, string jobApplicationId)
        {
            IList<Task<MeetingInfo>> createSchedulesTask = new List<Task<MeetingInfo>>();
            IList<MeetingInfo> createdSchedules = new List<MeetingInfo>();
            this.logger.LogInformation($"Started {nameof(this.SuggestAndCreateSchedules)} method in {nameof(ScheduleServiceController)}.");
            if (suggestMeetingsRequest == null || string.IsNullOrWhiteSpace(jobApplicationId))
            {
                this.logger.LogWarning($"{nameof(this.SuggestAndCreateSchedules)}: input is invalid");
                throw new BusinessRuleViolationException($"Input request does not contain a valid meeting info").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByApplicationId(jobApplicationId);
            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, jobApplicationId, string.Empty))
            {
                var meetingSuggestions = await this.schedule.GetMeetingSuggestions(suggestMeetingsRequest);
                foreach (var meetingInfo in meetingSuggestions)
                {
                    createSchedulesTask.Add(this.CreateSchedule(meetingInfo, jobApplicationId));
                }

                createdSchedules = (await Task.WhenAll(createSchedulesTask)).ToList();
            }
            else
            {
                throw new UnauthorizedStatusException($"You are unauthorized to access meeting info").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.SuggestAndCreateSchedules)} method in {nameof(ScheduleServiceController)}.");
            return createdSchedules;
        }

        /// <summary>
        /// Update schedule api
        /// </summary>
        /// <param name="meetingInfo">The schedule object.</param>
        /// <returns>The response.</returns>
        [HttpPut("updateschedule")]
        [MonitorWith("GTAV1UpdateSchedule")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MeetingInfo> UpdateSchedule([FromBody] MeetingInfo meetingInfo)
        {
            MeetingInfo meetingInfoResult;
            this.logger.LogInformation($"Started {nameof(this.UpdateSchedule)} method in {nameof(ScheduleServiceController)}.");
            if (meetingInfo == null)
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid meeting info").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByScheduleId(meetingInfo.Id);
            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, string.Empty, meetingInfo.Id))
            {
                ////Schedule status as saved
                meetingInfoResult = await this.schedule.UpdateSchedule(meetingInfo);
            }
            else
            {
                throw new UnauthorizedStatusException("You are unauthorized to access schedule").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateSchedule)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfoResult;
        }

        /// <summary>
        /// Manually updates the schedule participant response for a schedule.
        /// This is required in Candidate Tracker which allows manually overriding the schedule response.
        /// </summary>
        /// <param name="scheduleID">The unique id of the schedule</param>
        /// <param name="participantOid">The office graph identifier of the participant</param>
        /// <param name="responseStatus">The new status of the schedule</param>
        /// <returns>The operation status</returns>
        [HttpPut("getschedulesbyjobid/{scheduleId}/{participantOid}/{responseStatus}")]
        [MonitorWith("GTAV1UpdateScheduleParticipantResponse")]
        public async Task<bool> UpdateScheduleParticipantResponse(string scheduleID, string participantOid, InvitationResponseStatus responseStatus)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleParticipantResponse)} method in {nameof(ScheduleServiceController)}.");

            // TODO: add validation
            await this.schedule.UpdateScheduleParticipantResponse(scheduleID, participantOid, responseStatus);

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleParticipantResponse)} method in {nameof(ScheduleServiceController)}.");
            return true;
        }

        /// <summary>
        /// GetSchedule By JobId
        /// </summary>
        /// <param name="jobApplicationId">JobApplicationId.</param>
        /// <returns>The response.</returns>
        [HttpGet("getschedulesbyjobid/{jobApplicationId}")]
        [MonitorWith("GTAV1GetScheduleByScheduleId")]
        public async Task<List<MeetingInfo>> GetSchedulesByJobApplicationId(string jobApplicationId)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetSchedulesByJobApplicationId)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrEmpty(jobApplicationId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByApplicationId(jobApplicationId);
            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserInJobApplicationParticipants(hcmContextUserId, jobApplicationId, string.Empty)
                || await this.roleManager.IsReadOnlyRole(hcmContextUserId))
            {
                ////Get schedule details by job id
                meetingInfos = await this.schedule.GetSchedulesByJobApplicationId(jobApplicationId);
            }
            else
            {
                throw new UnauthorizedStatusException("You are unauthorized to access schedule").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.GetSchedulesByJobApplicationId)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfos;
        }

        /// <summary>
        /// GetSchedule By ScheduleId
        /// </summary>
        /// <param name="scheduleId">The schedule id.</param>
        /// <returns>The response.</returns>
        [HttpGet("getschedulebyid/{scheduleId}")]
        [MonitorWith("GTAV1GetScheduleByScheduleId")]
        public async Task<MeetingInfo> GetScheduleByScheduleId(string scheduleId)
        {
            MeetingInfo meetingInfoResult;
            this.logger.LogInformation($"Started {nameof(this.GetScheduleByScheduleId)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByScheduleId(scheduleId);
            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserInJobApplicationParticipants(hcmContextUserId, string.Empty, scheduleId)
                || await this.roleManager.IsReadOnlyRole(hcmContextUserId))
            {
                ////Get schedule details by schedule id
                meetingInfoResult = await this.schedule.GetScheduleByScheduleId(scheduleId);
            }
            else
            {
                throw new UnauthorizedStatusException("You are unauthorized to access schedule").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.GetScheduleByScheduleId)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfoResult;
        }

        /// <summary>
        /// GetSchedules
        /// </summary>
        /// <param name="freeBusyRequest">The free Busy Request.</param>
        /// <param name="jobApplicationId">job Application Id</param>
        /// <returns>The response.</returns>
        [HttpPost("getschedulesbyfreebusyrequest/{jobApplicationId}")]
        [MonitorWith("GTAV1GetSchedulesByFreeBusyReq")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<List<MeetingInfo>> GetSchedulesByFreeBusyReq([FromBody] FreeBusyRequest freeBusyRequest, string jobApplicationId)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleServiceController)}.");
            if (freeBusyRequest == null)
            {
                throw new InvalidRequestDataValidationException($"Input request does not contain a valid free busy info").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByApplicationId(jobApplicationId);
            ////Get schedules by user id
            meetingInfos = await this.schedule.GetSchedulesByFreeBusyRequest(freeBusyRequest, jobApplicationId);
            this.logger.LogInformation($"Finished {nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfos;
        }

        /// <summary>
        /// GetSchedules
        /// </summary>
        /// <param name="freeBusyRequest">The free Busy Request.</param>
        /// <param name="jobApplicationId">job Application Id</param>
        /// <returns>The response.</returns>
        [HttpPost("getschedules/{jobApplicationId}")]
        [MonitorWith("GTAV1GetSchedules")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<List<MeetingInfo>> GetSchedulesByFreeBusyRequest([FromBody] FreeBusyRequest freeBusyRequest, string jobApplicationId)
        {
            List<MeetingInfo> meetingInfos;
            this.logger.LogInformation($"Started {nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleServiceController)}.");
            if (freeBusyRequest == null)
            {
                throw new BusinessRuleViolationException($"Input request does not contain a valid free busy info").EnsureLogged(this.logger);
            }

            ////Get schedules by user id
            meetingInfos = await this.schedule.GetSchedulesByFreeBusyRequest(freeBusyRequest, jobApplicationId);
            this.logger.LogInformation($"Finished {nameof(this.GetSchedulesByFreeBusyRequest)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfos;
        }

        /// <summary>
        /// Send schedule api
        /// </summary>
        /// <param name="scheduleId">The schedule object.</param>
        /// <param name="emailTemplate">Email Template</param>
        /// <returns>The response.</returns>
        [HttpPost("queueschedule/{scheduleId}")]
        [MonitorWith("GTAV1QueueSchedule")]
        public async Task<MeetingInfo> SendSchedule(string scheduleId, [FromBody] EmailTemplate emailTemplate)
        {
            MeetingInfo meetingInfoResult;
            this.logger.LogInformation($"Started {nameof(this.SendSchedule)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new BusinessRuleViolationException($"Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            // this check is redundant but doesn't interfere with CT use case since job application itself will be null.
            await this.schedule.ValidateApplicationByScheduleId(scheduleId);

            var hcmContextUserId = this.hcmServiceContext.UserId;
            var rootActivityId = this.hcmServiceContext.RootActivityId;
            meetingInfoResult = await this.schedule.QueueSchedule(scheduleId, ScheduleStatus.Queued, emailTemplate, rootActivityId, hcmContextUserId, this.hcmServiceContext.Email, false);

            // if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, string.Empty, scheduleId))
            // {
            //    bool isWobUser = this.roleManager.IsUserWobAuthenticated(this.hcmServiceContext.isWobAuthenticated);
            //    ////Schedule status as queued
            //    meetingInfoResult = await this.schedule.QueueSchedule(scheduleId, ScheduleStatus.Queued, emailTemplate, rootActivityId, hcmContextUserId, this.hcmServiceContext.Email, isWobUser);
            // }
            // else
            // {
            //    throw new UnauthorizedStatusException($"You are unauthorized to send invitation").EnsureLogged(this.logger);
            // }
            this.logger.LogInformation($"Finished {nameof(this.SendSchedule)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfoResult;
        }

        /// <summary>
        /// Resend schedule api
        /// </summary>
        /// <param name="scheduleId">schedule id</param>
        /// <param name="serviceAccountName">service accountName</param>
        /// <returns>Task response</returns>
        [HttpPost("resendschedule/{scheduleId}")]
        [MonitorWith("GTAV1ReSendSchedule")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> ReSendSchedule(string scheduleId, string serviceAccountName = null)
        {
            this.logger.LogInformation($"Started {nameof(this.ReSendSchedule)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrWhiteSpace(scheduleId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByScheduleId(scheduleId);
            bool result = await this.schedule.ReSendSchedule(scheduleId, serviceAccountName);

            this.logger.LogInformation($"Finished {nameof(this.SendSchedule)} method in {nameof(ScheduleServiceController)}.");

            return result;
        }

        /// <summary>
        /// Update Schedule Status
        /// </summary>
        /// <param name="scheduleId">The schedule object.</param>
        /// <param name="status">The schedule object status.</param>
        /// <returns>The response.</returns>
        [HttpPut("updateschedulestatus/{scheduleId}/{status}")]
        [MonitorWith("GTAV1UpdateScheduleStatus")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MeetingInfo> UpdateScheduleStatus(string scheduleId, ScheduleStatus status)
        {
            MeetingInfo meetingInfoResult;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleStatus)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            ////Update schedule status
            meetingInfoResult = await this.schedule.UpdateScheduleStatus(scheduleId, status);
            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleStatus)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfoResult;
        }

        /// <summary>
        /// Update Schedule Service Account
        /// </summary>
        /// <param name="scheduleId">The schedule object.</param>
        /// <param name="serviceAccountName">The service Account Name.</param>
        /// <returns>The response.</returns>
        [HttpPut("updatescheduleserviceaccount/{scheduleId}/{serviceAccountName}")]
        [MonitorWith("GTAV1UpdateScheduleServiceAccount")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MeetingInfo> UpdateScheduleServiceAccount(string scheduleId, string serviceAccountName)
        {
            MeetingInfo meetingInfoResult;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleServiceAccount)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            ////Update schedule service account details
            meetingInfoResult = await this.schedule.UpdateScheduleServiceAccountAsync(scheduleId, serviceAccountName);
            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleServiceAccount)} method in {nameof(ScheduleServiceController)}.");
            return meetingInfoResult;
        }

        /// <summary>
        /// Delete schedule api
        /// </summary>
        /// <param name="scheduleId">The schedule object.</param>
        /// <returns>The response.</returns>
        [HttpDelete("deleteschedule/{scheduleId}")]
        [MonitorWith("GTAV1DeleteSchedule")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task DeleteSchedule(string scheduleId)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteSchedule)} method in {nameof(ScheduleServiceController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            var hcmContextUserId = this.hcmServiceContext.UserId;
            var rootActivityId = this.hcmServiceContext.RootActivityId;
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, string.Empty, scheduleId))
            {
                ////Schedule status as delete
                await this.schedule.DeleteSchedule(scheduleId, rootActivityId);
            }
            else
            {
                throw new UnauthorizedStatusException("You are unauthorized to delete schedule").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.DeleteSchedule)} method in {nameof(ScheduleServiceController)}.");
        }

        /// <summary>
        /// Sends the interview invitation to applicant.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <returns>The instance of <see cref="Task{IActionResult}"/> representing an asynchronous operation.</returns>
        /// <response code="200">No response body</response>
        /// <response code="400">If the job application id is missing or schedule invitation request is invalid or null.</response>
        [HttpPost]
        [Route("{jobApplicationId}/selectiveinvite")]
        [MonitorWith("HcmAttSApplicantInvt")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SendInterviewInvitationToApplicant(string jobApplicationId, [FromBody] ScheduleInvitationRequest scheduleInvitationRequest)
        {
            this.logger.LogInformation($"Started {nameof(this.SendInterviewInvitationToApplicant)} method in {nameof(ScheduleServiceController)}.");
            if (scheduleInvitationRequest.SharedSchedules == null)
            {
                throw new BusinessRuleViolationException($"This candidate has no interview schedule associated, unable to perform this action").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByApplicationId(jobApplicationId);
            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, jobApplicationId, string.Empty))
            {
                bool isWobUser = this.roleManager.IsUserWobAuthenticated(this.hcmServiceContext.isWobAuthenticated);
                await this.schedule.SendInterviewScheduleToApplicantAsync(jobApplicationId, scheduleInvitationRequest, this.hcmServiceContext?.Email, hcmContextUserId, isWobUser).ConfigureAwait(false);
            }
            else
            {
                throw new UnauthorizedStatusException($"You are unauthorized to send invitation").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendInterviewInvitationToApplicant)} method in {nameof(ScheduleServiceController)}.");
            return this.Ok();
        }

        /// <summary>
        /// Sends the interview invitation to applicant.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="scheduleInvitationRequestV2">The instance of <see cref="ScheduleInvitationRequestV2"/>.</param>
        /// <returns>The instance of <see cref="Task{IActionResult}"/> representing an asynchronous operation.</returns>
        /// <response code="200">No response body</response>
        /// <response code="400">If the job application id is missing or schedule invitation request is invalid or null.</response>
        [HttpPost]
        [Route("{jobApplicationId}/selectiveinviteV2")]
        [MonitorWith("HcmAttSApplicantInvtV2")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SendInterviewInvitationToApplicantV2(string jobApplicationId, [FromForm] ScheduleInvitationRequestV2 scheduleInvitationRequestV2)
        {
            ScheduleInvitationRequest scheduleInvitationRequest = this.CreateScheduleInvitationRequest(scheduleInvitationRequestV2);
            this.logger.LogInformation($"Started {nameof(this.SendInterviewInvitationToApplicantV2)} method in {nameof(ScheduleServiceController)}.");
            if (scheduleInvitationRequest.SharedSchedules == null)
            {
                throw new BusinessRuleViolationException($"This candidate has no interview schedule associated, unable to perform this action").EnsureLogged(this.logger);
            }

            await this.schedule.ValidateApplicationByApplicationId(jobApplicationId);
            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, jobApplicationId, string.Empty))
            {
                bool isWobUser = this.roleManager.IsUserWobAuthenticated(this.hcmServiceContext.isWobAuthenticated);
                await this.schedule.SendInterviewScheduleToApplicantAsync(jobApplicationId, scheduleInvitationRequest, this.hcmServiceContext?.Email, hcmContextUserId, isWobUser).ConfigureAwait(false);
            }
            else
            {
                throw new UnauthorizedStatusException($"You are unauthorized to send invitation").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendInterviewInvitationToApplicantV2)} method in {nameof(ScheduleServiceController)}.");
            return this.Ok();
        }

        /// <summary>
        /// Creates a scheduleInvitationRequest object from scheduleInvitationRequestV2 object.
        /// </summary>
        /// <param name="data">scheduleInvitationDetailsV2 object</param>
        /// <returns>An instance of <see cref="ScheduleInvitationRequest"/>.</returns>
        private ScheduleInvitationRequest CreateScheduleInvitationRequest(ScheduleInvitationRequestV2 data)
        {
            if (data == null)
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid data object").EnsureLogged(this.logger);
            }

            var schedules = data.SharedSchedulesScheduleId;
            var isIVSchedShared = data.SharedSchedulesIsInterviewScheduleShared;
            var isIVrNameShared = data.SharedSchedulesIsInterviewerNameShared;

            if (schedules == null || isIVSchedShared == null || isIVrNameShared == null)
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid sharedSchedules list").EnsureLogged(this.logger);
            }

            if ((schedules.Count != isIVSchedShared.Count) || (schedules.Count != isIVrNameShared.Count))
            {
                throw new InvalidRequestDataValidationException("Count mismatch: Input request does not contain a valid sharedSchedules list").EnsureLogged(this.logger);
            }
            else
            {
                var zip = schedules.Zip(isIVSchedShared, (scheduleID, isIVScheduleShared) => new { scheduleID, isIVScheduleShared })
                    .Zip(isIVrNameShared, (pair, isIVNameShared) => new { scheduleID = pair.scheduleID, isIVScheduleShared = pair.isIVScheduleShared, isIVNameShared = isIVNameShared });

                List<CandidateScheduleCommunication> sharedSchedules = new List<CandidateScheduleCommunication>();

                foreach (var entry in zip)
                {
                    sharedSchedules.Add(new CandidateScheduleCommunication
                    {
                        ScheduleId = entry.scheduleID,
                        IsInterviewScheduleShared = entry.isIVScheduleShared,
                        IsInterviewerNameShared = entry.isIVNameShared
                    });
                }

                return new ScheduleInvitationRequest
                {
                    Subject = data.Subject,
                    CcEmailAddressList = data.CcEmailAddressList,
                    EmailAttachments = new FileAttachmentRequest { Files = data.EmailAttachmentFiles, FileLabels = data.EmailAttachmentFileLabels },
                    EmailContent = data.EmailContent,
                    IsInterviewScheduleShared = data.IsInterviewScheduleShared,
                    IsInterviewTitleShared = data.IsInterviewTitleShared,
                    PrimaryEmailRecipient = data.PrimaryEmailRecipient,
                    SharedSchedules = sharedSchedules
                };
            }
        }

        //// TODO : To be removed to clean up teams meeting code.
        private string GetToken()
        {
            string auth;
            string token = null;
            if (this.HttpContext.Request.Headers.ContainsKey("Authorization")
                && this.HttpContext.Request.Headers["Authorization"].Any())
            {
                auth = this.HttpContext.Request.Headers["Authorization"][0];
                if (auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = auth.Split(" ")[1];
                }
            }

            return token;
        }
    }
}
