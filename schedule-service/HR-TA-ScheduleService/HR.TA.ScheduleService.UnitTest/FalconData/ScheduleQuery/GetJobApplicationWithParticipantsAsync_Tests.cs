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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Contracts;
    using HR.TA.Common.DocumentDB;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.TalentEntities.Enum;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetJobApplicationWithParticipantsAsync_Tests
    {
        private static readonly string participant1Oid = Guid.NewGuid().ToString();

        private static readonly string participant2Oid = Guid.NewGuid().ToString();

        private static readonly string jobApplicationId = Guid.NewGuid().ToString();

        private JobApplication jobApplication;

        private Mock<IHcmDocumentClient> clientMock;

        private List<Worker> workers;

        private ScheduleQuery scheduleQuery;

        private SearchMetadataResponse searchMetadataResponse;

        private Mock<IFalconQueryClient> falconClientMock;

        [TestInitialize]
        public void BeforeEach()
        {
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
            };
            this.jobApplication = new JobApplication
            {
                JobApplicationID = jobApplicationId,
                JobApplicationParticipants = new List<JobApplicationParticipant>
                {
                    new JobApplicationParticipant
                    {
                        OID = participant1Oid,
                        Role = JobParticipantRole.Interviewer,
                    },
                    new JobApplicationParticipant
                    {
                        OID = participant2Oid,
                        Role = JobParticipantRole.Contributor,
                    },
                },
                Status = JobApplicationStatus.Active,
            };
            this.searchMetadataResponse = new SearchMetadataResponse();
            this.searchMetadataResponse.Result = this.workers;
            TraceSourceMeta.LoggerFactory = new NullLoggerFactory();
            this.clientMock = new Mock<IHcmDocumentClient>();
            this.falconClientMock = new Mock<IFalconQueryClient>();
            this.falconClientMock.Setup(fcm => fcm.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.clientMock.Object);
            this.scheduleQuery = new ScheduleQuery(this.falconClientMock.Object, NullLogger<ScheduleQuery>.Instance);
        }

        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataTestMethod]
        public async Task GetJobApplicationWithParticipantsAsync_InvalidJobApplicationId(string jobApplicationId)
        {
            var ex = await Assert.ThrowsExceptionAsync<OperationFailedException>(async () => await this.scheduleQuery.GetJobApplicationWithParticipantsAsync(jobApplicationId).ConfigureAwait(false));
            Assert.IsTrue(ex.Message.Contains("The job application Id is missing.", StringComparison.Ordinal));
        }

        [TestMethod]
        public async Task GetJobApplicationWithParticipantsAsync_ValidInput()
        {
            this.clientMock.Setup(cm => cm.GetFirstOrDefault<JobApplication>(It.IsAny<Expression<Func<JobApplication, bool>>>(), It.IsAny<FeedOptions>())).ReturnsAsync(this.jobApplication);
            this.clientMock.Setup(cm => cm.GetWithPagination(It.IsAny<Expression<Func<Worker, bool>>>(), It.IsAny<FeedOptions>(), 0, 0)).ReturnsAsync(this.searchMetadataResponse);
            var jobAppicationParticipants = await this.scheduleQuery.GetJobApplicationWithParticipantsAsync(jobApplicationId).ConfigureAwait(false);
            Assert.IsNotNull(jobAppicationParticipants.Application);
            Assert.IsNotNull(jobAppicationParticipants.Participants);
            Assert.AreEqual(jobAppicationParticipants.Participants.Count, 2);

        }
    }
}
