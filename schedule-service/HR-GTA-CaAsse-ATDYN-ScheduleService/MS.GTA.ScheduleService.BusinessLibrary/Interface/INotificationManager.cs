// <copyright file="INotificationManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using System.Threading.Tasks;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.Contracts.V1;

    /// <summary>
    /// Notification Manager interface
    /// </summary>
    public interface INotificationManager
    {
        /// <summary>
        /// Process notification requests
        /// </summary>
        /// <param name="notificationContent">notification Content</param>
        /// <returns>Task for processing pending requests</returns>
        Task<bool> ProcessNotificationContent(NotificationContent notificationContent);

        /// <summary>
        /// Process user responses for schedule
        /// </summary>
        /// <param name="scheduleId">Schedule Id content</param>
        /// <returns>status of process</returns>
        Task<bool> ProcessScheduleResponse(string scheduleId);

        /// <summary>
        /// Process schedule responses for recovering responses lost during disaster.
        /// </summary>
        /// <returns>status of process</returns>
        Task ProcessScheduleResponse();
    }
}
