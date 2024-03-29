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
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Base.Security;
    using HR.TA.Common.Common.Common.Email.Contracts;
    using HR.TA.Common.Email.Contracts;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.ScheduleService.BusinessLibrary.Configurations;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.Data.Models;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Tracing;
    using BEM = BusinessLibrary.Business.V1;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendFeedbackReminderToAllAsyncTest
    {
        private Mock<IFalconQueryClient> falconQueryClient;
        private Mock<FalconQuery> falconQuery;
        private BEM.EmailManager manager;
        private Mock<ITraceSource> mockTraceSource;
        private Mock<INotificationClient> notificationClient;
        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<IScheduleQuery> mockScheduleQuery;
        private Mock<ILogger<BEM.EmailManager>> loggerMock;
        private Mock<ILogger<FalconQuery>> loggerMockFalconQuery;
        private Mock<IEmailHelper> emailHelperMock;
        private Mock<IHCMApplicationPrincipal> principalMock;
        private Mock<IInternalsProvider> internalsProviderMock;
        private PendingFeedback pendingFeedback;
        private EmailTemplateConfiguaration emailTemplateConfiguaration;
        private AutomatedFeedbackReminderConfiguration automatedFeedbackReminderConfiguration;
        private IVClientConfiguration iVClientConfiguration;
        private Common.Email.Contracts.EmailTemplate emailTemplate;

        [TestInitialize]
        public void BeforeEach()
        {
            this.loggerMockFalconQuery = new Mock<ILogger<FalconQuery>>();
            this.principalMock = new Mock<IHCMApplicationPrincipal>();
            this.falconQueryClient = new Mock<IFalconQueryClient>();
            this.falconQuery = new Mock<FalconQuery>(this.falconQueryClient.Object, this.loggerMockFalconQuery.Object);
            this.notificationClient = new Mock<INotificationClient>();
            this.mockConfigurationManager = new Mock<IConfigurationManager>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.loggerMock = new Mock<ILogger<BEM.EmailManager>>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.pendingFeedback = new PendingFeedback()
            {
                JobApplicationId = Guid.NewGuid().ToString(),
                Interviewer = new Worker
                {
                    WorkerId = Guid.NewGuid().ToString(),
                    OfficeGraphIdentifier = Guid.NewGuid().ToString(),
                    EmailPrimary = "interviewer@random.xyz",
                    Name = new PersonName
                    {
                        GivenName = "Awesome",
                        Surname = "Interviewer",
                    },
                },
                HiringManager = new Worker
                {
                    WorkerId = Guid.NewGuid().ToString(),
                    OfficeGraphIdentifier = Guid.NewGuid().ToString(),
                    EmailPrimary = "hm@random.xyz",
                    Name = new PersonName
                    {
                        GivenName = "Awesome",
                        Surname = "Hiring Manager",
                    },
                    FullName = "Awesome HM"
                },
                Recruiter = new Worker
                {
                    WorkerId = Guid.NewGuid().ToString(),
                    OfficeGraphIdentifier = Guid.NewGuid().ToString(),
                    EmailPrimary = "recruiter@random.xyz",
                    Name = new PersonName
                    {
                        GivenName = "Awesome",
                        Surname = "Recruiter",
                    },
                    FullName = "Awesome HM"
                },
                ExternalJobOpeningId = Guid.NewGuid().ToString(),
                PositionTitle = "Software Engineer",
                CandidateName = "Brilliant Candidate",
                ScheduleRequester = new Worker
                {
                    FullName = "Schedule Requester",
                    EmailPrimary = "schedulereq@random.xyz",
                }
            };
            this.emailTemplateConfiguaration = new EmailTemplateConfiguaration
            {
                FeedbackReminderEmailTemplate = "feedback reminder email template"
            };
            this.automatedFeedbackReminderConfiguration = new AutomatedFeedbackReminderConfiguration
            {
                FeedbackReminderOffsetDurationHours = 10,
                FeedbackReminderWindowMinutes = 30
            };
            this.iVClientConfiguration = new IVClientConfiguration
            {
                RecruitingHubClientUrl = "randomemail@random.xyz"
            };
            this.emailTemplate = new Common.Email.Contracts.EmailTemplate
            {
                Subject = "xyz",
                Body = "body {Call_To_Action_Link}"
            };
            this.mockConfigurationManager.Setup(mcm => mcm.Get<EmailTemplateConfiguaration>()).Returns(this.emailTemplateConfiguaration);
            this.mockConfigurationManager.Setup(mcm => mcm.Get<AutomatedFeedbackReminderConfiguration>()).Returns(this.automatedFeedbackReminderConfiguration);
            this.mockConfigurationManager.Setup(mcm => mcm.Get<IVClientConfiguration>()).Returns(this.iVClientConfiguration);
            this.mockScheduleQuery.Setup(msq => msq.GetTemplate(It.IsAny<string>())).ReturnsAsync(this.emailTemplate);
            this.manager = this.GetEmailManagerInstance();
        }

        [TestMethod]
        public async Task SendAllFeedbackRemindersAsync_ValidBusinessCase()
        {
            this.mockScheduleQuery.Setup(msq => msq.GetAllPendingFeedbacksForReminderAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<PendingFeedback> { this.pendingFeedback });
            List<NotificationItem> notificationItems = new List<NotificationItem>();
            NotificationItem a = new NotificationItem();
            a.TrackingId = "test";
            notificationItems.Add(a);
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);

            var mailSent = await this.manager.SendFeedbackReminderToAllAsync();
            Assert.IsTrue(mailSent);
        }

        [TestMethod]
        public async Task SendAllFeedbackRemindersAsync_InvalidInterviewer()
        {
            this.pendingFeedback.Interviewer = null;
            this.mockScheduleQuery.Setup(msq => msq.GetAllPendingFeedbacksForReminderAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<PendingFeedback> { this.pendingFeedback });

            var mailSent = await this.manager.SendFeedbackReminderToAllAsync();
            Assert.IsFalse(mailSent);
        }

        private BEM.EmailManager GetEmailManagerInstance()
        {
            return new BEM.EmailManager(this.notificationClient.Object, this.falconQuery.Object, this.mockConfigurationManager.Object, this.mockTraceSource.Object, this.mockScheduleQuery.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.loggerMock.Object);
        }
    }
}
