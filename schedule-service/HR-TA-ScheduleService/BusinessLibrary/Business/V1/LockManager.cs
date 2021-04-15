//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Business.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ServicePlatform.Tracing;

    /// <summary>
    /// Lock manager
    /// </summary>
    public class LockManager : ILockManager
    {
        /// <summary>
        /// schedule query
        /// </summary>
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>
        /// The instance for <see cref="ILogger{LockManager}"/>.
        /// </summary>
        private readonly ILogger<LockManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockManager"/> class
        /// </summary>
        /// <param name="query">schedule query instance</param>
        /// <param name="logger">The instance for <see cref="ILogger{LockManager}"/>.</param>
        public LockManager(IScheduleQuery query, ILogger<LockManager> logger)
        {
            this.scheduleQuery = query;
            this.logger = logger;
        }

        /// <summary>
        /// Gets gets or sets the tracer instance
        /// </summary>
        private ITraceSource Trace => ScheduleServiceBusinessLibraryTracer.Instance;

        /// <summary>
        /// GetItemsAsync based on schedule id
        /// </summary>
        /// <param name="scheduleID">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        public async Task<List<SendInvitationLock>> GetScheduleLockItems(IList<string> scheduleID)
        {
            this.logger.LogInformation($"Started {nameof(this.GetScheduleLockItems)} method in {nameof(LockManager)}.");
            List<SendInvitationLock> sendInvitationLocks = await this.scheduleQuery.GetScheduleLockItems(scheduleID);
            this.logger.LogInformation($"Finished {nameof(this.GetScheduleLockItems)} method in {nameof(LockManager)}.");
            return sendInvitationLocks;
        }

        /// <summary>
        /// Create ItemsAsync based on schedule id
        /// </summary>
        /// <param name="invitationLock">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        public async Task CreateScheduleLockItem(SendInvitationLock invitationLock)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateScheduleLockItem)} method in {nameof(LockManager)}.");
            await this.scheduleQuery.CreateScheduleLockItem(invitationLock);
            this.logger.LogInformation($"Finished {nameof(this.CreateScheduleLockItem)} method in {nameof(LockManager)}.");
        }

        /// <summary>
        /// Delete SendInvitationLock items
        /// </summary>
        /// <param name="scheduleIDs">scheduleids</param>
        /// <returns>result</returns>
        public async Task DeleteScheduleLockItems(IList<string> scheduleIDs)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteScheduleLockItems)} method in {nameof(LockManager)}.");
            await this.scheduleQuery.DeleteScheduleLockItems(scheduleIDs);
            this.logger.LogInformation($"Finished {nameof(this.DeleteScheduleLockItems)} method in {nameof(LockManager)}.");
        }

        /// <summary>
        /// Delete SendInvitationLock item
        /// </summary>
        /// <param name="lockID">lockID</param>
        /// <returns>result</returns>
        public async Task DeleteScheduleLockItem(string lockID)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteScheduleLockItem)} method in {nameof(LockManager)}.");

            await this.scheduleQuery.DeleteScheduleLockItem(lockID);
            this.logger.LogInformation($"Finished {nameof(this.DeleteScheduleLockItem)} method in {nameof(LockManager)}.");
        }

        /// <summary>
        /// Create notification lock item with notificationmessagelock item
        /// </summary>
        /// <param name="notificationLock">notification lock</param>
        /// <returns>result</returns>
        public async Task CreateNotificationLockItem(NotificationMessageLock notificationLock)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateNotificationLockItem)} method in {nameof(LockManager)}.");

            await this.scheduleQuery.CreateNotificationLockItem(notificationLock);
            this.logger.LogInformation($"Finished {nameof(this.CreateNotificationLockItem)} method in {nameof(LockManager)}.");
        }

        /// <summary>
        /// Get notification lock item with servicebusmessageid
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        public async Task<NotificationMessageLock> GetNotificationLockItem(string serviceBusMessageId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetNotificationLockItem)} method in {nameof(LockManager)}.");

            return await this.scheduleQuery.GetNotificationLockItem(serviceBusMessageId);
        }

        /// <summary>
        /// Delete NotificationLockItem with servicebusmessageid
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        public async Task DeleteNotificationLockItem(string serviceBusMessageId)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteNotificationLockItem)} method in {nameof(LockManager)}.");

            await this.scheduleQuery.DeleteNotificationLockItem(serviceBusMessageId);
            this.logger.LogInformation($"Finished {nameof(this.DeleteNotificationLockItem)} method in {nameof(LockManager)}.");
        }
    }
}
