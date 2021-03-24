//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using CommonLibrary.Common.Base.ServiceContext;
    using CommonLibrary.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// The Meeting Controller.
    /// </summary>
    [Route("v1/schedulemeeting")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MeetingController : HCMWebApiAuthenticatedController
    {
        /// <summary>
        /// holds read only schedule value
        /// </summary>
        private readonly IScheduleManager schedule;
        private readonly INotificationManager notificationManager;

        /// <summary>
        /// The instance for <see cref="ILogger{MeetingController}"/>.
        /// </summary>
        private readonly ILogger<MeetingController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="scheduler"> Scheduler interface </param>
        /// <param name="notificationManager">Notification Manager</param>
        /// <param name="logger">The instance for <see cref="ILogger{MeetingController}"/>.</param>
        public MeetingController(
            IHttpContextAccessor httpContextAccessor,
            IScheduleManager scheduler,
            INotificationManager notificationManager,
            ILogger<MeetingController> logger)
                : base(httpContextAccessor)
        {
            this.notificationManager = notificationManager;
            this.schedule = scheduler;
            this.logger = logger;
        }

        /// <summary>
        /// Sends calendar invite.
        /// </summary>
        /// <param name="serviceAccountName">Service Account Name</param>
        /// <param name="scheduleIds">schedule Ids to be processed</param>
        /// <returns>Successfully processed schedule Ids.</returns>
        [HttpPost("sendcalendarinvite")]
        [MonitorWith("GTAV1SendCalendarInvite")]
        public async Task<IList<string>> SendCalendarInvite(string serviceAccountName, [FromBody] IList<string> scheduleIds)
        {
            this.logger.LogInformation($"Started {nameof(this.SendCalendarInvite)} method in {nameof(MeetingController)}.");
            if (!scheduleIds?.Any() ?? true)
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            scheduleIds = scheduleIds?.Distinct()?.ToList();
            scheduleIds = await this.schedule.SendCalendarEvent(scheduleIds, serviceAccountName);

            this.logger.LogInformation($"Finished {nameof(this.SendCalendarInvite)} method in {nameof(MeetingController)}.");
            return scheduleIds;
        }

        /// <summary>
        /// Process notification
        /// </summary>
        /// <param name="notificationContent">notification content</param>
        /// <returns>A http 202 response</returns>
        [HttpPost("processNotification")]
        [MonitorWith("GTAV1ProcessNotification")]
        public async Task<ActionResult> ProcessNotification([FromBody] NotificationContent notificationContent)
        {
            this.logger.LogInformation($"Started {nameof(this.ProcessNotification)} method in {nameof(MeetingController)}.");
            if (notificationContent == null)
            {
                throw new InvalidRequestDataValidationException("Input request does not contain notification content").EnsureLogged(this.logger);
            }

            await this.notificationManager.ProcessNotificationContent(notificationContent);
            this.logger.LogInformation($"Finished {nameof(this.ProcessNotification)} method in {nameof(MeetingController)}.");
            return this.Accepted();
        }

        /// <summary>
        /// Process schedule responses
        /// </summary>
        /// <param name="scheduleId">schedule id</param>
        /// <returns>A http 202 response</returns>
        [HttpPost("ProcessScheduleResponses/{scheduleId}")]
        [MonitorWith("GTAV1ProcessScheduleResponses")]
        public async Task ProcessScheduleResponses(string scheduleId)
        {
            this.logger.LogInformation($"Started {nameof(this.ProcessScheduleResponses)} method in {nameof(MeetingController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule Id").EnsureLogged(this.logger);
            }

            await this.notificationManager.ProcessScheduleResponse(scheduleId);
            this.logger.LogInformation($"Finished {nameof(this.ProcessScheduleResponses)} method in {nameof(MeetingController)}.");
        }
    }
}
