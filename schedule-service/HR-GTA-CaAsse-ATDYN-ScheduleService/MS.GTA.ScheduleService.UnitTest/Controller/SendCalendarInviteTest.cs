// <copyright file="SendCalendarInviteTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class SendCalendarInviteTest
    {
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<IHCMServiceContext> mockHCMServiceContext;
        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<IServiceBusHelper> mockServiceBusHelper;
        private Mock<INotificationManager> mockNotificationManager;
        private Mock<IScheduleManager> mockScheduleManager;
        private Mock<ILogger<MeetingController>> loggerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockHCMServiceContext = new Mock<IHCMServiceContext>();
            this.mockServiceBusHelper = new Mock<IServiceBusHelper>();
            this.mockNotificationManager = new Mock<INotificationManager>();
            this.mockScheduleManager = new Mock<IScheduleManager>();
            this.mockConfigurationManager = new Mock<IConfigurationManager>();
            this.loggerMock = new Mock<ILogger<MeetingController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// Test SendCalendarInvite With InvalidInputs
        /// </summary>
        [TestMethod]
        public void SendCalendarInviteWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<MeetingController>();

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

                   var notificationContentNullException = meetingController.SendCalendarInvite(null, null);

                   Assert.IsInstanceOfType(notificationContentNullException.Exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// Test SendCalendarInvite With ValidInputs
        /// </summary>
        [TestMethod]
        public void SendCalendarInviteWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<MeetingController>();

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

                   this.mockScheduleManager.Setup(a => a.SendCalendarEvent(It.IsAny<List<string>>(), It.IsAny<string>())).ReturnsAsync(new List<string>());

                   var meetingController = new MeetingController(httpContextAccessorMock, this.mockScheduleManager.Object, this.mockNotificationManager.Object, this.loggerMock.Object);

                   var response = meetingController.SendCalendarInvite("test", new List<string>() { "test" });

                   Assert.AreEqual(response.IsCompletedSuccessfully, true);
               });
        }
    }
}
