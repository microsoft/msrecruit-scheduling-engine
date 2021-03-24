//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using CommonLibrary.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class NotificationListenTest
    {
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<IHCMServiceContext> mockHCMServiceContext;
        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<IServiceBusHelper> mockServiceBusHelper;
        private Mock<INotificationManager> mockNotificationManager;
        private Mock<IScheduleManager> mockScheduleManager;
        private Mock<ILogger<NotificationController>> loggerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockHCMServiceContext = new Mock<IHCMServiceContext>();
            this.mockServiceBusHelper = new Mock<IServiceBusHelper>();
            this.mockNotificationManager = new Mock<INotificationManager>();
            this.mockScheduleManager = new Mock<IScheduleManager>();
            this.mockConfigurationManager = new Mock<IConfigurationManager>();
            this.loggerMock = new Mock<ILogger<NotificationController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// Test NotificationListen With InvalidInputs
        /// </summary>
        [TestMethod]
        public void NotificationListenTestWithInvalidInputs()
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

                   var notifyController = new NotificationController(this.mockConfigurationManager.Object, this.mockServiceBusHelper.Object, this.mockHCMServiceContext.Object, this.loggerMock.Object);

                   var result = notifyController.Listen(null);
                   Assert.AreEqual(result.IsCompletedSuccessfully, true);
               });
        }

        /// <summary>
        /// Test NotificationListen With ValidInputs
        /// </summary>
        [TestMethod]
        public void NotificationListenTestWithValidInputs()
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

                   var notifyController = new NotificationController(this.mockConfigurationManager.Object, this.mockServiceBusHelper.Object, this.mockHCMServiceContext.Object, this.loggerMock.Object);

                   var result = notifyController.Listen("Test");
                   Microsoft.AspNetCore.Mvc.ContentResult contentResult = new Microsoft.AspNetCore.Mvc.ContentResult();
                   contentResult.Content = "Test";
                   contentResult.ContentType = "plain/text";
                   Assert.AreSame(contentResult.Content, ((Microsoft.AspNetCore.Mvc.ContentResult)result.Result).Content);
               });
        }
    }
}
