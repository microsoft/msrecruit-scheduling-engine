//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.Business.EmailManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.Security;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;
    using BEM = BusinessLibrary.Business.V1;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SendFeedbackEmailTests
    {
        private Mock<IFalconQueryClient> falconQueryClient;
        private Mock<FalconQuery> falconQuery;
        private BEM.EmailManager manager;
        private Mock<ITraceSource> mockTraceSource;
        private Mock<INotificationClient> notificationClient;
        private Mock<IConfigurationManager> mockConfiguarationManager;
        private Mock<IScheduleQuery> mockScheduleQuery;
        private Mock<ILogger<BEM.EmailManager>> loggerMock;
        private Mock<ILogger<FalconQuery>> loggerMockFalconQuery;
        private Mock<IEmailHelper> emailHelperMock;
        private Mock<IHCMApplicationPrincipal> principalMock;
        private EmailNotificationRequest notificationRequest;
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
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.loggerMock = new Mock<ILogger<BEM.EmailManager>>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.notificationRequest = new EmailNotificationRequest
            {
                JobApplicationId = "Test Job Application Id",
                EmailBody = "This is a email body.",
                EmailFooter = "This is a email footer",
                Subject = "I'm a subject.",
                MailTo = new List<GraphPerson>
                       {
                           new GraphPerson
                           {
                               Id = "Test User OId",
                               Email = "abc@xyz.com"
                           }
                       },
                MailCC = new List<GraphPerson>
                       {
                           new GraphPerson
                           {
                               Id = "Test HM OID",
                               Email = "hm@xyz.com"
                           }
                       }
            };
            this.manager = this.GetEmailManagerInstance();
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithNullParameter()
        {
            EmailNotificationRequest emailNotificationRequest = null;
            _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.manager.SendFeedbackEmailAsync(emailNotificationRequest));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithNullJobApplicationId()
        {
            this.notificationRequest.JobApplicationId = null;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid application id", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithEmptyJobApplicationId()
        {
            this.notificationRequest.JobApplicationId = string.Empty;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid application id", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithWhiteSpaceJobApplicationId()
        {
            this.notificationRequest.JobApplicationId = " ";
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid application id", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithNullMailTo()
        {
            this.notificationRequest.MailTo = null;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid 'To' members in email.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithEmptyMailTo()
        {
            this.notificationRequest.MailTo = new List<GraphPerson>();
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid 'To' members in email.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithValidNotificationRequestForValidBusinessCase()
        {
            JobApplication jobApplication = new JobApplication
            {
                JobApplicationID = "Test Job Application Id",
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant>
                {
                    new JobApplicationParticipant { OID = "Test HM OID", Role = JobParticipantRole.HiringManager },
                    new JobApplicationParticipant { OID = "Test Recruiter OID", Role = JobParticipantRole.Recruiter },
                    new JobApplicationParticipant { OID = "Test Scheduler OID", Role = JobParticipantRole.Contributor },
                    new JobApplicationParticipant { OID = "Test User OId", Role = JobParticipantRole.Interviewer },
                },
                JobOpening = new JobOpening { PositionTitle = "Test Engineer", ExternalJobOpeningID = "3832" },
                Candidate = new Candidate { FullName = new PersonName { GivenName = "Test Candidate" } },
            };
            List<Worker> workers = new List<Worker>
            {
                new Worker { OfficeGraphIdentifier = "Test HM OID", FullName = "HM", EmailPrimary = "hm@xyz.com" },
                new Worker { OfficeGraphIdentifier = "Test Recruiter OID", FullName = "Recruiter", EmailPrimary = "recruiter@xyz.com" },
                new Worker { OfficeGraphIdentifier = "Test Scheduler OID", FullName = "Scheduler", EmailPrimary = "scheduler@xyz.com" }
            };

            this.principalMock.SetupGet(principal => principal.UserObjectId).Returns("Test HM OID");
            this.principalMock.SetupGet(principal => principal.GivenName).Returns("HM Given Name");
            this.principalMock.SetupGet(principal => principal.EmailAddress).Returns("hm@xyz.com");
            this.falconQuery.Setup(q => q.GetWorkers(It.IsAny<List<string>>())).ReturnsAsync(workers);
            this.mockScheduleQuery.Setup(ja => ja.GetJobApplication(It.IsAny<string>())).ReturnsAsync(jobApplication);
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);

            var mailSent = await this.manager.SendFeedbackEmailAsync(this.notificationRequest);
            Assert.IsTrue(mailSent);
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithValidNotificationRequestForWobBusinessCase()
        {
            string schedulerOid = Guid.NewGuid().ToString();
            string wobUserId = Guid.NewGuid().ToString();
            JobApplication jobApplication = new JobApplication
            {
                JobApplicationID = "Test Job Application Id",
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant>
                {
                    new JobApplicationParticipant { OID = "Test HM OID", Role = JobParticipantRole.HiringManager },
                    new JobApplicationParticipant { OID = "Test Recruiter OID", Role = JobParticipantRole.Recruiter },
                    new JobApplicationParticipant { OID = schedulerOid, Role = JobParticipantRole.Contributor },
                    new JobApplicationParticipant { OID = "Test User OId", Role = JobParticipantRole.Interviewer },
                },
                JobOpening = new JobOpening { PositionTitle = "Test Engineer", ExternalJobOpeningID = "3832" },
                Candidate = new Candidate { FullName = new PersonName { GivenName = "Test Candidate" } },
            };

            var delegation = new Delegation
            {
                From = new Worker { OfficeGraphIdentifier = schedulerOid, FullName = "Scheduler", EmailPrimary = "scheduler@xyz.com" },
                To = new Worker { OfficeGraphIdentifier = wobUserId, FullName = "WobUserId", EmailPrimary = "wob@xyz.com" }
            };

            List<Worker> workers = new List<Worker>
            {
                new Worker { OfficeGraphIdentifier = "Test HM OID", FullName = "HM", EmailPrimary = "hm@xyz.com" },
                new Worker { OfficeGraphIdentifier = "Test Recruiter OID", FullName = "Recruiter", EmailPrimary = "recruiter@xyz.com" },
                new Worker { OfficeGraphIdentifier = schedulerOid, FullName = "Scheduler", EmailPrimary = "scheduler@xyz.com" }
            };

            this.principalMock.SetupGet(principal => principal.UserObjectId).Returns("Test HM OID");
            this.principalMock.SetupGet(principal => principal.GivenName).Returns("HM Given Name");
            this.principalMock.SetupGet(principal => principal.EmailAddress).Returns("hm@xyz.com");
            this.falconQuery.Setup(q => q.GetWorkers(It.IsAny<List<string>>())).ReturnsAsync(workers);
            this.mockScheduleQuery.Setup(wob => wob.GetWobUsersDelegation(schedulerOid)).Returns(Task.FromResult(new List<Delegation> { delegation }));
            this.mockScheduleQuery.Setup(ja => ja.GetJobApplication(It.IsAny<string>())).ReturnsAsync(jobApplication);
            this.notificationClient.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);

            var mailSent = await this.manager.SendFeedbackEmailAsync(this.notificationRequest, schedulerOid, true);
            Assert.IsTrue(mailSent);
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithNullEmailBody()
        {
            this.notificationRequest.EmailBody = null;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid email body.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithEmptyEmailBody()
        {
            this.notificationRequest.EmailBody = string.Empty;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid email body.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithWhiteSpaceEmailBody()
        {
            this.notificationRequest.EmailBody = " ";
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid email body.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithNullSubject()
        {
            this.notificationRequest.Subject = null;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid subject text.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithEmptySubject()
        {
            this.notificationRequest.Subject = string.Empty;
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid subject text.", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendFeedbackEmailWithWhiteSpaceSubject()
        {
            this.notificationRequest.Subject = " ";
            var exception = await Assert.ThrowsExceptionAsync<BusinessRuleViolationException>(async () => await this.manager.SendFeedbackEmailAsync(this.notificationRequest));
            Assert.IsTrue(exception.Message.Contains("Input request does not contain a valid subject text.", StringComparison.OrdinalIgnoreCase));
        }

        private BEM.EmailManager GetEmailManagerInstance()
        {
            return new BEM.EmailManager(this.notificationClient.Object, this.falconQuery.Object, this.mockConfiguarationManager.Object, this.mockTraceSource.Object, this.mockScheduleQuery.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.loggerMock.Object);
        }
    }
}
