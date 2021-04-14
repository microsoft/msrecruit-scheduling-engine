//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Base.Security;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Controllers.V1;
    using HR.TA.ScheduleService.UnitTest.Mocks;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.TalentContracts.ScheduleService;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendEmailToInterviewersTests
    {
        private Mock<IEmailManager> emailManager;
        private Mock<IHCMServiceContext> hCMServiceContextMock;
        private IHttpContextAccessor httpContextAccessorMock;
        private Mock<ILogger<EmailController>> loggerMock;
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<IRoleManager> roleManagerMock;
        private EmailController emailController;

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

        [TestMethod]
        public async Task SendEmailToInterviewersWithValidBusinessCase()
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
                   EmailNotificationRequest notificationRequest = new EmailNotificationRequest
                   {
                       JobApplicationId = "Test Job Application Id",
                       EmailBody = "This is a email body.",
                       EmailFooter = "This is a email footer",
                       Subject = "I'm a subject.",
                       MailTo = new List<GraphPerson>
                       {
                           new GraphPerson
                           {
                               Id = "Test User Id",
                               Email = "abc@xyz.com"
                           }
                       },
                       MailCC = new List<GraphPerson>
                       {
                           new GraphPerson
                           {
                               Id = "Test User Id",
                               Email = "abc@xyz.com"
                           }
                       }
                   };

                   this.emailManager
                   .Setup(a => a.SendFeedbackEmailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>(), It.IsAny<bool>()))
                   .Returns(Task.FromResult(true));
                   this.roleManagerMock.Setup(a => a.IsUserHMorRecOrContributor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
                   var emailSent = await this.emailController.SendEmailToInterviewers(notificationRequest);
                   this.emailManager.Verify(em => em.SendFeedbackEmailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
               });
        }

        private EmailController GetControllerInstance()
        {
            return new EmailController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.emailManager.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
