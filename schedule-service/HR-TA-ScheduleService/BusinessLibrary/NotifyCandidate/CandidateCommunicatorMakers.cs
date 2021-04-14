//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.NotifyCandidate
{
    using HR.TA.Common.Base.Security.V2;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.Data.DataProviders;

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
