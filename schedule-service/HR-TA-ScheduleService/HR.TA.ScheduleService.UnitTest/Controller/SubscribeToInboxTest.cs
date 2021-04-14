//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Controllers.V1;
    using HR.TA.ScheduleService.UnitTest.Mocks;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;

    [TestClass]
    public class SubscribeToInboxTest
    {
        private Mock<IGraphSubscriptionManager> scheduleManagerMock;
        private Mock<ILogger<SubscriptionController>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforEach()
        {
            this.scheduleManagerMock = new Mock<IGraphSubscriptionManager>();
            this.loggerMock = new Mock<ILogger<SubscriptionController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SubscribeToInbox
        /// </summary>
        [TestMethod]
        public void SubscribeToInboxTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<SubscriptionController>();

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
                   var controller = new SubscriptionController(httpContextAccessorMock,this.scheduleManagerMock.Object, this.loggerMock.Object);

                   var exception = controller.SubscribeToInboxController(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// SubscribeToInbox
        /// </summary>
        [TestMethod]
        public void SubscribeToInboxTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<SubscriptionController>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

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

                   this.scheduleManagerMock.Setup(a => a.SubscribeToInbox(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(false));

                   var controller = new SubscriptionController(httpContextAccessorMock,this.scheduleManagerMock.Object, this.loggerMock.Object);

                   var result = controller.SubscribeToInboxController("12345");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
