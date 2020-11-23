// <copyright file="InternalsProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MS.GTA.ScheduleService.UnitTest, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67" +
    "871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0b" +
    "d333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307" +
    "e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c3" +
    "08055da9")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Globalization;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.Common.MSGraph.Configuration;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.NotifyCandidate;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>
    /// The <see cref="InternalsProvider"/> implements mechanism to provision internal objects.
    /// </summary>
    /// <seealso cref="IInternalsProvider" />
    public class InternalsProvider : IInternalsProvider
    {
        /// <summary>
        /// The notification client
        /// </summary>
        private readonly INotificationClient notificationClient;

        /// <summary>
        /// The schedule query
        /// </summary>
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>
        /// The logger factory.
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The email helper
        /// </summary>
        private readonly IEmailHelper emailHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalsProvider"/> class.
        /// </summary>
        /// <param name="notificationClient">The instance for <see cref="INotificationClient"/>.</param>
        /// <param name="scheduleQuery">The instance for <see cref="IScheduleQuery"/>.</param>
        /// <param name="emailHelper">The instance for <see cref="IEmailHelper"/>.</param>
        /// <param name="emailClient">The instance for <see cref="IEmailClient"/>.</param>
        /// <param name="configurationManager">The instance for <see cref="IConfigurationManager"/>.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public InternalsProvider(
            INotificationClient notificationClient,
            IScheduleQuery scheduleQuery,
            IEmailHelper emailHelper,
            IEmailClient emailClient,
            IConfigurationManager configurationManager,
            ILoggerFactory loggerFactory)
        {
            CommonDataService.Common.Internal.Contract.CheckValue(notificationClient, nameof(notificationClient));
            CommonDataService.Common.Internal.Contract.CheckValue(scheduleQuery, nameof(scheduleQuery));
            CommonDataService.Common.Internal.Contract.CheckValue(emailHelper, nameof(emailHelper));
            CommonDataService.Common.Internal.Contract.CheckValue(emailClient, nameof(emailClient));
            CommonDataService.Common.Internal.Contract.CheckValue(configurationManager, nameof(configurationManager));
            CommonDataService.Common.Internal.Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            this.notificationClient = notificationClient;
            this.scheduleQuery = scheduleQuery;
            this.emailHelper = emailHelper;
            this.loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Gets the candidate communicator.
        /// </summary>
        /// <param name="requesterEmail">The requester email address.</param>
        /// <returns>
        /// The instance for <see cref="ICandidateCommunicator" />.
        /// </returns>
        public ICandidateCommunicator GetCandidateCommunicator(string requesterEmail)
        {
            CandidateCommunicatorMakers communicatorMakers = new CandidateCommunicatorMakers
            {
                RequesterEmail = requesterEmail,
                EmailHelper = this.emailHelper,
                ScheduleQuery = this.scheduleQuery,
                NotificationClient = this.notificationClient
            };

            return new CandidateCommunicator(communicatorMakers, this.loggerFactory.CreateLogger<CandidateCommunicator>());
        }
    }
}
