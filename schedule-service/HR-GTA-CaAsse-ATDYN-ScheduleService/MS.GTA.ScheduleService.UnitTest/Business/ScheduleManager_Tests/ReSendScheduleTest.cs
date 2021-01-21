namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
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
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class ReSendScheduleTest
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
            this.serviceBusHelperMock = new Mock<IServiceBusHelper>();
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.internalsProvider = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.contextMock = new Mock<IHCMServiceContext>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// ReSendScheduleTest
        /// </summary>
        [TestMethod]
        public void ReSendScheduleTestWithNullInputs()
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
                   var exception = scheduleManager.ReSendSchedule(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// ReSendScheduleTest
        /// </summary>
        [TestMethod]
        public void ReSendScheduleTestWithInValidInputs()
        {
            string scheduleId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = scheduleId;
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
                   this.scheduleQueryMock.Setup(sq => sq.GetScheduleByScheduleId(It.IsAny<string>())).ReturnsAsync(meetingInfo);

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.ReSendSchedule(scheduleId);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// ReSendScheduleTest
        /// </summary>
        [TestMethod]
        public void ReSendScheduleTestWithValidInputs()
        {
            string scheduleId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = scheduleId;
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
                    SchedulerAccountEmail = "test@microsoft.com",
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
                   this.scheduleQueryMock.Setup(sq => sq.GetScheduleByScheduleId(It.IsAny<string>())).ReturnsAsync(meetingInfo);

                   var scheduleManager = this.GetScheduleManagerInstance();
                   var result = scheduleManager.ReSendSchedule(scheduleId);

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
