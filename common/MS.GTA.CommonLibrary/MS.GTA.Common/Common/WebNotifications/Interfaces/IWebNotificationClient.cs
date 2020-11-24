// <copyright file="IWebNotificationClient.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.WebNotifications.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.Common.WebNotifications.Models;

    /// <summary>
    /// The <see cref="IWebNotificationClient"/> interface provides mechanism to send web notifications.
    /// </summary>
    public interface IWebNotificationClient
    {
        /// <summary>
        /// Posts the notifications the service.
        /// </summary>
        /// <param name="webNotificationRequests">The instance for <see cref="IEnumerable{WebNotificationRequest}"/>.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        Task PostNotificationsAsync(IEnumerable<WebNotificationRequest> webNotificationRequests);
    }
}
