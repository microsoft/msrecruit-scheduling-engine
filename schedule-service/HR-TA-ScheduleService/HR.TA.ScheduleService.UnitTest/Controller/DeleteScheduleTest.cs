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
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Controllers.V1;
    using HR.TA.ScheduleService.UnitTest.Mocks;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;

    [TestClass]
    public class DeleteScheduleTest
    {
        private IHttpContextAccessor httpContextAccessorMock;

        private Mock<IHCMServiceContext> hCMServiceContextMock;

        private Mock<IScheduleManager> scheduleManagerMock;

        private Mock<ILogger<ScheduleServiceController>> loggerMock;

        private Mock<IRoleManager> roleManagerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

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
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleTestWithNullInputs()
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

                   var exception = controller.DeleteSchedule(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleTestWithValidInputs()
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

                   this.scheduleManagerMock.Setup(a => a.DeleteSchedule(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));

                   var controller = this.GetControllerInstance();

                   var result = controller.DeleteSchedule("12345");
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        private ScheduleServiceController GetControllerInstance()
        {
            return new ScheduleServiceController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.scheduleManagerMock.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
