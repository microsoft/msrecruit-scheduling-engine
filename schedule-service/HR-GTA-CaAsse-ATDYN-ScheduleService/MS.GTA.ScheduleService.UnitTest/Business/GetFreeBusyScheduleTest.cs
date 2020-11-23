namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class GetFreeBusyScheduleTest
    {
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

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IInternalsProvider> internalsProviderMock;

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
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithNullInputs()
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
                   var exception = scheduleManager.GetFreeBusySchedule(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithValidInputs()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow;

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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>()));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.GetFreeBusySchedule(meetingInfo);

                   Assert.IsNotNull(result.Result);
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithInValidData()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow;

            FindFreeBusyScheduleResponse response = new FindFreeBusyScheduleResponse()
            {
                AvailabilityView = "Test",
                ScheduleId = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>() { response}));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var exception = scheduleManager.GetFreeBusySchedule(meetingInfo).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(SchedulerProcessingException));
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithValidData()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1));

            FindFreeBusyScheduleResponse response = new FindFreeBusyScheduleResponse()
            {
                AvailabilityView = "Test",
                ScheduleId = "test@microsoft.com",
                WorkingHours = new WorkingHours()
                {
                    StartTime = new Microsoft.Graph.TimeOfDay(1, 1, 10),
                    EndTime = new Microsoft.Graph.TimeOfDay(2, 2, 20),
                    TimeZone = new Microsoft.Graph.TimeZoneBase()
                    {
                        Name = TimeZoneInfo.Local.StandardName
                    },
                    DaysOfWeek = new List<string>()
                    {
                        "wednesday",
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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>() { response }));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.GetFreeBusySchedule(meetingInfo);

                   Assert.IsNotNull(result.Result);
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithInValidDay()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1));

            FindFreeBusyScheduleResponse response = new FindFreeBusyScheduleResponse()
            {
                AvailabilityView = "Test",
                ScheduleId = "test@microsoft.com",
                WorkingHours = new WorkingHours()
                {
                    StartTime = new Microsoft.Graph.TimeOfDay(1, 1, 10),
                    EndTime = new Microsoft.Graph.TimeOfDay(2, 2, 20),
                    TimeZone = new Microsoft.Graph.TimeZoneBase()
                    {
                        Name = TimeZoneInfo.Local.StandardName
                    },
                    DaysOfWeek = new List<string>()
                    {
                        "sunday",
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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>() { response }));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.GetFreeBusySchedule(meetingInfo);

                   Assert.IsNotNull(result.Result);
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithValidDay()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));

            FindFreeBusyScheduleResponse response = new FindFreeBusyScheduleResponse()
            {
                AvailabilityView = "Test",
                ScheduleId = "test@microsoft.com",
                WorkingHours = new WorkingHours()
                {
                    StartTime = new Microsoft.Graph.TimeOfDay(1, 1, 10),
                    EndTime = new Microsoft.Graph.TimeOfDay(2, 2, 20),
                    TimeZone = new Microsoft.Graph.TimeZoneBase()
                    {
                        Name = TimeZoneInfo.Local.StandardName
                    },
                    DaysOfWeek = new List<string>()
                    {
                        "TuesDay",
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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>() { response }));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.GetFreeBusySchedule(meetingInfo);

                   Assert.IsNotNull(result.Result);
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithError()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));

            FindFreeBusyScheduleResponse response = new FindFreeBusyScheduleResponse()
            {
                AvailabilityView = "Test",
                ScheduleId = "test@microsoft.com",
                WorkingHours = new WorkingHours()
                {
                    StartTime = new Microsoft.Graph.TimeOfDay(1, 1, 10),
                    EndTime = new Microsoft.Graph.TimeOfDay(2, 2, 20),
                    TimeZone = new Microsoft.Graph.TimeZoneBase()
                    {
                        Name = TimeZoneInfo.Local.StandardName
                    },
                    DaysOfWeek = new List<string>()
                    {
                        "TuesDay",
                    }
                },
                Error = new FreeBusyError()
                {
                    ResponseCode = "ErrorNoFreeBusyAccess",
                    Message = "ErrorNoFreeBusyAccess"
                }
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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>() { response }));

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.GetFreeBusySchedule(meetingInfo);

                   Assert.IsNotNull(result.Result);
               });
        }

        /// <summary>
        /// GetFreeBusyScheduleTest
        /// </summary>
        [TestMethod]
        public void GetFreeBusyScheduleTestWithInValidTime()
        {
            FreeBusyRequest meetingInfo = new FreeBusyRequest();
            meetingInfo.UserGroups = new System.Collections.Generic.List<UserGroup>()
            {
                new UserGroup()
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
                },
            };

            meetingInfo.UtcEnd = DateTime.UtcNow;
            meetingInfo.UtcStart = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1));

            FindFreeBusyScheduleResponse response = new FindFreeBusyScheduleResponse()
            {
                AvailabilityView = "Test",
                ScheduleId = "test@microsoft.com",
                WorkingHours = new WorkingHours()
                {
                    StartTime = new Microsoft.Graph.TimeOfDay(1, 1, 10),
                    EndTime = new Microsoft.Graph.TimeOfDay(2, 2, 20),
                    TimeZone = new Microsoft.Graph.TimeZoneBase()
                    {
                        Name = "Test"
                    },
                    DaysOfWeek = new List<string>()
                    {
                        "1",
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
                   this.outlookProviderMock.Setup(a => a.SendPostFindFreeBusySchedule(It.IsAny<FindFreeBusyScheduleRequest>())).Returns(Task.FromResult(new List<FindFreeBusyScheduleResponse>() { response }));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.GetFreeBusySchedule(meetingInfo);

                   Assert.IsNotNull(result.Result);
               });
        }

        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
