namespace MS.GTA.ScheduleService.UnitTest.Business.NotifyCandidate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.NotifyCandidate;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;
    using Newtonsoft.Json;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CandidateCommunicator_SendIInvitationAsyncTests
    {
        private const string jobApplicationId = "Test Job Application Id";

        private const string userEmail = "abc@xyz.com";

        private Mock<IScheduleQuery> scheduleQueryMock;

        private Mock<IHCMPrincipalRetriever> principalRetrieverMock;

        private Mock<INotificationClient> notificationClientMock;

        private Mock<IEmailHelper> emailHelperMock;

        private Mock<IInternalsProvider> internalsProviderMock;

        private ScheduleInvitationRequest scheduleInvitationRequest;

        private CandidateCommunicator candidateCommunicator;

        private Mock<ILogger<CandidateCommunicator>> loggerMock;

        private JobApplication jobApplication;

        [TestInitialize]
        public void BeforEach()
        {
            this.jobApplication = new JobApplication
            {
                JobApplicationID = "Test Job Application Id",
                Status = JobApplicationStatus.Active,
                JobApplicationParticipants = new List<JobApplicationParticipant> { new JobApplicationParticipant { OID = "Test HM OID", Role = JobParticipantRole.HiringManager } },
                IsScheduleSentToCandidate = false
            };

            this.principalRetrieverMock = new Mock<IHCMPrincipalRetriever>();
            this.loggerMock = new Mock<ILogger<CandidateCommunicator>>();
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            CandidateCommunicatorMakers communicatorMakers = new CandidateCommunicatorMakers
            {
                ScheduleQuery = this.scheduleQueryMock.Object,
                EmailHelper = this.emailHelperMock.Object,
                NotificationClient = this.notificationClientMock.Object,
                RequesterEmail = userEmail
            };

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

            this.candidateCommunicator = new CandidateCommunicator(communicatorMakers, this.loggerMock.Object);
        }

        [TestMethod]
        public async Task SendInvitationAsync_NullJobApplication()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.candidateCommunicator.SendInvitationAsync(jobApplication: null, scheduleInvitationRequest: this.scheduleInvitationRequest, schedules: this.GetSchedules()));
            Assert.IsTrue(exception.ParamName.Equals("jobApplication", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendInvitationAsync_NullSchedules()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.candidateCommunicator.SendInvitationAsync(this.jobApplication, this.scheduleInvitationRequest, schedules: null));
            Assert.IsTrue(exception.ParamName.Equals("schedules", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendInvitationAsync_NullScheduleInvitationRequest()
        {
            this.scheduleInvitationRequest = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.candidateCommunicator.SendInvitationAsync(this.jobApplication, this.scheduleInvitationRequest, this.GetSchedules()));
            Assert.IsTrue(exception.ParamName.Equals("scheduleInvitationRequest", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task SendInvitationAsync_ValidInput()
        {
            var schedules = this.GetSchedules();
            JobApplicationScheduleMailDetails scheduleMailDetails = new JobApplicationScheduleMailDetails { ScheduleMailSubject = "Test Subject" };
            this.notificationClientMock.Setup(nc => nc.SendEmail(It.IsAny<List<NotificationItem>>(), It.IsAny<string>())).ReturnsAsync(true);
            this.emailHelperMock.Setup(em => em.GetScheduleSummaryAsync(schedules, this.scheduleInvitationRequest, this.GetTimezone())).ReturnsAsync("Test Summary");
            this.scheduleQueryMock.Setup(sq => sq.GetScheduleMailDetails(jobApplicationId)).ReturnsAsync(scheduleMailDetails);
            this.scheduleQueryMock.Setup(sq => sq.GetTimezoneForJobApplication(jobApplicationId)).ReturnsAsync(this.GetTimezone());
            Task task = this.candidateCommunicator.SendInvitationAsync(this.jobApplication, this.scheduleInvitationRequest, this.GetSchedules());
            task.Wait();
            Assert.IsTrue(task.Status == TaskStatus.RanToCompletion);
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