//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.UnitTest.FalconData.ScheduleQuery_Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Graph;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Contracts;
    using HR.TA.Common.DocumentDB;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.FalconEntities.Attract;
    using HR.TA.TalentEntities.Enum;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetAllPendingFeedbacksForReminderAsyncTest
    {
        private static readonly string jobOpeningId = Guid.NewGuid().ToString();

        private static readonly string externalJobOpeningId = Guid.NewGuid().ToString();

        private static readonly string participant1Oid = Guid.NewGuid().ToString();

        private static readonly string participant2Oid = Guid.NewGuid().ToString();

        private static readonly string hiringManagerOid = Guid.NewGuid().ToString();

        private static readonly string recruiterOid = Guid.NewGuid().ToString();

        private static readonly string jobApplicationId = Guid.NewGuid().ToString();

        private static readonly string candidateOid = Guid.NewGuid().ToString();

        private static readonly DateTime currentDateTime = DateTime.UtcNow;

        private static readonly int offsetHour = 24;

        private static readonly int windowMinutes = 1440;

        private JobOpening jobOpening;

        private List<JobApplication> jobApplications;

        private List<JobApplicationSchedule> schedules;

        private Mock<IHcmDocumentClient> clientMock;

        private List<Worker> workers;

        private ScheduleQuery scheduleQuery;

        private SearchMetadataResponse searchMetadataResponse;

        private Mock<IFalconQueryClient> falconClientMock;

        private Candidate candidate;

        [TestInitialize]
        public void BeforeEach()
        {
            this.jobOpening = new JobOpening
            {
                JobOpeningID = jobOpeningId,
                PositionTitle = "Software Engineer",
                ExternalJobOpeningID = externalJobOpeningId
            };

            this.workers = new List<Worker>
            {
                new Worker
                {
                    WorkerId = participant1Oid,
                    OfficeGraphIdentifier = participant1Oid,
                    Name = new PersonName
                    {
                        GivenName = "Given 1",
                        Surname = "Surname 1",
                    },
                    EmailPrimary = "primary1@somedomain.com",
                },
                new Worker
                {
                    WorkerId = participant2Oid,
                    OfficeGraphIdentifier = participant2Oid,
                    Name = new PersonName
                    {
                        GivenName = "Given 2",
                        Surname = "Surname 2",
                    },
                    EmailPrimary = "primary2@somedomain.com",
                },
                new Worker
                {
                    WorkerId = hiringManagerOid,
                    OfficeGraphIdentifier = hiringManagerOid,
                    Name = new PersonName
                    {
                        GivenName = "Awesome",
                        Surname = "Hiring Manager",
                    },
                    EmailPrimary = "hiring.manager@somedomain.com",
                },
                new Worker
                {
                    WorkerId = recruiterOid,
                    OfficeGraphIdentifier = recruiterOid,
                    Name = new PersonName
                    {
                        GivenName = "Best",
                        Surname = "Ever Recruiter",
                    },
                    EmailPrimary = "best.recruiter@somedomain.com",
                },
                new Worker
                {
                    WorkerId = candidateOid,
                    OfficeGraphIdentifier = candidateOid,
                    Name = new PersonName
                    {
                        GivenName = "Brilliant",
                        Surname = "Candidate",
                    },
                    EmailPrimary = "brc@somedomain.com",
                }
            };

            this.candidate = new Candidate
            {
                CandidateID = candidateOid,
                FullName = new PersonName
                {
                    GivenName = "Brilliant",
                    Surname = "Candidate",
                }
            };

            this.schedules = new List<JobApplicationSchedule>
            {
                new JobApplicationSchedule
                {
                    EndDateTime = currentDateTime.AddDays(-1 * offsetHour).AddHours(-1),
                    JobApplicationID = jobApplicationId,
                    Participants = new List<JobApplicationScheduleParticipant>
                    {
                        new JobApplicationScheduleParticipant
                        {
                            OID = participant2Oid,
                            Role = JobParticipantRole.Interviewer,
                            IsAssessmentCompleted = false
                        },
                        new JobApplicationScheduleParticipant
                        {
                            OID = participant1Oid,
                            Role = JobParticipantRole.Interviewer,
                            IsAssessmentCompleted = true
                        },
                    },
                    ScheduleRequester = this.workers[0]
                },
                new JobApplicationSchedule
                {
                    EndDateTime = currentDateTime.AddDays(-1 * offsetHour).AddHours(-1),
                    JobApplicationID = jobApplicationId,
                    Participants = new List<JobApplicationScheduleParticipant>
                    {
                        new JobApplicationScheduleParticipant
                        {
                            OID = participant2Oid,
                            Role = JobParticipantRole.Interviewer,
                            IsAssessmentCompleted = true
                        },
                        new JobApplicationScheduleParticipant
                        {
                            OID = participant1Oid,
                            Role = JobParticipantRole.Interviewer,
                            IsAssessmentCompleted = false
                        },
                    },
                    ScheduleRequester = this.workers[1]
                },
                new JobApplicationSchedule
                {
                    EndDateTime = currentDateTime.AddDays(-1 * offsetHour).AddHours(-2),
                    JobApplicationID = jobApplicationId,
                    Participants = new List<JobApplicationScheduleParticipant>
                    {
                        new JobApplicationScheduleParticipant
                        {
                            OID = participant2Oid,
                            Role = JobParticipantRole.Interviewer,
                            IsAssessmentCompleted = false
                        },
                        new JobApplicationScheduleParticipant
                        {
                            OID = participant1Oid,
                            Role = JobParticipantRole.Interviewer,
                            IsAssessmentCompleted = false
                        },
                    },
                    ScheduleRequester = this.workers[0]
                }
            };

            this.jobApplications = new List<JobApplication>
            {
                new JobApplication {
                    JobApplicationID = jobApplicationId,
                    JobApplicationParticipants = new List<JobApplicationParticipant>
                    {
                        new JobApplicationParticipant
                        {
                            OID = participant2Oid,
                            Role = JobParticipantRole.Interviewer,
                        },
                        new JobApplicationParticipant
                        {
                            OID = participant1Oid,
                            Role = JobParticipantRole.Interviewer,
                        },
                        new JobApplicationParticipant
                        {
                            OID = hiringManagerOid,
                            Role = JobParticipantRole.HiringManager,
                        },
                        new JobApplicationParticipant
                        {
                            OID = recruiterOid,
                            Role = JobParticipantRole.Recruiter,
                        },
                    },
                    Status = JobApplicationStatus.Active,
                    Candidate = this.candidate,
                    JobApplicationSchedules = new List<JobApplicationSchedule>
                    {
                        this.schedules[0]
                    },
                    JobOpening = this.jobOpening
                }
            };
                
            TraceSourceMeta.LoggerFactory = new NullLoggerFactory();
            this.clientMock = new Mock<IHcmDocumentClient>();
            this.falconClientMock = new Mock<IFalconQueryClient>();
            this.falconClientMock.Setup(fcm => fcm.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.clientMock.Object);
            this.scheduleQuery = new ScheduleQuery(this.falconClientMock.Object, NullLogger<ScheduleQuery>.Instance);
        }

        [TestMethod]
        public async Task GetAllPendingFeedbacksForReminderAsyncTest_InvalidOffsetDurationHours()
        {
            var ex = await Assert.ThrowsExceptionAsync<OperationFailedException>(async () => await this.scheduleQuery.GetAllPendingFeedbacksForReminderAsync(-1, 10).ConfigureAwait(false));
            Assert.IsTrue(ex.Message.Contains("Invalid feedback reminder offset duration.", StringComparison.Ordinal));
        }

        [TestMethod]
        public async Task GetAllPendingFeedbacksForReminderAsyncTest_InvalidWindowMinutes()
        {
            var ex = await Assert.ThrowsExceptionAsync<OperationFailedException>(async () => await this.scheduleQuery.GetAllPendingFeedbacksForReminderAsync(10, -1).ConfigureAwait(false));
            Assert.IsTrue(ex.Message.Contains("Invalid reminder window duration.", StringComparison.Ordinal));
        }

        [TestMethod]
        public async Task GetAllPendingFeedbacksForReminderAsync_ValidInput()
        {
            this.clientMock.Setup(cm => cm.Get<JobApplicationSchedule>(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), It.IsAny<FeedOptions>())).ReturnsAsync(this.schedules);
            this.clientMock.Setup(cm => cm.Get<JobApplication>(It.IsAny<Expression<Func<JobApplication, bool>>>(), It.IsAny<FeedOptions>())).ReturnsAsync(this.jobApplications);
            this.clientMock.Setup(cm => cm.Get<Worker>(It.IsAny<Expression<Func<Worker, bool>>>(), It.IsAny<FeedOptions>())).ReturnsAsync(this.workers);
            this.clientMock
                .Setup(c => c.GetWithProjections<Candidate, Candidate>(It.IsAny<Expression<Func<Candidate, bool>>>(), It.IsAny<Expression<Func<Candidate, Candidate>>>(), null))
                .ReturnsAsync(new List<Candidate>() { this.candidate });
            this.clientMock
                .Setup(c => c.GetWithProjections<JobOpening, JobOpening>(It.IsAny<Expression<Func<JobOpening, bool>>>(), It.IsAny<Expression<Func<JobOpening, JobOpening>>>(), null))
                .ReturnsAsync(new List<JobOpening>() { this.jobOpening });

            var pendingFeedbacks = await this.scheduleQuery.GetAllPendingFeedbacksForReminderAsync(offsetHour, windowMinutes).ConfigureAwait(false);
            Assert.IsNotNull(pendingFeedbacks);
            Assert.AreEqual(this.candidate.FullName.GivenName + " " + this.candidate.FullName.Surname, pendingFeedbacks[0].CandidateName, "Expected candidate name to be same.");
            Assert.AreEqual(this.jobOpening.ExternalJobOpeningID, pendingFeedbacks[0].ExternalJobOpeningId);
        }
    }
}
