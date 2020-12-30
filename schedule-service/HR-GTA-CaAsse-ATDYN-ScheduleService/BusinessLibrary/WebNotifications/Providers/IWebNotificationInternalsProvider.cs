// <copyright file="IWebNotificationInternalsProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Providers
{
    using System.Collections.Generic;
    using MS.GTA.Common.WebNotifications.Interfaces;
    using MS.GTA.ScheduleService.Data.Models;

    /// <summary>
    /// The <see cref="IWebNotificationInternalsProvider"/> interface provides a mechanism for internal objects factory.
    /// </summary>
    public interface IWebNotificationInternalsProvider
    {
        /// <summary>
        /// Gets the invite status update web notification data extractor.
        /// </summary>
        /// <param name="interviewerInviteResponseInfos">The instance for <see cref="IEnumerable{InterviewerInviteResponseInfo}"/>.</param>
        /// <returns>The instance for <see cref="IWebNotificationDataExtractor"/>.</returns>
        IWebNotificationDataExtractor GetInviteStatusUpdateWebNotificationDataExtractor(IEnumerable<InterviewerInviteResponseInfo> interviewerInviteResponseInfos);

        /// <summary>
        /// Gets the invite status update web notification template provider.
        /// </summary>
        /// <returns>The instance for <see cref="IWebNotificationTemplateProvider"/>.</returns>
        IWebNotificationTemplateProvider GetInviteStatusUpdateWebNotificationTemplateProvider();
    }
}
