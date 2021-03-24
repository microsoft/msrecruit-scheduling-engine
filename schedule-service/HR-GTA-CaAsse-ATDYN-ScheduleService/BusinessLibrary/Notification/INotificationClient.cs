//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Notification
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using CommonLibrary.Common.Common.Common.Email.Contracts;
    using CommonLibrary.Common.Email.GraphContracts;
    using CommonLibrary.Common.Email.SendGridContracts;
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
        /// <param name="trackingID">Schedule ID/JobApplication ID</param>
        /// <returns>response</returns>
        Task<bool> SendEmail(List<NotificationItem> notificationItems, string trackingID);
    }
}
