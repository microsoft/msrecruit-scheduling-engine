//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Controllers.V1;
    using HR.TA.ScheduleService.UnitTest.Mocks;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Exceptions;

    [TestClass]
    public class SendScheduleTest
    {
        private IHttpContextAccessor httpContextAccessorMock;

        private Mock<IHCMServiceContext> hCMServiceContextMock;

        private Mock<IScheduleManager> scheduleManagerMock;

        private Mock<ILogger<ScheduleServiceController>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IRoleManager> roleManagerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

            this.hCMServiceContextMock = new Mock<IHCMServiceContext>();
            this.scheduleManagerMock = new Mock<IScheduleManager>();
            this.loggerMock = new Mock<ILogger<ScheduleServiceController>>();
            this.roleManagerMock = new Mock<IRoleManager>();

            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SendScheduleTest
        /// </summary>
        [TestMethod]
        public void SendScheduleTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

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
                   var controller = this.GetControllerInstance();

                   var exception = controller.SendSchedule(string.Empty, null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// SendScheduleTest
        /// </summary>
        [TestMethod]
        public void SendScheduleTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();
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

                   this.scheduleManagerMock.Setup(a => a.CreateSchedule(It.IsAny<MeetingInfo>(), It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));

                   var controller = this.GetControllerInstance();

                   var result = controller.SendSchedule("12345", null);
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        private ScheduleServiceController GetControllerInstance()
        {
            return new ScheduleServiceController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.scheduleManagerMock.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
