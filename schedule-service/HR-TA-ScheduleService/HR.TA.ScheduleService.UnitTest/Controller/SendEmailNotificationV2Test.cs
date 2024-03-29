//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.ScheduleService.BusinessLibrary.Exceptions;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Controllers.V1;
    using HR.TA.ScheduleService.UnitTest.Mocks;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.TalentContracts.ScheduleService;

    [TestClass]
    public class SendEmailNotificationV2Test
    {
        private IHttpContextAccessor httpContextAccessorMock;

        private Mock<IHCMServiceContext> hCMServiceContextMock;

        private EmailController emailController;

        private Mock<IEmailManager> emailManagerMock;

        private Mock<ILogger<EmailController>> loggerMock;

        private Mock<IRoleManager> roleManagerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforEach()
        {
            this.httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

            this.hCMServiceContextMock = new Mock<IHCMServiceContext>();
            this.emailManagerMock = new Mock<IEmailManager>();
            this.loggerMock = new Mock<ILogger<EmailController>>();
            this.roleManagerMock = new Mock<IRoleManager>();

            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SendEmailNotificationV2Test with null jobApplicationId.
        /// </summary>
        [TestMethod]
        public void SendEmailNotificationV2TestWithInvalidInputs()
        {
            this.emailController = this.GetControllerInstance();

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
                   this.emailController = this.GetControllerInstance();

                   var scheduleInvitationDetailsV2 = new ScheduleInvitationDetailsV2
                   {
                       IsInterviewTitleShared = true,
                       CcEmailAddressList = new List<string>() { "abc@xyz.com" },
                       EmailContent = "Email Content",
                       PrimaryEmailRecipients = new List<string>() { "def@xyz.com" },
                       Subject = "Subject",
                   };

                   var exception = this.emailController.SendEmailNotificationV2(null, scheduleInvitationDetailsV2).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
                   Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid application id", StringComparison.Ordinal));
               });
        }

        /// <summary>
        /// SendEmailNotificationV2Test with no attachments.
        /// </summary>
        [TestMethod]
        public void SendEmailNotificationV2TestWithNullAttachments()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            logger.ExecuteRootAsync(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   this.emailController = this.GetControllerInstance();

                   var scheduleInvitationDetailsV2 = new ScheduleInvitationDetailsV2
                   {
                       IsInterviewTitleShared = true,
                       CcEmailAddressList = new List<string>() { "abc@xyz.com" },
                       EmailContent = "Email Content",
                       PrimaryEmailRecipients = new List<string>() { "def@xyz.com" },
                       Subject = "Subject",
                   };

                   var scheduleInvitationDetails = new ScheduleInvitationDetails();
                   this.emailManagerMock
                   .Setup(a => a.SendEmailNotification(It.IsAny<string>(), scheduleInvitationDetails, It.IsAny<string>(), It.IsAny<string>(), false))
                   .Returns(Task.FromResult(true));

                   this.roleManagerMock.Setup(a => a.IsUserInJobApplicationParticipants(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

                   var result = await this.emailController.SendEmailNotificationV2(It.IsAny<string>(), scheduleInvitationDetailsV2);

                   Assert.IsNotNull(result);
                   Assert.AreEqual(result, HttpStatusCode.OK);
               });
        }

        /// <summary>
        /// SendEmailNotificationV2Test with attachments
        /// </summary>
        [TestMethod]
        public void SendEmailNotificationV2TestWithAttachments()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<EmailController>();
            logger.ExecuteRootAsync(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               async () =>
               {
                   this.emailController = this.GetControllerInstance();

                   var scheduleInvitationDetailsV2 = new ScheduleInvitationDetailsV2
                   {
                       IsInterviewTitleShared = true,
                       CcEmailAddressList = new List<string>() { "abc@xyz.com" },
                       EmailAttachmentFileLabels = new List<string>() { "attachment1" },
                       EmailContent = "Email Content",
                       PrimaryEmailRecipients = new List<string>() { "def@xyz.com" },
                       Subject = "Subject",
                   };

                   var mockFileCollection = new Mock<IFormFileCollection>();
                   scheduleInvitationDetailsV2.EmailAttachmentFiles = mockFileCollection.Object;
                   mockFileCollection.Setup(ob => ob.Count).Returns(1);
                   var mockFile = new Mock<IFormFile>();
                   mockFile.Setup(ob => ob.Length).Returns(4);
                   mockFileCollection.Setup(ob => ob[0]).Returns(mockFile.Object);
                   var lstFiles = new List<IFormFile>() { mockFile.Object };
                   mockFileCollection.Setup(ob => ob.GetEnumerator()).Returns(lstFiles.GetEnumerator());
                   mockFile.Setup(ob => ob.ContentType).Returns("image/png");
                   mockFile.Setup(ob => ob.FileName).Returns("SampleValidName.docx");

                   var scheduleInvitationDetails = new ScheduleInvitationDetails();
                   this.emailManagerMock
                   .Setup(a => a.SendEmailNotification(It.IsAny<string>(), scheduleInvitationDetails, It.IsAny<string>(), It.IsAny<string>(), false))
                   .Returns(Task.FromResult(true));

                   this.roleManagerMock.Setup(a => a.IsUserInJobApplicationParticipants(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

                   var result = await this.emailController.SendEmailNotificationV2(It.IsAny<string>(), scheduleInvitationDetailsV2);

                   Assert.IsNotNull(result);
                   Assert.AreEqual(result, HttpStatusCode.OK);
               });
        }

        private EmailController GetControllerInstance()
        {
            return new EmailController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.emailManagerMock.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
