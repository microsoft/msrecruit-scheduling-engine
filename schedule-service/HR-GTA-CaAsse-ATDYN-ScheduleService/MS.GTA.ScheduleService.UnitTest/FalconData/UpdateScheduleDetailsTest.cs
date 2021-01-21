namespace MS.GTA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using Common = MS.GTA.Common;

    [TestClass]
    public class UpdateScheduleDetailsTest
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
            this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);
            this.loggerMock = new Mock<ILogger<ScheduleQuery>>();
            this.scheduleQuery = new ScheduleQuery(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// UpdateScheduleEmailStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleEmailStatusTestWithNullInputs()
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
                   var exception = this.scheduleQuery.UpdateScheduleEmailStatus(string.Empty, ScheduleStatus.Queued, null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// UpdateScheduleEmailStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleEmailStatusTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.UserGroups = new UserGroup()
            {
                FreeBusyTimeId = Guid.NewGuid().ToString(),
                Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        },
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
            };
            meetingInfo.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
            {
                new MeetingDetails()
                {
                    CalendarEventId = Guid.NewGuid().ToString(),
                    Subject = "Test",
                    UtcStart = DateTime.Now,
                    UtcEnd = DateTime.UtcNow,
                },
            };

            string jobApplicatioNid = Guid.NewGuid().ToString();

            JobApplicationSchedule jobApplication = new JobApplicationSchedule()
            {
                ScheduleID = meetingInfo.Id,
                JobApplicationID = jobApplicatioNid,
                ScheduleStatus = ScheduleStatus.Saved,
                Participants = new List<Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant>()
                    {
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "Test",
                        },
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "123456",
                        }
                    }
            };

            JobApplicationScheduleMailDetails applicationScheduleMailDetails = new JobApplicationScheduleMailDetails()
            {
                ScheduleID = meetingInfo.Id
            };

            JobApplication jobApplication1 = new JobApplication()
            {
                Id = jobApplicatioNid,
                JobApplicationActivities = new List<JobApplicationActivity>()
                {
                    new JobApplicationActivity()
                    {
                        ActivityType = TalentEntities.Enum.JobApplicationActivityType.IVRequested,
                    }
                }
            };

            EmailTemplate template = new EmailTemplate()
            {
                TemplateName = "Test",
                Subject = "TestSubject",
                EmailContent = "TestContent",
            };

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
                    .Returns(Task.FromResult(jobApplication));
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplicationScheduleMailDetails, bool>>>(), null))
                    .Returns(Task.FromResult(applicationScheduleMailDetails));

                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplication, bool>>>(), null))
                    .Returns(Task.FromResult(jobApplication1));

                   var result = this.scheduleQuery.UpdateScheduleEmailStatus(meetingInfo.Id, ScheduleStatus.Queued, template);

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// UpdateScheduleEmailStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleEmailStatusTestWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.UserGroups = new UserGroup()
            {
                FreeBusyTimeId = Guid.NewGuid().ToString(),
                Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        },
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
            };
            meetingInfo.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
            {
                new MeetingDetails()
                {
                    CalendarEventId = Guid.NewGuid().ToString(),
                    Subject = "Test",
                    UtcStart = DateTime.Now,
                    UtcEnd = DateTime.UtcNow,
                },
            };

            EmailTemplate template = new EmailTemplate()
            {
                TemplateName = "Test",
                Subject = "TestSubject",
                EmailContent = "TestContent",
            };

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

                   var result = this.scheduleQuery.UpdateScheduleEmailStatus(meetingInfo.Id, ScheduleStatus.Queued, template);

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// UpdateScheduleEmailStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleEmailStatusTestWithValidData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = GTA.Talent.EnumSetModel.ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.UserGroups = new UserGroup()
            {
                FreeBusyTimeId = Guid.NewGuid().ToString(),
                Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        },
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
            };
            meetingInfo.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
            {
                new MeetingDetails()
                {
                    CalendarEventId = Guid.NewGuid().ToString(),
                    Subject = "Test",
                    UtcStart = DateTime.Now,
                    UtcEnd = DateTime.UtcNow,
                },
            };

            EmailTemplate template = new EmailTemplate()
            {
                TemplateName = "Test",
                Subject = "TestSubject",
                EmailContent = "TestContent",
            };

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
                    .Returns(Task.FromResult(jobApplication));
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobApplication, bool>>>(), null))
                    .Returns(Task.FromResult(new JobApplication()));

                   var result = this.scheduleQuery.UpdateScheduleEmailStatus(meetingInfo.Id, ScheduleStatus.Queued, template);

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// UpdateScheduleWithCalendatEventResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithCalendatEventResponseTestWithNullInputs()
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
                   var exception = this.scheduleQuery.UpdateScheduleWithCalendatEventResponse(null, null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateScheduleWithCalendatEventResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithCalendatEventResponseTestWithInvalidData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            CalendarEvent calendarEvent = new CalendarEvent();
            calendarEvent.Id = Guid.NewGuid().ToString();
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

                   var exception = this.scheduleQuery.UpdateScheduleWithCalendatEventResponse(calendarEvent, null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateScheduleWithCalendatEventResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithCalendatEventResponseTestWithValidData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            CalendarEvent calendarEvent = new CalendarEvent();
            calendarEvent.Id = Guid.NewGuid().ToString();
            calendarEvent.Attendees = new List<MeetingAttendee>()
            {
                new MeetingAttendee()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "abTest",
                    },
                    Status = new MeetingAttendeeStatus()
                    {
                        Response = "accepted",
                    }
                },
                new MeetingAttendee()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "123456",
                    },
                    Status = new MeetingAttendeeStatus()
                    {
                        Response = "declined",
                    }
                },
                new MeetingAttendee()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "testsdc123456",
                    },
                    Status = new MeetingAttendeeStatus()
                    {
                        Response = "tentativelyaccepted",
                    }
                },
            };

            JobApplicationSchedule jobApplication = new JobApplicationSchedule()
            {
                ScheduleID = Guid.NewGuid().ToString(),
                CalendarEventId = calendarEvent.Id,
                JobApplicationID = Guid.NewGuid().ToString(),
                ScheduleStatus = ScheduleStatus.Saved,
                Participants = new List<Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant>()
                    {
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "abTest",
                        },
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "123456",
                        },
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "testsdc123456",
                        }
                    }
            };

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
                    .Returns(Task.FromResult(jobApplication));

                   var result = this.scheduleQuery.UpdateScheduleWithCalendatEventResponse(calendarEvent, null);

                   Assert.IsNotNull(result);
               });
        }
    }
}
