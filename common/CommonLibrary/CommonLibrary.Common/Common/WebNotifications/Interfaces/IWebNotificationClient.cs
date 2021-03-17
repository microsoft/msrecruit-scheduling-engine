//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.WebNotifications.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CommonLibrary.Common.WebNotifications.Models;

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
