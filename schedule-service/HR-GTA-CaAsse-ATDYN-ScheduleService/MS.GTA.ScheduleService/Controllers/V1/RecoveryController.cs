//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using CommonLibrary.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Controller for features related to Recover app or disaster recovery related activities.
    /// </summary>
    [Route("v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RecoveryController : HCMWebApiAuthenticatedController
    {
        private readonly INotificationManager notificationManager;
        private readonly ILogger<RecoveryController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoveryController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The instance for <see cref="IHttpContextAccessor"/></param>
        /// <param name="notificationManager">The instance for <see cref="INotificationManager"/></param>
        /// <param name="logger">The instance for <see cref="ILogger{MeetingController}"/></param>
        public RecoveryController(IHttpContextAccessor httpContextAccessor, INotificationManager notificationManager, ILogger<RecoveryController> logger)
            : base(httpContextAccessor)
        {
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Process schedule responses for recovering responses lost during disaster.
        /// </summary>
        /// <returns>A task.</returns>
        [HttpPost("processScheduleResponses")]
        [MonitorWith("GTAV1RecoveryProcessScheduleResponses")]
        public async Task ProcessScheduleResponses()
        {
            this.logger.LogInformation($"Started {nameof(this.ProcessScheduleResponses)} method in {nameof(RecoveryController)}.");

            await this.notificationManager.ProcessScheduleResponse();
            this.logger.LogInformation($"Finished {nameof(this.ProcessScheduleResponses)} method in {nameof(RecoveryController)}.");
        }
    }
}
