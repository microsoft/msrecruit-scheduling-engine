// <copyright file="CandidateCommunicatorMakers.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.NotifyCandidate
{
    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.Data.DataProviders;

    /// <summary>
    /// The <see cref="CandidateCommunicatorMakers"/> stores object references essential to construct <see cref="CandidateCommunicator"/>.
    /// </summary>
    internal class CandidateCommunicatorMakers
    {
        /// <summary>
        /// Gets or sets the requester email address.
        /// </summary>
        /// <value>
        /// The requester email address.
        /// </value>
        public string RequesterEmail { get; set; }

        /// <summary>
        /// Gets or sets the email helper.
        /// </summary>
        /// <value>
        /// The instance for <see cref="IEmailHelper"/>.
        /// </value>
        public IEmailHelper EmailHelper { get; set; }

        /// <summary>
        /// Gets or sets the schedule query.
        /// </summary>
        /// <value>
        /// The The instance for <see cref="IScheduleQuery"/>.
        /// </value>
        public IScheduleQuery ScheduleQuery { get; set; }

        /// <summary>
        /// Gets or sets the notification client.
        /// </summary>
        /// <value>
        /// The instance for <see cref="INotificationClient"/>.
        /// </value>
        public INotificationClient NotificationClient { get; set; }
    }
}
