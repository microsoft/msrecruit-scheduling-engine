namespace HR.TA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Provisioning.Entities.XrmEntities.Attract;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.EnumSetModel;
    using HR.TA.Talent.FalconEntities.Attract;
    using HR.TA.Talent.FalconEntities.Attract.Conference;
    using HR.TA.Talent.TalentContracts.ScheduleService;
    using Common = HR.TA.Common;

    [TestClass]
    public class UpdateScheduleDataTest
    {
        private IScheduleQuery scheduleQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<ILogger<ScheduleQuery>> loggerMock;

        private Mock<Common.DocumentDB.IHcmDocumentClient> mockDocumentClient;

        [TestInitialize]
        public void BeforEach()
        {
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.loggerMock = new Mock<ILogger<ScheduleQuery>>();
            this.mockDocumentClient = new Mock<Common.DocumentDB.IHcmDocumentClient>();
            this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);
            this.scheduleQuery = new ScheduleQuery(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// UpdateScheduleStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleStatusTestWithNullInputs()
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
                   var exception = this.scheduleQuery.UpdateScheduleStatus(string.Empty, TA.Talent.EnumSetModel.ScheduleStatus.NotScheduled).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// UpdateScheduleStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleStatusTestWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = null;
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

                   var result = this.scheduleQuery.UpdateScheduleStatus("12345", ScheduleStatus.Queued);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleStatusTestWithValidData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = TA.Talent.EnumSetModel.ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;

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

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var result = this.scheduleQuery.UpdateScheduleStatus("12345", TA.Talent.EnumSetModel.ScheduleStatus.Queued);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleStatusTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleStatusTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = ScheduleStatus.Queued;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;

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

                   var result = this.scheduleQuery.UpdateScheduleStatus("12345", ScheduleStatus.Queued);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleServiceAccountTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleServiceAccountTestWithNullInputs()
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
                   var exception = this.scheduleQuery.UpdateScheduleServiceAccount(string.Empty, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateScheduleServiceAccount
        /// </summary>
        [TestMethod]
        public void UpdateScheduleServiceAccountTestWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = null;
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

                   var result = this.scheduleQuery.UpdateScheduleServiceAccount("12345", "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleServiceAccountTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleServiceAccountTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";

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

                   var result = this.scheduleQuery.UpdateScheduleServiceAccount(jobApplication.ScheduleID, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleServiceAccountTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleServiceAccountTestWithData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";

            Worker worker = new Worker();
            worker.OfficeGraphIdentifier = Guid.NewGuid().ToString();
            jobApplication.ScheduleRequester = worker;

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

                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<Worker, bool>>>(), null))
                   .Returns(Task.FromResult(new Worker()));

                   var result = this.scheduleQuery.UpdateScheduleServiceAccount(jobApplication.ScheduleID, "Test1@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateJobApplicationScheduleDetailsTest
        /// </summary>
        [TestMethod]
        public void UpdateJobApplicationScheduleDetailsTestWithNullInputs()
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
                   var exception = this.scheduleQuery.UpdateJobApplicationScheduleDetails(null, ScheduleStatus.Queued, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateJobApplicationScheduleDetailsTest
        /// </summary>
        [TestMethod]
        public void UpdateJobApplicationScheduleDetailsTestWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = null;
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

                   var scheduleIdEventIdMap = new Dictionary<string, CalendarEvent>();
                   scheduleIdEventIdMap.Add("12345", null);
                   var result = this.scheduleQuery.UpdateJobApplicationScheduleDetails(scheduleIdEventIdMap, ScheduleStatus.Queued, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateJobApplicationScheduleDetailsTest
        /// </summary>
        [TestMethod]
        public void UpdateJobApplicationScheduleDetailsTestWithValidInputs()
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
            jobApplication.Participants = new List<Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant>()
                    {
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "Test",
                            ParticipantStatus =  TalentEntities.Enum.InvitationResponseStatus.None,
                        },
                        new Common.Provisioning.Entities.FalconEntities.Attract.JobApplicationScheduleParticipant()
                        {
                            ParticipantMetadata = "123456",
                        }
                    };

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
                   .Returns(Task.FromResult(jobApplication));

                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<JobApplicationSchedule, bool>>>(), null))
                 .Returns(Task.FromResult<IEnumerable<JobApplicationSchedule>>(schedules));

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var scheduleIdEventIdMap = new Dictionary<string, CalendarEvent>();
                   scheduleIdEventIdMap.Add("12345", null);
                   var result = this.scheduleQuery.UpdateJobApplicationScheduleDetails(scheduleIdEventIdMap, ScheduleStatus.Queued, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleWithResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithResponseTestWithoutEvent()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            Message message = new Message()
            {
                Body = new CalendarBody(),
                Sender = new Organizer()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "Test",
                    },
                },
                MeetingMessageType = "meetingDeclined",
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
                   var result = this.scheduleQuery.UpdateScheduleWithResponse(message, It.IsAny<Microsoft.Graph.User>(), It.IsAny<Microsoft.Graph.User>()).Exception;

                   Assert.IsInstanceOfType(result.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateScheduleWithResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithResponseTestWithoutSender()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            Message message = new Message()
            {
                Body = new CalendarBody(),
                Event = new CalendarEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                },
                MeetingMessageType = "meetingDeclined",
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
                   var result = this.scheduleQuery.UpdateScheduleWithResponse(message, It.IsAny<Microsoft.Graph.User>(), It.IsAny<Microsoft.Graph.User>()).Exception;

                   Assert.IsInstanceOfType(result.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateScheduleWithResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithResponseTestWithoutSchedule()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            Message message = new Message()
            {
                Body = new TA.ScheduleService.Contracts.V1.CalendarBody(),
                Event = new CalendarEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                },
                Sender = new Organizer()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "Test",
                    },
                },
                MeetingMessageType = "meetingDeclined",
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

                   var result = this.scheduleQuery.UpdateScheduleWithResponse(message, null, null).Exception;

                   Assert.IsInstanceOfType(result.InnerException, typeof(OperationFailedException));
               });
        }

        /// <summary>
        /// UpdateScheduleWithResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithResponseTestWithDeclined()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            Message message = new Message()
            {
                Body = new CalendarBody(),
                Event = new CalendarEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                },
                Sender = new Organizer()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "Test",
                    },
                },
                MeetingMessageType = "meetingDeclined",
            };

            Microsoft.Graph.User sender = new Microsoft.Graph.User();
            sender.Mail = "Test";

            JobApplicationSchedule jobApplication = new JobApplicationSchedule()
            {
                ScheduleID = Guid.NewGuid().ToString(),
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

                   var result = this.scheduleQuery.UpdateScheduleWithResponse(message, sender, null);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleWithResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithResponseTestWithAccepted()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            Message message = new Message()
            {
                Body = new CalendarBody(),
                Event = new CalendarEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                },
                Sender = new Organizer()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "Test",
                    },
                },
                MeetingMessageType = "meetingAccepted",
                SentDateTime = DateTimeOffset.Now,
                BodyPreview = "Test",
            };

            Microsoft.Graph.User sender = new Microsoft.Graph.User();
            sender.Mail = "Test";

            JobApplicationSchedule jobApplication = new JobApplicationSchedule()
            {
                ScheduleID = Guid.NewGuid().ToString(),
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

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var result = this.scheduleQuery.UpdateScheduleWithResponse(message, null, sender);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleWithResponseTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithResponseTestWithTentativelyAccepted()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            Message message = new Message()
            {
                Body = new CalendarBody(),
                Event = new CalendarEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                },
                Sender = new Organizer()
                {
                    EmailAddress = new MeetingAttendeeEmailAddress()
                    {
                        Address = "Test",
                    },
                },
                MeetingMessageType = "meetingTenativelyAccepted",
            };

            Microsoft.Graph.User sender = new Microsoft.Graph.User();
            sender.Mail = "Test";

            JobApplicationSchedule jobApplication = new JobApplicationSchedule()
            {
                ScheduleID = Guid.NewGuid().ToString(),
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

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var result = this.scheduleQuery.UpdateScheduleWithResponse(message, sender, null);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithNullInputs()
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
                   var exception = this.scheduleQuery.UpdateScheduleDetails(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = null;
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

                   var result = this.scheduleQuery.UpdateScheduleDetails(new MeetingInfo());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = TA.Talent.EnumSetModel.ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = ScheduleStatus.Saved;
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

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var result = this.scheduleQuery.UpdateScheduleDetails(meetingInfo);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidStatus()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = TA.Talent.EnumSetModel.ScheduleStatus.Saved;
            jobApplication.ServiceAccountEmail = "Test@microsoft.com";
            jobApplication.Description = "test";
            jobApplication.StartDateTime = DateTime.UtcNow;
            jobApplication.EndDateTime = DateTime.Today;

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = ScheduleStatus.Queued;
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

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var result = this.scheduleQuery.UpdateScheduleDetails(meetingInfo);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidResponseData()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
            jobApplication.ScheduleID = Guid.NewGuid().ToString();
            jobApplication.ScheduleStatus = TA.Talent.EnumSetModel.ScheduleStatus.Queued;
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
                            InvitationResponseStatus = TalentEntities.Enum.InvitationResponseStatus.Accepted,
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
                    Attendees = new List<Attendee>()
                {
                    new Attendee()
                    {
                        User = new GraphPerson()
                        {
                            Name = "Test",
                            GivenName = "Test",
                            UserPrincipalName = "test",
                            Email = "test@microsoft.com",
                            MobilePhone = "3242423",
                            Id = Guid.NewGuid().ToString(),
                            InvitationResponseStatus = TalentEntities.Enum.InvitationResponseStatus.None
                        },
                        ResponseStatus = TalentEntities.Enum.InvitationResponseStatus.None,
                        ResponseComment = "tesf"
                    }
                },
                },
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

                   this.mockDocumentClient.Setup(a => a.Update<JobApplicationSchedule>(It.IsAny<JobApplicationSchedule>(), null));

                   var result = this.scheduleQuery.UpdateScheduleDetails(meetingInfo);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateJobApplicationStatusHistoryAsyncTest
        /// </summary>
        [TestMethod]
        public void UpdateJobApplicationStatusHistoryAsyncTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = null;
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

                   var result = this.scheduleQuery.UpdateJobApplicationStatusHistoryAsync("123456", JobApplicationActionType.SendToCandidate);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// Update schedule with sharing status async test
        /// </summary>
        [TestMethod]
        public void UpdateScheduleWithSharingStatusAsyncTest()
        {
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
                            InvitationResponseStatus = TalentEntities.Enum.InvitationResponseStatus.Accepted,
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
                    IsInterviewerNameShared = false,
                    IsInterviewScheduleShared = false,
                    Attendees = new List<Attendee>()
                {
                    new Attendee()
                    {
                        User = new GraphPerson()
                        {
                            Name = "Test",
                            GivenName = "Test",
                            UserPrincipalName = "test",
                            Email = "test@microsoft.com",
                            MobilePhone = "3242423",
                            Id = Guid.NewGuid().ToString(),
                            InvitationResponseStatus = TalentEntities.Enum.InvitationResponseStatus.None
                        },
                        ResponseStatus = TalentEntities.Enum.InvitationResponseStatus.None,
                        ResponseComment = "tesf"
                    }
                },
                },
            };

            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();
            JobApplicationSchedule jobApplication = new JobApplicationSchedule();
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

                   var result = this.scheduleQuery.UpdateScheduleWithSharingStatusAsync(new CandidateScheduleCommunication());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
