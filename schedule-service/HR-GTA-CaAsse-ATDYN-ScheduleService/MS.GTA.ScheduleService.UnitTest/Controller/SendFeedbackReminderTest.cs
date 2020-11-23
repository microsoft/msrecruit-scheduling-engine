// <copyright file="SendFeedbackReminderTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

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
    /// Tests for SendFeedbackReminder method
    /// </summary>
    [TestClass]
    public class SendFeedbackReminderTest
    {
        private Mock<IEmailManager> emailManager;
        private Mock<IHCMServiceContext> hCMServiceContextMock;
        private IHttpContextAccessor httpContextAccessorMock;
        private Mock<ILogger<EmailController>> loggerMock;
        private Mock<IRoleManager> roleManagerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

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
        /// SendFeedbackReminder Test with valid input
        /// </summary>
        [TestMethod]
        public void SendFeedbackReminderTestValidInput()
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

                   PendingFeedback pendingFeedback = new PendingFeedback { JobApplicationId = "123", InterviewerOID = "123" };

                   this.emailManager
                   .Setup(a => a.SendFeedbackReminder(It.IsAny<PendingFeedback>()))
                   .Returns(Task.FromResult(true));
                   this.roleManagerMock.Setup(a => a.IsUserInJobApplicationParticipants(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

                   var result = emailController.SendFeedbackReminder(pendingFeedback);
                   Assert.AreEqual(result.Status.ToString(), "RanToCompletion");
                   Assert.AreEqual(result.Result, true);
                   this.emailManager.Verify(a => a.SendFeedbackReminder(It.IsAny<PendingFeedback>()), Times.Once);
               });
        }

        /// <summary>
        /// SendFeedbackReminder Test with invalid input
        /// </summary>
        [TestMethod]
        public void SendFeedbackReminderTestInvalidInput()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();
                   var emailController = this.GetControllerInstance();

                   await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => { await emailController.SendFeedbackReminder(null); });
                   await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => { await emailController.SendFeedbackReminder(new PendingFeedback { InterviewerOID = "123" }); });
                   await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => { await emailController.SendFeedbackReminder(new PendingFeedback { JobApplicationId = "123" }); });
               });
        }

        private EmailController GetControllerInstance()
        {
            return new EmailController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.emailManager.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
