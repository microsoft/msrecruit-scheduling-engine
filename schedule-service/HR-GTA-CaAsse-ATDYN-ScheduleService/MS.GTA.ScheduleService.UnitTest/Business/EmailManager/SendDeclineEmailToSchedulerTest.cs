// <copyright file="SendDeclineEmailToSchedulerTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Business.EmailManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Tracing;
    using BEM = BusinessLibrary.Business.V1;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendDeclineEmailToSchedulerTest
    {
        private const string RequesterEmail = "abc@xyz.com";
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
        private Mock<IInternalsProvider> internalsProviderMock;
        private EmailTemplateConfiguaration emailTemplateConfiguaration;
        private IVClientConfiguration iVClientConfiguration;
        private Common.Email.Contracts.EmailTemplate emailTemplate;

        [TestInitialize]
        public void BeforeEach()
        {
            this.loggerMockFalconQuery = new Mock<ILogger<FalconQuery>>();
            this.falconQueryClient = new Mock<IFalconQueryClient>();
            this.falconQuery = new Mock<FalconQuery>(this.falconQueryClient.Object, this.loggerMockFalconQuery.Object);
            this.notificationClient = new Mock<INotificationClient>();
            this.mockConfigurationManager = new Mock<IConfigurationManager>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.loggerMock = new Mock<ILogger<BEM.EmailManager>>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.emailTemplateConfiguaration = new EmailTemplateConfiguaration
            {
                InterviewerDeclineEmailTemplate = "Interviewer decline email template"
            };
            this.iVClientConfiguration = new IVClientConfiguration
            {
                RecruitingHubClientUrl = "randomemail@random.xyz"
            };
            this.emailTemplate = new Common.Email.Contracts.EmailTemplate
            {
                Subject = "Interviewer has declined the Interview",
                Body = "body {Call_To_Action_Link}"
            };
            this.mockConfigurationManager.Setup(mcm => mcm.Get<EmailTemplateConfiguaration>()).Returns(this.emailTemplateConfiguaration);
            this.mockConfigurationManager.Setup(mcm => mcm.Get<IVClientConfiguration>()).Returns(this.iVClientConfiguration);
            this.mockScheduleQuery.Setup(msq => msq.GetTemplate(It.IsAny<string>())).ReturnsAsync(this.emailTemplate);
            this.manager = this.GetEmailManagerInstance();
        }

        [TestMethod]
        public async Task SendDeclineEmailToSchedulerAsync_ValidBusinessCase()
        {
            List<NotificationItem> notificationItems = new List<NotificationItem>();
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>())).Callback<List<NotificationItem>>((nts) => notificationItems = nts).ReturnsAsync(true);
            InterviewerResponseNotification notification = new InterviewerResponseNotification
            {
                InterviewerOid = Guid.NewGuid().ToString(),
                JobApplicationId = Guid.NewGuid().ToString(),
                ScheduleId = Guid.NewGuid().ToString(),
                ResponseStatus = TalentEntities.Enum.InvitationResponseStatus.Declined
            };
            this.mockScheduleQuery.Setup(msq => msq.GetScheduleByScheduleId(notification.ScheduleId)).ReturnsAsync(new MeetingInfo
            {
                Requester = new GraphPerson
                {
                    Email = RequesterEmail
                }
            });

            this.mockScheduleQuery.Setup(msq => msq.GetJobApplication(notification.JobApplicationId)).ReturnsAsync(new Common.Provisioning.Entities.FalconEntities.Attract.JobApplication
            {
                JobOpening = new Common.Provisioning.Entities.FalconEntities.Attract.JobOpening
                {
                    ExternalJobOpeningID = "123456"
                },
                Candidate = new Common.Provisioning.Entities.FalconEntities.Attract.Candidate
                {
                    FullName = new PersonName
                    {
                        GivenName = "abc",
                        Surname = "xyz"
                    }
                }
            });

            await this.manager.SendDeclineEmailToScheduler(notification).ConfigureAwait(false);
            this.notificationClient.Verify(nt => nt.SendEmail(notificationItems), Times.Once);
            this.mockScheduleQuery.Verify(sq => sq.GetJobApplication(notification.JobApplicationId), Times.Once());
            this.mockScheduleQuery.Verify(sq => sq.GetScheduleByScheduleId(notification.ScheduleId), Times.Once());

        }

        private BEM.EmailManager GetEmailManagerInstance()
        {
            return new BEM.EmailManager(this.notificationClient.Object, this.falconQuery.Object, this.mockConfigurationManager.Object, this.mockTraceSource.Object, this.mockScheduleQuery.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.loggerMock.Object);
        }
    }
}
