﻿// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="INotificationClient.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Notification
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Email.GraphContracts;
    using MS.GTA.Common.Email.SendGridContracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.EnumSetModel.SchedulingService;

    /// <summary>The Notification client interface.</summary>
    public interface INotificationClient
    {
        /// <summary>Transform from email address with HCM domain .</summary>
        /// <param name="email">The email.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string TransformFromEmail(string email);

        /// <summary>
        /// send e-mail notification item
        /// </summary>
        /// <param name="notificationItems">notification items</param>
        /// <returns>response</returns>
        Task<bool> SendEmail(List<NotificationItem> notificationItems);
    }
}