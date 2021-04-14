//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.EnumSetModel;
    using HR.TA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class UpdateScheduleManagerTest
    {
        private Mock<IScheduleQuery> scheduleQueryMock;

        private FalconQuery falconQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<FalconQuery>> falconLoggerMock;

        private Mock<IServiceBusHelper> serviceBusHelperMock;

        private Mock<IGraphSubscriptionManager> graphSubscriptionManagerMock;

        private Mock<IEmailClient> emailClientMock;

        private Mock<IOutlookProvider> outlookProviderMock;

        private Mock<INotificationClient> notificationClientMock;

        private Mock<ILogger<ScheduleManager>> loggerMock;

        private Mock<IEmailHelper> emailHelperMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IInternalsProvider> internalsProviderMock;

        private Mock<IUserDetailsProvider> userDetailsProviderMock;

        private Mock<IHCMServiceContext> contextMock;

        private Mock<IConfigurationManager> configMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.contextMock = new Mock<IHCMServiceContext>();
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.serviceBusHelperMock = new Mock<IServiceBusHelper>();
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// UpdateScheduleServiceAccountTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleServiceAccountTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();
                   var exception = scheduleManager.UpdateScheduleServiceAccountAsync(string.Empty, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// UpdateScheduleServiceAccountTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleServiceAccountTestWithValidInputs()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.UpdateScheduleServiceAccountAsync("12345", Guid.NewGuid().ToString());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleStatusTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();
                   var exception = scheduleManager.UpdateScheduleStatus(string.Empty, ScheduleStatus.Sent).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// UpdateScheduleStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleStatusTestWithValidInputs()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.UpdateScheduleStatus("12345", ScheduleStatus.Sent);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
