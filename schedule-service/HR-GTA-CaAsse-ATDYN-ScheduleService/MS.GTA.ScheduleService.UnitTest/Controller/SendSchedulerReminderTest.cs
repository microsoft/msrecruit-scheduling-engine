//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.TalentContracts.InterviewService;

    /// <summary>
    /// Tests for SendSchedulerReminder method
    /// </summary>
    [TestClass]
    public class SendSchedulerReminderTest
    {
        private Mock<IEmailManager> emailManager;
        private Mock<IHCMServiceContext> hCMServiceContextMock;
        private IHttpContextAccessor httpContextAccessorMock;
        private Mock<ILogger<EmailController>> loggerMock;
        private ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<IRoleManager> roleManagerMock;

        /// <summary>
        /// test Initialize Method
        /// </summary>
        [TestInitialize]
        public void BeforeEach()
        {
            this.httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();
            this.hCMServiceContextMock = new Mock<IHCMServiceContext>();
            this.emailManager = new Mock<IEmailManager>();
            this.loggerMock = new Mock<ILogger<EmailController>>();
            this.roleManagerMock = new Mock<IRoleManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SendSchedulerReminder Test with valid input
        /// </summary>
        [TestMethod]
        public void SendSchedulerReminderTestValidInput()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
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
                   var emailController = this.GetControllerInstance();

                   this.emailManager
                   .Setup(a => a.SendSchedulerReminder())
                   .Returns(Task.FromResult(true));

                   var result = emailController.SendSchedulerReminder();
                   Assert.AreEqual(result.Status.ToString(), "RanToCompletion");
                   Assert.AreEqual(result.Result, true);
                   this.emailManager.Verify(a => a.SendSchedulerReminder(), Times.Once);
               });
        }

        private EmailController GetControllerInstance()
        {
            return new EmailController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.emailManager.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
