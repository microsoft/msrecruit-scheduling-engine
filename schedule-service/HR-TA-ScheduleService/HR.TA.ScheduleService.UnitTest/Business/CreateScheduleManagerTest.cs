//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class CreateScheduleManagerTest
    {
        private Mock<IScheduleQuery> scheduleQueryMock;

        private FalconQuery falconQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<FalconQuery>> falconLoggerMock;

        private Mock<IServiceBusHelper> serviceBusHelperMock;

        private Mock<IGraphSubscriptionManager> graphSubscriptionManagerMock;

        private Mock<IEmailClient> emailClientMock;

        private Mock<IEmailHelper> emailHelperMock;

        private Mock<IOutlookProvider> outlookProviderMock;

        private Mock<INotificationClient> notificationClientMock;

        private Mock<ILogger<ScheduleManager>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IInternalsProvider> internalsProvider;

        private Mock<IUserDetailsProvider> userDetailsProviderMock;

        private Mock<IHCMServiceContext> contextMock;

        private Mock<IConfigurationManager> configMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.contextMock = new Mock<IHCMServiceContext>();
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
            this.internalsProvider = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();

                   var exception = scheduleManager.CreateSchedule(null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleTestWithInValidInputs()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.CreateSchedule(meetingInfo, "12345");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// CreateScheduleTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleTestWithValidInputs()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();
            string jobId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();
            JobApplication jobApplication = new JobApplication()
            {
                JobApplicationID = jobId,
                JobApplicationParticipants = new List<JobApplicationParticipant>()
                {
                    new JobApplicationParticipant()
                    {
                        Role = TalentEntities.Enum.JobParticipantRole.Contributor,
                        OID = userId
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
                   var scheduleManager = this.GetScheduleManagerInstance();

                   this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));

                   var result = scheduleManager.CreateSchedule(meetingInfo, jobId);

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
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var scheduleManager = this.GetScheduleManagerInstance();
                   var exception = scheduleManager.UpdateSchedule(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidInputs()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
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
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));
                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.UpdateSchedule(meetingInfo);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidTimeInput()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
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
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   MeetingInfo meeting = new MeetingInfo();
                   meeting.Id = meetingInfo.Id;
                   meeting.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
                    {
                        new MeetingDetails()
                        {
                            CalendarEventId = Guid.NewGuid().ToString(),
                            Subject = "Test",
                            UtcStart = DateTime.Now,
                            UtcEnd = DateTime.UtcNow,
                        },
                    };

                   var result = scheduleManager.UpdateSchedule(meeting);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidParticipantsInput()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
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
                            Name = "Test4",
                            Email = "test4@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
            };
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   MeetingInfo meeting = new MeetingInfo();
                   meeting.Id = meetingInfo.Id;
                   meeting.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
                    {
                        new MeetingDetails()
                        {
                            CalendarEventId = Guid.NewGuid().ToString(),
                            Subject = "Test",
                            UtcStart = meetingInfo.MeetingDetails[0].UtcStart,
                            UtcEnd = meetingInfo.MeetingDetails[0].UtcEnd
                        },
                    };
                   meeting.UserGroups = new UserGroup()
                   {
                       FreeBusyTimeId = Guid.NewGuid().ToString(),
                       Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test3@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
                   };
                   var result = scheduleManager.UpdateSchedule(meeting);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithValidParticipants2Input()
        {
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
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
                            Name = "Test4",
                            Email = "test4@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
            };
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   MeetingInfo meeting = new MeetingInfo();
                   meeting.Id = meetingInfo.Id;
                   meeting.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
                    {
                        new MeetingDetails()
                        {
                            CalendarEventId = Guid.NewGuid().ToString(),
                            Subject = "Test",
                            UtcStart = meetingInfo.MeetingDetails[0].UtcStart,
                            UtcEnd = meetingInfo.MeetingDetails[0].UtcEnd
                        },
                    };
                   meeting.UserGroups = new UserGroup()
                   {
                       FreeBusyTimeId = Guid.NewGuid().ToString(),
                       Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test4@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        },
                        new GraphPerson()
                        {
                            Name = "Test4",
                            Email = "test3@microsoft.com",
                            Id = Guid.NewGuid().ToString(),
                        }
                    },
                   };
                   var result = scheduleManager.UpdateSchedule(meeting);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// UpdateScheduleTest
        /// </summary>
        [TestMethod]
        public void UpdateScheduleTestWithInValidInputs()
        {
            MeetingInfo result = null;

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).Returns(Task.FromResult(result));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var exception = scheduleManager.UpdateSchedule(meetingInfo).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProvider.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
