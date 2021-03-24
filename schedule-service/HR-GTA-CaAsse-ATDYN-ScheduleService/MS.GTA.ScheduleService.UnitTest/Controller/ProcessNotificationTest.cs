//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using CommonLibrary.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class ProcessNotificationTest
    {
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<IServiceBusHelper> mockServiceBusHelper;
        private Mock<INotificationManager> mockNotificationManager;
        private Mock<IScheduleManager> mockScheduleManager;
        private Mock<ILogger<MeetingController>> loggerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockServiceBusHelper = new Mock<IServiceBusHelper>();
            this.mockNotificationManager = new Mock<INotificationManager>();
            this.mockScheduleManager = new Mock<IScheduleManager>();
            this.mockConfigurationManager = new Mock<IConfigurationManager>();
            this.mockScheduleManager = new Mock<IScheduleManager>();
            this.loggerMock = new Mock<ILogger<MeetingController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// Test ProcessNotification With InvalidInputs
        /// </summary>
        [TestMethod]
        public void TestProcessNotificationWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<NotificationController>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

                   var meetingController = new MeetingController(httpContextAccessorMock, this.mockScheduleManager.Object, this.mockNotificationManager.Object, this.loggerMock.Object);

                   var notificationContentNullException = meetingController.ProcessNotification(null);

                   Assert.IsInstanceOfType(notificationContentNullException.Exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// Test ProcessNotification With ValidInputs
        /// </summary>
        [TestMethod]
        public void TestProcessNotificationWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<NotificationController>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

                   this.mockNotificationManager.Setup(a => a.ProcessNotificationContent(It.IsAny<NotificationContent>())).ReturnsAsync(true);
                   var meetingController = new MeetingController(httpContextAccessorMock, this.mockScheduleManager.Object, this.mockNotificationManager.Object, this.loggerMock.Object);

                   var response = meetingController.ProcessNotification(this.GetMockNotificationContent());

                   Assert.AreEqual(response.IsCompletedSuccessfully, true);
                   this.mockNotificationManager.Verify(a => a.ProcessNotificationContent(It.IsAny<NotificationContent>()), Times.Once);
               });
        }

        private NotificationContent GetMockNotificationContent()
        {
            return new NotificationContent()
            {
                Value = new List<Notification>()
            };
        }
    }
}
