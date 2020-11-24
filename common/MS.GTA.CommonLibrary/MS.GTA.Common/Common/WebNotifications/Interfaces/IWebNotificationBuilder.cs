// <copyright file="IWebNotificationBuilder.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.WebNotifications.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.Common.WebNotifications.Models;

    /// <summary>
    /// The <see cref="IWebNotificationBuilder"/> interface provides mechanism to build the web notifications.
    /// </summary>
    public interface IWebNotificationBuilder
    {
        /// <summary>
        /// Builds the web notifications.
        /// </summary>
        /// <param name="notificationDataExtractor">The instance for <see cref="IWebNotificationDataExtractor"/>.</param>
        /// <param name="templateProvider">The instance for <see cref="IWebNotificationTemplateProvider"/>.</param>
        /// <returns>The instance of <see cref="IEnumerable{WebNotificationRequest}"/> in <see cref="Task"/>.</returns>
        Task<IEnumerable<WebNotificationRequest>> Build(IWebNotificationDataExtractor notificationDataExtractor, IWebNotificationTemplateProvider templateProvider);
    }
}
