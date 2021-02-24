//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.Security;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendShareFeedbackEmailToAATest
    {
        private Mock<IEmailManager> emailManager; 
        private Mock<IRoleManager> roleManagerMock;
        private Mock<IHCMServiceContext> hCMServiceContextMock;
        private IHttpContextAccessor httpContextAccessorMock;
        private Mock<ILogger<EmailController>> loggerMock;
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private EmailController emailController;
        private Mock<IHCMApplicationPrincipal> principalMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();
            this.hCMServiceContextMock = new Mock<IHCMServiceContext>();
            this.emailManager = new Mock<IEmailManager>();
            this.roleManagerMock = new Mock<IRoleManager>();
            this.loggerMock = new Mock<ILogger<EmailController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.principalMock = new Mock<IHCMApplicationPrincipal>();
        }

        [TestMethod]
        public async Task SendShareFeedbackEmailToAAWithNullRequest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            await logger.ExecuteRootAsync(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   this.emailController = this.GetControllerInstance();
                   this.emailManager
                   .Setup(a => a.SendShareFeedbackEmailToAA(It.IsAny<string>(), It.IsAny<string[]>()))
                   .Returns(Task.FromResult(true));

                   var exception = await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => await this.emailController.SendShareFeedbackEmailToAA(null, null));
                   Assert.IsTrue(exception.Message.Contains("SendShareFeedbackEmailToAA: input paramenter cannot be null or empty", StringComparison.Ordinal));
               });
        }

        [TestMethod]
        public async Task SendShareFeedbackEmailToAAWithNullJobApplicationId()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            await logger.ExecuteRootAsync(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   this.emailController = this.GetControllerInstance();

                   this.emailManager
                   .Setup(a => a.SendShareFeedbackEmailToAA(It.IsAny<string>(), It.IsAny<string[]>()))
                   .Returns(Task.FromResult(true));

                   var exception = await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => await this.emailController.SendShareFeedbackEmailToAA(null, null));
                   Assert.IsTrue(exception.Message.Contains("SendShareFeedbackEmailToAA: input paramenter cannot be null or empty", StringComparison.Ordinal));
               });
        }

        [TestMethod]
        public async Task SendShareFeedbackEmailToAAWithNullPostParameter()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            await logger.ExecuteRootAsync(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   this.emailController = this.GetControllerInstance();
                   string jobApplicationid = Guid.NewGuid().ToString();

                   this.emailManager
                   .Setup(a => a.SendShareFeedbackEmailToAA(It.IsAny<string>(), It.IsAny<string[]>()))
                   .Returns(Task.FromResult(true));

                   var exception = await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => await this.emailController.SendShareFeedbackEmailToAA(jobApplicationid, null));
                   Assert.IsTrue(exception.Message.Contains("SendShareFeedbackEmailToAA: input paramenter cannot be null or empty", StringComparison.Ordinal));
               });
        }

        [TestMethod]
        public async Task SendShareFeedbackEmailToAAWithValidBusinessCase()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            await logger.ExecuteRootAsync(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   string jobApplicationId = Guid.NewGuid().ToString();
                   string[] oids = new string[1];
                   oids.SetValue(Guid.NewGuid().ToString(), 0);

                   this.emailController = this.GetControllerInstance();
                   this.emailManager
                   .Setup(a => a.SendShareFeedbackEmailToAA(It.IsAny<string>(), It.IsAny<string[]>()))
                   .Returns(Task.FromResult(true));
                   this.roleManagerMock.Setup(a => a.IsUserContributor(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

                   var emailSent = await this.emailController.SendShareFeedbackEmailToAA(jobApplicationId, oids);
                   this.emailManager.Verify(em => em.SendShareFeedbackEmailToAA(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
               });
        }

        private EmailController GetControllerInstance()
        {
            return new EmailController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.emailManager.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
