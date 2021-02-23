//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.ScheduleService.UnitTest.Business.ScheduleManager_Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.NotifyCandidate;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;
    using Newtonsoft.Json;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ScheduleManager_SendInterviewScheduleToApplicantAsyncTests
    {
        private const string jobApplicationId = "Test Job Application Id";

        private const string rootActivityId = "D126FDDCC2BA4A15B715A29E8AE626B4";

        private const string userObjectId = "Test User Oid";

        private const string userEmail = "abc@xyz.com";

        private Mock<IScheduleQuery> scheduleQueryMock;

        private FalconQuery falconQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<FalconQuery>> falconLoggerMock;

        private Mock<IServiceBusHelper> serviceBusHelperMock;

        private Mock<IGraphSubscriptionManager> graphSubscriptionManagerMock;

        private Mock<IEmailClient> emailClientMock;

        private Mock<IOutlookProvider> outlookProviderMock;

        private Mock<INotificationClient> notificationClientMock;

        private Mock<ILogger<ScheduleManager>> loggerMock;

        private Mock<IEmailHelper> emailHelperMock;

        private readonly ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IInternalsProvider> internalsProviderMock;

        private Mock<IUserDetailsProvider> userDetailsProviderMock;

        private ScheduleManager scheduleManager;

        private ScheduleInvitationRequest scheduleInvitationRequest;

        private Mock<ICandidateCommunicator> candidateCommunicatorMock;

        private Mock<IHCMServiceContext> contextMock;

        private Mock<IConfigurationManager> configMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.contextMock = new Mock<IHCMServiceContext>();
            this.contextMock.SetupGet(cm => cm.RootActivityId).Returns(rootActivityId);
            this.candidateCommunicatorMock = new Mock<ICandidateCommunicator>();
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.serviceBusHelperMock = new Mock<IServiceBusHelper>();
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.configMock = new Mock<IConfigurationManager>();
            this.scheduleInvitationRequest = new ScheduleInvitationRequest
            {
                PrimaryEmailRecipient = "abc@xyz.com",
                CcEmailAddressList = new List<string>(),
                Subject = "Test subject",
                EmailContent = "Test email body",
                IsInterviewScheduleShared = true,
                IsInterviewTitleShared = true,
                SharedSchedules = new List<CandidateScheduleCommunication>
                {
                    new CandidateScheduleCommunication
                    {
                        IsInterviewerNameShared = true,
                        IsInterviewScheduleShared = true,
                        ScheduleId = "Test Schedule Id"
                    }
                }
            };

            this.scheduleManager = new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task SendInterviewScheduleToApplicantAsync_InvalidJobApplicationId(string jobAppId)
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await this.scheduleManager.SendInterviewScheduleToApplicantAsync(jobAppId, this.scheduleInvitationRequest, userEmail));
            Assert.IsTrue(exception.Message.Equals("jobApplicationId", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendInterviewScheduleToApplicantAsync_NullScheduleInvitationRequest()
        {
            this.scheduleInvitationRequest = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.scheduleManager.SendInterviewScheduleToApplicantAsync(jobApplicationId, this.scheduleInvitationRequest, userEmail));
            Assert.IsTrue(exception.ParamName.Equals("scheduleInvitationRequest", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendInterviewScheduleToApplicantAsync_ValidInput()
        {
            JobApplication jobApplication = new JobApplication
            {
                JobApplicationID = "Test Job Application Id",
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant> { new JobApplicationParticipant { OID = "Test HM OID", Role = JobParticipantRole.HiringManager } },
                IsScheduleSentToCandidate = false
            };

            var schedules = this.GetSchedules();
            JobApplicationScheduleMailDetails scheduleMailDetails = new JobApplicationScheduleMailDetails { ScheduleMailSubject = "Test Subject" };
            this.scheduleQueryMock.Setup(sq => sq.UpdateScheduleWithSharingStatusAsync(It.IsAny<CandidateScheduleCommunication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.UpdateJobApplicaton(It.IsAny<JobApplication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.GetScheduleMailDetails(jobApplicationId)).ReturnsAsync(scheduleMailDetails);
            this.scheduleQueryMock.Setup(a => a.GetJobApplication(jobApplicationId)).ReturnsAsync(jobApplication);
            this.scheduleQueryMock.Setup(sq => sq.GetSchedulesByJobApplicationId(jobApplicationId)).ReturnsAsync(schedules);
            this.candidateCommunicatorMock.Setup(cc => cc.SendInvitationAsync(It.IsAny<JobApplication>(), this.scheduleInvitationRequest, It.IsAny<List<MeetingInfo>>())).ReturnsAsync(true);
            this.internalsProviderMock.Setup(ip => ip.GetCandidateCommunicator(It.IsAny<string>())).Returns(this.candidateCommunicatorMock.Object);
            await this.scheduleManager.SendInterviewScheduleToApplicantAsync(jobApplicationId, this.scheduleInvitationRequest, userEmail);
        }

        [TestMethod]
        public async Task SendInterviewScheduleToApplicantAsync_ValidInputForWobScenario()
        {
            string wobUserId = Guid.NewGuid().ToString();
            JobApplication jobApplication = new JobApplication
            {
                JobApplicationID = "Test Job Application Id",
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant> { new JobApplicationParticipant { OID = "Test HM OID", Role = JobParticipantRole.HiringManager } },
                IsScheduleSentToCandidate = false
            };

            Delegation delegation = new Delegation
            {
                From = new Worker { OfficeGraphIdentifier = userObjectId, FullName = "Scheduler", EmailPrimary = "scheduler@xyz.com" },
                To = new Worker { OfficeGraphIdentifier = wobUserId, FullName = "WobUserId", EmailPrimary = "wob@xyz.com" }
            };

            var schedules = this.GetSchedules();
            JobApplicationScheduleMailDetails scheduleMailDetails = new JobApplicationScheduleMailDetails { ScheduleMailSubject = "Test Subject" };
            this.scheduleQueryMock.Setup(sq => sq.UpdateScheduleWithSharingStatusAsync(It.IsAny<CandidateScheduleCommunication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.UpdateJobApplicaton(It.IsAny<JobApplication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.GetScheduleMailDetails(jobApplicationId)).ReturnsAsync(scheduleMailDetails);
            this.scheduleQueryMock.Setup(a => a.GetJobApplication(jobApplicationId)).ReturnsAsync(jobApplication);
            this.scheduleQueryMock.Setup(wob => wob.GetWobUsersDelegation(userObjectId)).Returns(Task.FromResult(new List<Delegation> { delegation }));
            this.scheduleQueryMock.Setup(sq => sq.GetSchedulesByJobApplicationId(jobApplicationId)).ReturnsAsync(schedules);
            this.candidateCommunicatorMock.Setup(cc => cc.SendInvitationAsync(It.IsAny<JobApplication>(), this.scheduleInvitationRequest, It.IsAny<List<MeetingInfo>>())).ReturnsAsync(true);
            this.internalsProviderMock.Setup(ip => ip.GetCandidateCommunicator(It.IsAny<string>())).Returns(this.candidateCommunicatorMock.Object);
            this.scheduleInvitationRequest.CcEmailAddressList.Add(userEmail);
            await this.scheduleManager.SendInterviewScheduleToApplicantAsync(jobApplicationId, this.scheduleInvitationRequest, userEmail, userObjectId, true);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleViolationException))]

        public async Task SendInterviewScheduleToApplicantAsync_WhenApplicationClosedOrOffered()
        {
            JobApplication jobApplication = null;

            var schedules = this.GetSchedules();
            JobApplicationScheduleMailDetails scheduleMailDetails = new JobApplicationScheduleMailDetails { ScheduleMailSubject = "Test Subject" };
            this.scheduleQueryMock.Setup(sq => sq.UpdateScheduleWithSharingStatusAsync(It.IsAny<CandidateScheduleCommunication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.UpdateJobApplicaton(It.IsAny<JobApplication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.GetScheduleMailDetails(jobApplicationId)).ReturnsAsync(scheduleMailDetails);
            this.scheduleQueryMock.Setup(a => a.GetJobApplication(jobApplicationId)).ReturnsAsync(jobApplication);
            this.scheduleQueryMock.Setup(sq => sq.GetSchedulesByJobApplicationId(jobApplicationId)).ReturnsAsync(schedules);
            this.candidateCommunicatorMock.Setup(cc => cc.SendInvitationAsync(It.IsAny<JobApplication>(), this.scheduleInvitationRequest, It.IsAny<List<MeetingInfo>>())).ReturnsAsync(true);
            this.internalsProviderMock.Setup(ip => ip.GetCandidateCommunicator(It.IsAny<string>())).Returns(this.candidateCommunicatorMock.Object);
            await this.scheduleManager.SendInterviewScheduleToApplicantAsync(jobApplicationId, this.scheduleInvitationRequest, userEmail);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleViolationException))]

        public async Task SendInterviewScheduleToApplicantAsync_ApplicationHasNoSchedule()
        {
            string jobApplicationId = Guid.NewGuid().ToString();
            JobApplication jobApplication = new JobApplication
            {
                JobApplicationID = jobApplicationId,
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant> { new JobApplicationParticipant { OID = "Test HM OID", Role = JobParticipantRole.HiringManager } },
                IsScheduleSentToCandidate = false
            };

            List<MeetingInfo> schedules = null; // No schedule from query layer
            JobApplicationScheduleMailDetails scheduleMailDetails = new JobApplicationScheduleMailDetails { ScheduleMailSubject = "Test Subject" };
            this.scheduleQueryMock.Setup(sq => sq.UpdateScheduleWithSharingStatusAsync(It.IsAny<CandidateScheduleCommunication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.UpdateJobApplicaton(It.IsAny<JobApplication>())).ReturnsAsync(true);
            this.scheduleQueryMock.Setup(sq => sq.GetScheduleMailDetails(jobApplicationId)).ReturnsAsync(scheduleMailDetails);
            this.scheduleQueryMock.Setup(a => a.GetJobApplication(jobApplicationId)).ReturnsAsync(jobApplication);
            this.scheduleQueryMock.Setup(sq => sq.GetSchedulesByJobApplicationId(jobApplicationId)).ReturnsAsync(schedules);
            this.candidateCommunicatorMock.Setup(cc => cc.SendInvitationAsync(It.IsAny<JobApplication>(), this.scheduleInvitationRequest, It.IsAny<List<MeetingInfo>>())).ReturnsAsync(true);
            this.internalsProviderMock.Setup(ip => ip.GetCandidateCommunicator(It.IsAny<string>())).Returns(this.candidateCommunicatorMock.Object);
            await this.scheduleManager.SendInterviewScheduleToApplicantAsync(jobApplicationId, this.scheduleInvitationRequest, userEmail);
        }

        private List<MeetingInfo> GetSchedules()
        {
            var filePath = ".\\Data\\testschedules.json";
            var fileContent = File.ReadAllText(filePath);
            List<MeetingInfo> schedules = JsonConvert.DeserializeObject<List<MeetingInfo>>(fileContent);
            return schedules;
        }

        private Timezone GetTimezone()
        {
            Timezone timezone = new Timezone
            {
                TimezoneName = "Asia/Calcutta",
                TimezoneAbbr = "IST",
                UTCOffset = 330,
                UTCOffsetInHours = "+05:30"
            };

            return timezone;
        }
    }
}
