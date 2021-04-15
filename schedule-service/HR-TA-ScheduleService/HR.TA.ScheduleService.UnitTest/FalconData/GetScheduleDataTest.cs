//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.Common.DocumentDB;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.EnumSetModel;
    using HR.TA.Talent.FalconEntities.Attract;
    using Common = HR.TA.Common;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.Talent.FalconEntities.Attract.Conference;
    using HR.TA.ScheduleService.Contracts;

    [TestClass]
    public class GetScheduleDataTest
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
        /// GetScheduleByScheduleIdTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdTestWithNullInputs()
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
                   var exception = this.scheduleQuery.GetScheduleByScheduleId(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetScheduleByScheduleId
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdTestWithValidInputs()
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
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                   .Returns(Task.FromResult(new JobApplicationSchedule()));

                   var result = this.scheduleQuery.GetScheduleByScheduleId("12345");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleByScheduleId
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdTestWithData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            JobApplicationSchedule schedule = new JobApplicationSchedule();
            schedule.Id = Guid.NewGuid().ToString();
            schedule.ScheduleStatus = ScheduleStatus.Queued;
            schedule.ServiceAccountEmail = "Test@microsoft.com";
            schedule.Description = "test";
            schedule.StartDateTime = DateTime.UtcNow;
            schedule.EndDateTime = DateTime.Today;

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                   .Returns(Task.FromResult(schedule));

                   var result = this.scheduleQuery.GetScheduleByScheduleId("12345");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetSchedules
        /// </summary>
        [TestMethod]
        public void GetSchedulesIdTestWithNullInputs()
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
                   var exception = this.scheduleQuery.GetSchedules(null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// GetSchedules
        /// </summary>
        [TestMethod]
        public void GetSchedulesTestWithValidInputs()
        {
            FindFreeBusyScheduleRequest meetingInfo = new FindFreeBusyScheduleRequest();
            meetingInfo.EndTime = new MeetingDateTime()
            {
                DateTime = DateTime.UtcNow.ToString()
            };

            meetingInfo.Schedules = new List<string>()
            {
                "Test",
                "12345",
                "123456"
            };

            meetingInfo.StartTime = new MeetingDateTime();

            List<JobApplicationSchedule> jobApplicationSchedules = new List<JobApplicationSchedule>()
            {
                new JobApplicationSchedule()
                {
                    ScheduleID = Guid.NewGuid().ToString(),
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Today,
                    ScheduleStatus = TA.Talent.EnumSetModel.ScheduleStatus.NotScheduled,
                    Participants = new List<Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant>()
                    {
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "Test",
                        },
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "12345",
                        }
                    }
                }
            };

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
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                   .Returns(Task.FromResult<IEnumerable<JobApplicationSchedule>>(jobApplicationSchedules));

                   var result = this.scheduleQuery.GetSchedules(meetingInfo, "123456");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationIdTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithNullInputs()
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
                   var exception = this.scheduleQuery.GetSchedulesByJobApplicationId(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationId
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            List<JobApplicationSchedule> schedules = new List<JobApplicationSchedule>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = "12345";
            jobApplication.ScheduleStatus = ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;
            jobApplication.CalendarEventId = "EvenId";
            schedules.Add(jobApplication);

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                   .Returns(Task.FromResult(new JobApplicationSchedule()));

                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
              .Returns(Task.FromResult<IEnumerable<JobApplicationSchedule>>(schedules));

                   var result = this.scheduleQuery.GetSchedulesByJobApplicationId("12345");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleByScheduleIdsTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdsTestWithNullInputs()
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
                   var exception = this.scheduleQuery.GetScheduleByScheduleIds(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetScheduleByScheduleIdsTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdsTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            List<string> ids = new List<string>()
            {
                "1234567897",
                "465765464654"
            };

            List<JobApplicationSchedule> schedules = new List<JobApplicationSchedule>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = "12345";
            jobApplication.ScheduleStatus = ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;
            jobApplication.CalendarEventId = "EvenId";
            schedules.Add(jobApplication);

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                   .Returns(Task.FromResult<IEnumerable<JobApplicationSchedule>>(schedules));

                   var result = this.scheduleQuery.GetScheduleByScheduleIds(ids);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleMailDetailsTest
        /// </summary>
        [TestMethod]
        public void GetScheduleMailDetailsTestWithNullInputs()
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
                   var exception = this.scheduleQuery.GetScheduleMailDetails(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetScheduleMailDetailsTest
        /// </summary>
        [TestMethod]
        public void GetScheduleMailDetailsTestWithInputs()
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
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplicationScheduleMailDetails, bool>>>(), null))
                    .Returns(Task.FromResult(new JobApplicationScheduleMailDetails()));

                   var result = this.scheduleQuery.GetScheduleMailDetails("123456");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetJobOpeningPositionTitleTest
        /// </summary>
        [TestMethod]
        public void GetJobOpeningPositionTitleTestWithNullInputs()
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
                   var exception = this.scheduleQuery.GetJobOpeningPositionTitle(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetJobOpeningPositionTitleTest
        /// </summary>
        [TestMethod]
        public void GetJobOpeningPositionTitleTestWithInputs()
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
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobOpening, bool>>>(), null))
                    .Returns(Task.FromResult(new JobOpening()));

                   var result = this.scheduleQuery.GetJobOpeningPositionTitle("123465");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
