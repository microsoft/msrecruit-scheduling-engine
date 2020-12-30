//----------------------------------------------------------------------------
// <copyright file="LockController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// Lock controller
    /// </summary>
    [Route("v1/lock")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LockController : HCMWebApiAuthenticatedController
    {
        private readonly ILockManager lockManager;

        /// <summary>
        /// holds servicecontext
        /// </summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary>
        /// The instance for <see cref="ILogger{LockController}"/>.
        /// </summary>
        private readonly ILogger<LockController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="serviceContext">service context</param>
        /// <param name="locManager">lock manager instance</param>
        /// <param name="logger">The instance for <see cref="ILogger{LockController}"/>.</param>
        public LockController(
            IHttpContextAccessor httpContextAccessor,
            IHCMServiceContext serviceContext,
            ILockManager locManager,
            ILogger<LockController> logger)
                : base(httpContextAccessor)
        {
            this.hcmServiceContext = serviceContext;
            this.lockManager = locManager;
            this.logger = logger;
        }

        /// <summary>
        /// Get SendInvitationLock based on scheduleid
        /// </summary>
        /// <param name="scheduleIds">schedule IDs.</param>
        /// <returns>Send invitation locks.</returns>
        [HttpPost("getinvitationlocks")]
        [MonitorWith("GTAV1GetInvitationLocks")]
        public async Task<List<SendInvitationLock>> GetInvitationLocks([FromBody]IList<string> scheduleIds)
        {
            List<SendInvitationLock> invitationLocks;
            this.logger.LogInformation($"Started {nameof(this.GetInvitationLocks)} method in {nameof(LockController)}.");

            if (scheduleIds == null || scheduleIds.Count <= 0)
            {
                throw new InvalidRequestDataValidationException("GetSendInvitationLocks -- scheduleIDs cannot be null").EnsureLogged(this.logger);
            }

            invitationLocks = await this.lockManager.GetScheduleLockItems(scheduleIds);
            this.logger.LogInformation($"Finished {nameof(this.GetInvitationLocks)} method in {nameof(LockController)}.");
            return invitationLocks;
        }

        /// <summary>
        /// Create SendInvitationLock with scheduleid
        /// </summary>
        /// <param name="invitationLock">schedule ID.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpPost("createinvitationlock")]
        [MonitorWith("GTAV1CreateInvitationLock")]
        public async Task CreateInvitationLocks([FromBody]SendInvitationLock invitationLock)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateInvitationLocks)} method in {nameof(LockController)}.");

            if (invitationLock == null || string.IsNullOrEmpty(invitationLock?.ScheduleId))
            {
                throw new InvalidRequestDataValidationException("CreateInvitationLocks -- scheduleID cannot be null").EnsureLogged(this.logger);
            }

            await this.lockManager.CreateScheduleLockItem(invitationLock);
            this.logger.LogInformation($"Finished {nameof(this.CreateInvitationLocks)} method in {nameof(LockController)}.");
        }

        /// <summary>
        /// Delete SendInvitationLock with scheduleids
        /// </summary>
        /// <param name="scheduleIds">schedule IDs.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpDelete("deleteinvitationlocks")]
        [MonitorWith("GTAV1DeleteInvitationLocks")]
        public async Task DeleteInvitationLocks([FromBody]IList<string> scheduleIds)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteInvitationLocks)} method in {nameof(LockController)}.");
            if (scheduleIds == null || scheduleIds.Count <= 0)
            {
                throw new InvalidRequestDataValidationException("DeleteInvitationLocks -- scheduleID cannot be null").EnsureLogged(this.logger);
            }

            await this.lockManager.DeleteScheduleLockItems(scheduleIds);
            this.logger.LogInformation($"Finished {nameof(this.DeleteInvitationLocks)} method in {nameof(LockController)}.");
        }

        /// <summary>
        /// Delete SendInvitationLock with lockID
        /// </summary>
        /// <param name="lockID">lockID.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpDelete("deleteinvitationlock/{lockID}")]
        [MonitorWith("GTAV1DeleteInvitationLocks")]
        public async Task DeleteInvitationLock(string lockID)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteInvitationLock)} method in {nameof(LockController)}.");

            if (string.IsNullOrEmpty(lockID))
            {
                throw new InvalidRequestDataValidationException("DeleteInvitationLocks -- lockID cannot be null").EnsureLogged(this.logger);
            }

            await this.lockManager.DeleteScheduleLockItem(lockID);
        }

        /// <summary>
        /// Create notificationLock with notification message lock item
        /// </summary>
        /// <param name="notificationLock">schedule ID.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpPost("createnotificationlock")]
        [MonitorWith("GTAV1CreateNotificationLock")]
        public async Task CreateNotificationLock([FromBody]NotificationMessageLock notificationLock)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateNotificationLock)} method in {nameof(LockController)}.");

            if (notificationLock == null || string.IsNullOrEmpty(notificationLock?.ServiceBusMessageId))
            {
                throw new InvalidRequestDataValidationException("CreateNotificationLock -- ServiceBusMessageId cannot be null").EnsureLogged(this.logger);
            }

            await this.lockManager.CreateNotificationLockItem(notificationLock);
            this.logger.LogInformation($"Finished {nameof(this.CreateNotificationLock)} method in {nameof(LockController)}.");
        }

        /// <summary>
        /// Get notificationLock with ServiceBusMessageId
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpGet("getnotificationlock/{serviceBusMessageId}")]
        [MonitorWith("GTAV1GetNotificationLock")]
        public async Task<NotificationMessageLock> GetNotificationLock(string serviceBusMessageId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetNotificationLock)} method in {nameof(LockController)}.");

            if (string.IsNullOrEmpty(serviceBusMessageId))
            {
                throw new InvalidRequestDataValidationException("GetNotificationLock -- serviceBusMessageId cannot be null").EnsureLogged(this.logger);
            }

            var result = await this.lockManager.GetNotificationLockItem(serviceBusMessageId);
            this.logger.LogInformation($"Finished {nameof(this.GetNotificationLock)} method in {nameof(LockController)}.");
            return result;
        }

        /// <summary>
        /// deletenotificationlock with serviceBusMessageId
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpDelete("deletenotificationlock/{serviceBusMessageId}")]
        [MonitorWith("GTAV1DeleteInvitationLocks")]
        public async Task DeleteNotificationLock(string serviceBusMessageId)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteNotificationLock)} method in {nameof(LockController)}.");

            if (string.IsNullOrEmpty(serviceBusMessageId))
            {
                throw new InvalidRequestDataValidationException("DeleteNotificationLock -- serviceBusMessageId cannot be null").EnsureLogged(this.logger);
            }

            await this.lockManager.DeleteNotificationLockItem(serviceBusMessageId);
            this.logger.LogInformation($"Finished {nameof(this.DeleteNotificationLock)} method in {nameof(LockController)}.");
        }
    }
}
