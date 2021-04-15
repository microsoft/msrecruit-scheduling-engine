//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// LockManager interface
    /// </summary>
    public interface ILockManager
    {
        /// <summary>
        /// GetItemsAsync based on schedule id
        /// </summary>
        /// <param name="scheduleIDs">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        Task<List<SendInvitationLock>> GetScheduleLockItems(IList<string> scheduleIDs);

        /// <summary>
        /// GetItemsAsync based on schedule id
        /// </summary>
        /// <param name="scheduleID">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        Task CreateScheduleLockItem(SendInvitationLock scheduleID);

        /// <summary>
        /// Delete SendInvitationLock items
        /// </summary>
        /// <param name="scheduleIDs">scheduleid</param>
        /// <returns>result</returns>
        Task DeleteScheduleLockItems(IList<string> scheduleIDs);

        /// <summary>
        /// Delete SendInvitationLock item
        /// </summary>
        /// <param name="lockID">lockID</param>
        /// <returns>result</returns>
        Task DeleteScheduleLockItem(string lockID);

        /// <summary>
        /// Create notification lock item with notificationmessagelock item
        /// </summary>
        /// <param name="notificationLock">notification lock</param>
        /// <returns>result</returns>
        Task CreateNotificationLockItem(NotificationMessageLock notificationLock);

        /// <summary>
        /// Get notification lock with servicebusmessageid
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        Task<NotificationMessageLock> GetNotificationLockItem(string serviceBusMessageId);

        /// <summary>
        /// Delete NotificationLockItem with servicebusmessageid
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        Task DeleteNotificationLockItem(string serviceBusMessageId);
    }
}
