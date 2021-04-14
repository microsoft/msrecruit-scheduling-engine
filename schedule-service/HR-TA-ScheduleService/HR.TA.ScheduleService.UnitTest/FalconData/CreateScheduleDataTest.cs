//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.FalconEntities.Attract;
    using Common = HR.TA.Common;

    [TestClass]
    public class CreateScheduleDataTest
    {
        private IScheduleQuery scheduleQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<ScheduleQuery>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<Common.DocumentDB.IHcmDocumentClient> mockDocumentClient;

        [TestInitialize]
        public void BeforEach()
        {
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.mockDocumentClient = new Mock<Common.DocumentDB.IHcmDocumentClient>();
            this.loggerMock = new Mock<ILogger<ScheduleQuery>>();
            this.scheduleQuery = new ScheduleQuery(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.scheduleQuery.CreateSchedule(null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleTestWithExistInput()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            JobApplicationSchedule schedule = new JobApplicationSchedule();
            schedule.ScheduleID = meetingInfo.Id;

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);

                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                   .Returns(Task.FromResult(schedule));

                   this.mockDocumentClient.Setup(a => a.Create(It.IsAny<JobApplicationSchedule>(), null))
                   .Returns(Task.FromResult(schedule));

                   var result = this.scheduleQuery.CreateSchedule(meetingInfo, "12345");
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);

                   this.mockDocumentClient.Setup(a => a.Create(It.IsAny<JobApplicationSchedule>(), null))
                   .Returns(Task.FromResult(new JobApplicationSchedule()));

                   var result = this.scheduleQuery.CreateSchedule(meetingInfo, "12345");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
