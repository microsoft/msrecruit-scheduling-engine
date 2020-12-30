// <copyright file="GetPendingSchedulesTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.DocumentDB;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.FalconEntities.Attract;

    /// <summary>
    /// Tests for GetPendingSchedules method
    /// </summary>
    [TestClass]
    public class GetPendingSchedulesTest
    {
        private IScheduleQuery scheduleQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IHcmDocumentClient> mockDocumentClient;

        private Mock<ILogger<ScheduleQuery>> loggerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.loggerMock = new Mock<ILogger<ScheduleQuery>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.mockDocumentClient = new Mock<IHcmDocumentClient>();
            this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);
            this.scheduleQuery = new ScheduleQuery(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// Test for Get Job Application Valid request
        /// </summary>
        /// <returns><see cref="Task"/> for the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetPendingSchedulesTestValid()
        {
            JobApplicationScheduleParticipant participant1 = new JobApplicationScheduleParticipant { OID = "1234", Role = TalentEntities.Enum.JobParticipantRole.Interviewer, ParticipantStatus = TalentEntities.Enum.InvitationResponseStatus.Pending };
            List<JobApplicationScheduleParticipant> participantList = new List<JobApplicationScheduleParticipant>();
            participantList.Add(participant1);
            JobApplicationSchedule schedule1 = new JobApplicationSchedule()
            {
                ScheduleID = "123",
                Participants = participantList
            };
            IList<JobApplicationSchedule> schedules = new List<JobApplicationSchedule>();
            ScheduleRemind remind1 = new ScheduleRemind()
            {
                ScheduleID = "123"
            };
            schedules.Add(schedule1);
            this.mockDocumentClient
                .Setup(c => c.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                .ReturnsAsync(schedules);
            this.mockDocumentClient
                .Setup(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<ScheduleRemind, bool>>>(), null))
                .ReturnsAsync(remind1);

            var result = await this.scheduleQuery.GetPendingSchedules();

            Assert.IsNotNull(result, "Job application schedule to be not null.");

            this.mockDocumentClient
                .Verify(c => c.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null), Times.Once, "Expected to get single job application once.");
            this.mockDocumentClient
                .Verify(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<ScheduleRemind, bool>>>(), null), Times.Once, "Expected to get single job application once.");

        }

        /// <summary>
        /// Test for Get Job Application Valid request
        /// </summary>
        /// <returns><see cref="Task"/> for the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetPendingSchedulesTestWithInputs()
        {
            JobApplicationScheduleParticipant participant1 = new JobApplicationScheduleParticipant { OID = "1234", Role = TalentEntities.Enum.JobParticipantRole.Interviewer, ParticipantStatus = TalentEntities.Enum.InvitationResponseStatus.Pending };
            List<JobApplicationScheduleParticipant> participantList = new List<JobApplicationScheduleParticipant>();
            participantList.Add(participant1);
            JobApplicationSchedule schedule1 = new JobApplicationSchedule()
            {
                ScheduleID = "123",
                Participants = participantList
            };
            IList<JobApplicationSchedule> schedules = new List<JobApplicationSchedule>();
            ScheduleRemind remind1 = new ScheduleRemind()
            {
                ScheduleID = "123"
            };

            ScheduleRemind scheduleRemaing = null;

            schedules.Add(schedule1);
            this.mockDocumentClient
                .Setup(c => c.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                .ReturnsAsync(schedules);

            this.mockDocumentClient
                .Setup(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<ScheduleRemind, bool>>>(), null))
                .ReturnsAsync(scheduleRemaing);

            var result = await this.scheduleQuery.GetPendingSchedules();

            Assert.IsNotNull(result, "Job application schedule to be not null.");

            this.mockDocumentClient
                .Verify(c => c.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null), Times.Once, "Expected to get single job application once.");
            this.mockDocumentClient
                .Verify(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<ScheduleRemind, bool>>>(), null), Times.Once, "Expected to get single job application once.");
        }

        /// <summary>
        /// Test for Get Job Application Valid request
        /// </summary>
        /// <returns><see cref="Task"/> for the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetPendingSchedulesForRecoveryTestValid()
        {
            JobApplicationScheduleParticipant participant1 = new JobApplicationScheduleParticipant
            {
                OID = "1234",
                Role = TalentEntities.Enum.JobParticipantRole.Interviewer,
                ParticipantStatus = TalentEntities.Enum.InvitationResponseStatus.Pending
            };
            List<JobApplicationScheduleParticipant> participantList = new List<JobApplicationScheduleParticipant>();
            participantList.Add(participant1);
            JobApplicationSchedule schedule1 = new JobApplicationSchedule()
            {
                ScheduleID = "123",
                Participants = participantList,
                StartDateTime = DateTime.Now.AddDays(-2),
                ScheduleStatus = ScheduleStatus.Sent,
            };
            IList<JobApplicationSchedule> schedules = new List<JobApplicationSchedule>();
            schedules.Add(schedule1);

            this.mockDocumentClient
                .Setup(c => c.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                .ReturnsAsync(schedules);
            this.mockDocumentClient
                .Setup(c => c.GetFirstOrDefault(It.IsAny<Expression<Func<ScheduleRemind, bool>>>(), null))
                .ReturnsAsync(It.IsAny<ScheduleRemind>());

            var result = await this.scheduleQuery.GetPendingSchedules(true);

            Assert.IsNotNull(result, "Job application schedule to be not null.");

            this.mockDocumentClient
                .Verify(
                    c => c.Get(
                        It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null),
                    Times.Once,
                    "Expected to get single job application once.");
            this.mockDocumentClient
                .Verify(
                    c => c.GetFirstOrDefault(
                    It.IsAny<Expression<Func<ScheduleRemind, bool>>>(), null),
                    Times.Never,
                    "Expected to get single job application once.");
        }
    }
}
