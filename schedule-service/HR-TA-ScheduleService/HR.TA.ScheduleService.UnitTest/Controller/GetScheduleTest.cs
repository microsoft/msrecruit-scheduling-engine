//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Collections.Generic;
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
    public class GetScheduleTest
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
        /// GetScheduleTest
        /// </summary>
        [TestMethod]
        public void GetScheduleTestWithNullInputs()
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

                   var exception = controller.GetScheduleByScheduleId(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetScheduleTest
        /// </summary>
        [TestMethod]
        public void GetScheduleTestWithValidInputs()
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

                   var result = controller.GetScheduleByScheduleId("12345");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleTest
        /// </summary>
        [TestMethod]
        public void GetSchedulesByFreeBusyRequestTestWithNullInputs()
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

                   var exception = controller.GetSchedulesByFreeBusyRequest(null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// GetScheduleTest
        /// </summary>
        [TestMethod]
        public void GetSchedulesByFreeBusyRequestTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            List<MeetingInfo> result = new List<MeetingInfo>()
            {
                meetingInfo
            };

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

                   this.scheduleManagerMock.Setup(a => a.GetSchedulesByFreeBusyRequest(It.IsAny<FreeBusyRequest>(), It.IsAny<string>())).Returns(Task.FromResult(result));

                   var controller = this.GetControllerInstance();

                   var apiresult = controller.GetSchedulesByFreeBusyRequest(new FreeBusyRequest(), string.Empty);
                   Assert.IsNotNull(apiresult);
                   Assert.IsTrue(apiresult.IsCompleted);
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithNullInputs()
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

                   var exception = controller.GetFreeBusySchedule(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithValidInputs()
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

                   this.scheduleManagerMock.Setup(a => a.GetFreeBusySchedule(It.IsAny<FreeBusyRequest>())).Returns(Task.FromResult(new List<MeetingInfo>()));

                   var controller = this.GetControllerInstance();

                   var result = controller.GetFreeBusySchedule(new FreeBusyRequest());
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationIdTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithNullInputs()
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

                   var exception = controller.GetSchedulesByJobApplicationId(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationIdTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>()
            {
                meetingInfo
            };

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

                   this.scheduleManagerMock.Setup(a => a.GetSchedulesByJobApplicationId(It.IsAny<string>())).Returns(Task.FromResult(meetingInfos));

                   var controller = this.GetControllerInstance();

                   var result = controller.GetSchedulesByJobApplicationId("12345");
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
