//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.UnitTest.Business.EmailManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Common.Common.Email.Contracts;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Tracing;
    using BEM = BusinessLibrary.Business.V1;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendEmailNotificationTest
    {
        private Mock<IFalconQueryClient> falconQueryClient;
        private Mock<FalconQuery> falconQuery;
        private BEM.EmailManager manager;
        private Mock<ITraceSource> mockTraceSource;
        private Mock<INotificationClient> notificationClient;
        private Mock<IConfigurationManager> mockConfiguarationManager;
        private Mock<IScheduleQuery> mockScheduleQuery;
        private Mock<ILogger<BEM.EmailManager>> loggerMock;
        private Mock<IEmailHelper> emailHelperMock;
        private Mock<ILogger<FalconQuery>> loggerMockFalconQuery;
        private Mock<IInternalsProvider> internalsProviderMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.loggerMockFalconQuery = new Mock<ILogger<FalconQuery>>();
            this.falconQueryClient = new Mock<IFalconQueryClient>();
            this.falconQuery = new Mock<FalconQuery>(this.falconQueryClient.Object, this.loggerMockFalconQuery.Object);
            this.notificationClient = new Mock<INotificationClient>();
            this.mockConfiguarationManager = new Mock<IConfigurationManager>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.loggerMock = new Mock<ILogger<BEM.EmailManager>>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.manager = new BEM.EmailManager(this.notificationClient.Object, this.falconQuery.Object, this.mockConfiguarationManager.Object, this.mockTraceSource.Object, this.mockScheduleQuery.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// SendEmailNotificationV2Test with null emailDetails.
        /// </summary>
        [TestMethod]
        public async Task SendEmailNotificationWithNullEmailDetails()
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidRequestDataValidationException>(async () => await this.manager.SendEmailNotification("JA123", null, null));
            Assert.IsTrue(exception.Message.Contains("Data validation failed: SendEmailNotification method in EmailManager: Send email notification failed as the details are null...", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// SendEmailNotificationV2Test with no EmailAttachments.
        /// </summary>
        [TestMethod]
        public async Task SendEmailNotificationWithoutEmailAttachments()
        {
            ScheduleInvitationDetails emailDetails = new ScheduleInvitationDetails
            {
                IsInterviewScheduleShared = true,
                IsInterviewTitleShared = true,
                Subject = "Subject",
                PrimaryEmailRecipients = new List<string>() { "abc@xyz.com" },
                CcEmailAddressList = new List<string>() { "def@xyz.com" },
                EmailContent = "Email content",
            };

            Timezone timeZoneInfo = new Timezone { TimezoneAbbr = "IST", UTCOffset = 120 };
            this.mockScheduleQuery.Setup(ja => ja.GetSchedulesByJobApplicationId(It.IsAny<string>())).ReturnsAsync(new List<MeetingInfo>());
            this.mockScheduleQuery.Setup(ja => ja.GetTimezoneForJobApplication(It.IsAny<string>())).ReturnsAsync(timeZoneInfo);
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);

            var mailSent = await this.manager.SendEmailNotification(It.IsAny<string>(), emailDetails, It.IsAny<string>(), It.IsAny<string>(), false);
            Assert.IsTrue(mailSent);
        }

        /// <summary>
        /// SendEmailNotificationV2Test with Email Attachments.
        /// </summary>
        [TestMethod]
        public async Task SendEmailNotificationWithEmailAttachments()
        {
            ScheduleInvitationDetails emailDetails = new ScheduleInvitationDetails
            {
                IsInterviewScheduleShared = true,
                IsInterviewTitleShared = true,
                Subject = "Subject",
                PrimaryEmailRecipients = new List<string>() { "abc@xyz.com" },
                CcEmailAddressList = new List<string>() { "def@xyz.com" },
                EmailContent = "Email content",
                EmailAttachments = new FileAttachmentRequest(),
            };

            emailDetails.EmailAttachments.FileLabels = new List<string>() { "attachment1" };
            var mockFileCollection = new Mock<IFormFileCollection>();
            emailDetails.EmailAttachments.Files = mockFileCollection.Object;
            mockFileCollection.Setup(ob => ob.Count).Returns(1);
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(ob => ob.Length).Returns(4);
            mockFileCollection.Setup(ob => ob[0]).Returns(mockFile.Object);
            var lstFiles = new List<IFormFile>() { mockFile.Object };
            mockFileCollection.Setup(ob => ob.GetEnumerator()).Returns(lstFiles.GetEnumerator());
            mockFile.Setup(ob => ob.ContentType).Returns("image/png");
            mockFile.Setup(ob => ob.FileName).Returns("SampleValidName.docx");

            Timezone timeZoneInfo = new Timezone { TimezoneAbbr = "IST", UTCOffset = 120 };
            this.mockScheduleQuery.Setup(ja => ja.GetSchedulesByJobApplicationId(It.IsAny<string>())).ReturnsAsync(new List<MeetingInfo>());
            this.mockScheduleQuery.Setup(ja => ja.GetTimezoneForJobApplication(It.IsAny<string>())).ReturnsAsync(timeZoneInfo);
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);

            var mailSent = await this.manager.SendEmailNotification(It.IsAny<string>(), emailDetails, It.IsAny<string>(), It.IsAny<string>(), false);
            Assert.IsTrue(mailSent);
        }
    }
}
