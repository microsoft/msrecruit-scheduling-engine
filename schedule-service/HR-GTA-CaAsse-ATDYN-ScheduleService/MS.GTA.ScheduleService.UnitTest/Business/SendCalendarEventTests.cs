//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using CommonLibrary.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.TalentEntities.Enum;

    [TestClass]
    public class SendCalendarEventTests
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

        private Mock<IInternalsProvider> internalsProvider;

        private Mock<IUserDetailsProvider> userDetailsProviderMock;

        private Mock<IHCMServiceContext> contextMock;

        private Mock<IConfigurationManager> configMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.contextMock = new Mock<IHCMServiceContext>();
            this.serviceBusHelperMock = new Mock<IServiceBusHelper>();
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.internalsProvider = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithEmptyInputs()
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

                   var exception = scheduleManager.SendCalendarEvent(null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithNullInputs()
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

                   var exception = scheduleManager.SendCalendarEvent(null, "Test").Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithValidInputs()
        {
            string userId = Guid.NewGuid().ToString();
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
                            Id = userId,
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
                                Email = "test@microsoft.com",
                                Id = userId,
                            }
                        }
                    }
                },
            };

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>();
            meetingInfos.Add(meetingInfo);

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Id = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.SendPostEvent(It.IsAny<string>(), It.IsAny<CalendarEvent>())).Returns(Task.FromResult(calendarEvent));

                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleIds(It.IsAny<List<string>>())).Returns(Task.FromResult(meetingInfos));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.SendCalendarEvent(new List<string>() { Guid.NewGuid().ToString() }, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithDeleteStatusInput()
        {
            string userId = Guid.NewGuid().ToString();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = GTA.Talent.EnumSetModel.ScheduleStatus.Delete;
            meetingInfo.UserGroups = new UserGroup()
            {
                FreeBusyTimeId = Guid.NewGuid().ToString(),
                Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = userId,
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
                                Email = "test@microsoft.com",
                                Id = userId,
                            }
                        }
                    }
                },
            };

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>();
            meetingInfos.Add(meetingInfo);

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Id = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.SendPostEvent(It.IsAny<string>(), It.IsAny<CalendarEvent>())).Returns(Task.FromResult(calendarEvent));

                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleIds(It.IsAny<List<string>>())).Returns(Task.FromResult(meetingInfos));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.SendCalendarEvent(new List<string>() { Guid.NewGuid().ToString() }, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithSentStatusInput()
        {
            string userId = Guid.NewGuid().ToString();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = GTA.Talent.EnumSetModel.ScheduleStatus.Sent;
            meetingInfo.UserGroups = new UserGroup()
            {
                FreeBusyTimeId = Guid.NewGuid().ToString(),
                Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = userId,
                        }
                    },
            };
            meetingInfo.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
            {
                new MeetingDetails()
                {
                    ////CalendarEventId = Guid.NewGuid().ToString(),
                    MeetingLocation = new InterviewMeetingLocation()
                    {
                        Room = new Room()
                        {
                            Name = "test",
                            Address = "test",
                        },
                        RoomList = new Room()
                        {
                            Name = "test",
                            Address = "test",
                        }
                    },
                    Subject = "Test",
                    UtcStart = DateTime.Now,
                    UtcEnd = DateTime.UtcNow,
                    Attendees = new List<Attendee>()
                    {
                        new Attendee()
                        {
                            User = new GraphPerson()
                            {
                                Email = "test@microsoft.com",
                                Id = userId,
                            }
                        }
                    }
                }
            };

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>();
            meetingInfos.Add(meetingInfo);

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Id = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.SendPostEvent(It.IsAny<string>(), It.IsAny<CalendarEvent>())).Returns(Task.FromResult(calendarEvent));

                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleIds(It.IsAny<List<string>>())).Returns(Task.FromResult(meetingInfos));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.SendCalendarEvent(new List<string>() { Guid.NewGuid().ToString() }, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }



        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithSavedStatusInput()
        {
            string userId = Guid.NewGuid().ToString();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = GTA.Talent.EnumSetModel.ScheduleStatus.Saved;
            meetingInfo.UserGroups = new UserGroup()
            {
                FreeBusyTimeId = Guid.NewGuid().ToString(),
                Users = new System.Collections.Generic.List<GraphPerson>()
                    {
                        new GraphPerson()
                        {
                            Name = "Test",
                            Email = "test@microsoft.com",
                            Id = userId,
                        }
                    },
            };
            meetingInfo.MeetingDetails = new System.Collections.Generic.List<MeetingDetails>()
            {
                new MeetingDetails()
                {
                    ////CalendarEventId = Guid.NewGuid().ToString(),
                    MeetingLocation = new InterviewMeetingLocation()
                    {
                        Room = new Room()
                        {
                            Name = "test",
                            Address = "test",
                        },
                        RoomList = new Room()
                        {
                            Name = "test",
                            Address = "test",
                        }
                    },
                    Subject = "Test",
                    UtcStart = DateTime.Now,
                    UtcEnd = DateTime.UtcNow,
                    Attendees = new List<Attendee>()
                    {
                        new Attendee()
                        {
                            User = new GraphPerson()
                            {
                                Email = "test@microsoft.com",
                                Id = userId,
                            }
                        }
                    }
                }
            };

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>();
            meetingInfos.Add(meetingInfo);

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Id = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.SendPostEvent(It.IsAny<string>(), It.IsAny<CalendarEvent>())).Returns(Task.FromResult(calendarEvent));

                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleIds(It.IsAny<List<string>>())).Returns(Task.FromResult(meetingInfos));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.SendCalendarEvent(new List<string>() { Guid.NewGuid().ToString() }, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// SendCalendarEventTest
        /// </summary>
        [TestMethod]
        public void SendCalendarEventTestWithMeetingLocation()
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
                    MeetingLocation = new InterviewMeetingLocation()
                    {
                        Room = new Room()
                        {
                            Address = "Test",
                            Name = "Test",
                            Status = InvitationResponseStatus.TentativelyAccepted,
                        },
                        RoomList = new Room()
                        {
                            Address = "Test",
                            Name = "Test",
                            Status = InvitationResponseStatus.TentativelyAccepted,
                        },
                    },
                },
            };

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Id = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.SendPostEvent(It.IsAny<string>(), It.IsAny<CalendarEvent>())).Returns(Task.FromResult(calendarEvent));

                   this.scheduleQueryMock.Setup(a => a.GetScheduleByScheduleId(It.IsAny<string>())).Returns(Task.FromResult(meetingInfo));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.SendCalendarEvent(new List<string>() { Guid.NewGuid().ToString() }, "Test@microsoft.com");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProvider.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
