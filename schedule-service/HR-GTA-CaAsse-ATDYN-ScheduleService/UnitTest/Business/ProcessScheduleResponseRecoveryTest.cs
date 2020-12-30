// <copyright file="ProcessScheduleResponseRecoveryTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.Common.WebNotifications.Interfaces;
    using MS.GTA.Data.DataAccess;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Providers;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract;

    [TestClass]
    public class ProcessScheduleResponseRecoveryTest
    {
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<IDocDbDataAccess> mockDocDbDataAccess;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IOutlookProvider> mockOutlookProvider;
        private Mock<IScheduleQuery> mockScheduleQuery;
        private Mock<ITraceSource> mockTraceSource;
        private Mock<INotificationClient> mockNotificationClient;
        private Mock<IEmailTemplateDataAccess> mockEmailTemplateDataAccess;
        private Mock<IConfigurationManager> mockConfiguarationManager;
        private Mock<ILogger<NotificationManager>> loggerMock;
        private Mock<IEmailManager> mockEmailManager;
        private Mock<IWebNotificationBuilderClient> webNotificationsBuilderClientMock;
        private Mock<IWebNotificationInternalsProvider> webNotificationINternalsProviderMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.webNotificationsBuilderClientMock = new Mock<IWebNotificationBuilderClient>();
            this.webNotificationINternalsProviderMock = new Mock<IWebNotificationInternalsProvider>();
            this.mockDocDbDataAccess = new Mock<IDocDbDataAccess>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.mockOutlookProvider = new Mock<IOutlookProvider>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.mockNotificationClient = new Mock<INotificationClient>();
            this.mockEmailTemplateDataAccess = new Mock<IEmailTemplateDataAccess>();
            this.mockConfiguarationManager = new Mock<IConfigurationManager>();
            this.mockEmailManager = new Mock<IEmailManager>();
            this.loggerMock = new Mock<ILogger<NotificationManager>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// Test ProcessNotification With valid inputs
        /// </summary>
        [TestMethod]
        public void TestProcessNotificationRecoveryWithValidInputs()
        {
            Worker worker1 = new Worker { OfficeGraphIdentifier = "worker1Oid", Name = new PersonName { GivenName = "test", Surname = "1" }, FullName = "test 1", EmailPrimary = "test1@microsoft.com" };
            JobApplicationSchedule schedule1 = new JobApplicationSchedule { JobApplicationID = "123", ScheduleRequester = worker1, ScheduleID = "ScheduleId1" };
            IList<JobApplicationSchedule> schedules = new List<JobApplicationSchedule>();
            schedules.Add(schedule1);

            var notificationManager = new NotificationManager(this.mockDocDbDataAccess.Object, this.mockEmailClient.Object, this.mockOutlookProvider.Object, this.mockScheduleQuery.Object, this.mockNotificationClient.Object, this.mockConfiguarationManager.Object, this.mockEmailTemplateDataAccess.Object, this.mockEmailManager.Object, this.webNotificationINternalsProviderMock.Object, this.webNotificationsBuilderClientMock.Object, this.loggerMock.Object);

            this.mockScheduleQuery.Setup(a => a.GetPendingSchedules(It.IsAny<bool>())).ReturnsAsync(schedules);
            this.mockScheduleQuery.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).ReturnsAsync(It.IsAny<MeetingInfo>());

            var notification = notificationManager.ProcessScheduleResponse();

            Assert.AreEqual("RanToCompletion", notification.Status.ToString());
            this.mockScheduleQuery.Verify(a => a.GetPendingSchedules(It.IsAny<bool>()), Times.Once);
            this.mockScheduleQuery.Verify(a => a.GetScheduleByScheduleId(It.IsAny<string>()), Times.Once);
        }
    }
}
