//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.Business.EmailManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.Security;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Email.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ScheduleService.FalconData.ViewModelExtensions;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;
    using Newtonsoft.Json;
    using BEM = BusinessLibrary.Business.V1;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendShareFeedbackEmailToAATests
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
        private Mock<IHCMApplicationPrincipal> principalMock;
        private Mock<IInternalsProvider> internalsProviderMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.loggerMockFalconQuery = new Mock<ILogger<FalconQuery>>();
            this.principalMock = new Mock<IHCMApplicationPrincipal>();
            this.falconQueryClient = new Mock<IFalconQueryClient>();
            this.falconQuery = new Mock<FalconQuery>(this.falconQueryClient.Object, this.loggerMockFalconQuery.Object);
            this.notificationClient = new Mock<INotificationClient>();
            this.mockConfiguarationManager = new Mock<IConfigurationManager>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.loggerMock = new Mock<ILogger<BEM.EmailManager>>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.manager = new BEM.EmailManager( this.notificationClient.Object, this.falconQuery.Object, this.mockConfiguarationManager.Object, this.mockTraceSource.Object, this.mockScheduleQuery.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.loggerMock.Object);
        }

        [TestMethod]
        public async Task SendShareFeedbackEmailToAAWithNullJobApplicationId()
        {
            string[] oids = new string[1];
            oids.SetValue("testOid", 0);
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendShareFeedbackEmailToAA(null, oids));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid application or team member", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithEmptyJobApplicationId()
        {
            string[] oids = new string[1];
            oids.SetValue("testOid", 0);
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendShareFeedbackEmailToAA(string.Empty, oids));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithWhiteSpaceJobApplicationId()
        {
            string[] oids = new string[1];
            oids.SetValue("testOid", 0);
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendShareFeedbackEmailToAA(" ", oids));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendShareFeedbackEmailToAAForValidBusinessCase()
        {
            var filePath = ".\\Data\\sharingFeedbackTemplate.json";
            var fileContent = File.ReadAllText(filePath);
            string[] oids = new string[1];
            oids.SetValue("Test Scheduler OID", 0);
            FalconEmailTemplate falconEmailTemplate = JsonConvert.DeserializeObject<FalconEmailTemplate>(fileContent);
            Common.Email.Contracts.EmailTemplate sendAATemplate = falconEmailTemplate.ToContract();
            JobApplication jobApplication = new JobApplication
            {
                JobApplicationID = "Test Job Application Id",
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant> {
                    new JobApplicationParticipant { OID = "Test Scheduler OID", Role = JobParticipantRole.Contributor }
                },
                JobOpening = new JobOpening { PositionTitle = "Test Engineer", ExternalJobOpeningID = "3832" },
                Candidate = new Candidate { FullName = new PersonName { GivenName = "Test Candidate" } },
            };

            Worker worker = new Worker { OfficeGraphIdentifier = "Test Scheduler OID", FullName = "Scheduler", EmailPrimary = "scheduler@xyz.com" };
            IVClientConfiguration clientConfiguration = new IVClientConfiguration { RecruitingHubClientUrl = "clienturl" };

            this.principalMock.SetupGet(principal => principal.UserObjectId).Returns("Test Scheduler OID");
            this.principalMock.SetupGet(principal => principal.GivenName).Returns("HM Given Name");
            this.principalMock.SetupGet(principal => principal.EmailAddress).Returns("hm@xyz.com");

            EmailTemplateConfiguaration emailTemplateConfiguaration = new EmailTemplateConfiguaration { };
            this.mockConfiguarationManager
                .Setup(c => c.Get<EmailTemplateConfiguaration>())
                .Returns(emailTemplateConfiguaration);
            this.mockScheduleQuery
                .Setup(c => c.GetTemplate(It.IsAny<string>()))
                .ReturnsAsync(sendAATemplate);
            this.mockConfiguarationManager
                .Setup(c => c.Get<IVClientConfiguration>())
                .Returns(clientConfiguration);
            this.falconQuery.Setup(q => q.GetWorker(It.IsAny<string>())).ReturnsAsync(worker);
            this.mockScheduleQuery.Setup(ja => ja.GetJobApplication(It.IsAny<string>())).ReturnsAsync(jobApplication);
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);

            var mailSent = await this.manager.SendShareFeedbackEmailToAA("Test Job Application Id ", oids);
            Assert.IsTrue(mailSent);
        }
    }
}
