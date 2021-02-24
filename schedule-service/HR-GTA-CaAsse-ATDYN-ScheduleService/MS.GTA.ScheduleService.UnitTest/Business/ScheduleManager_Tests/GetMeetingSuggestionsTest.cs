//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.ScheduleService.UnitTest.Business.ScheduleManager_Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
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
    using MS.GTA.Talent.TalentContracts.ScheduleService;

    [TestClass]
    public class GetMeetingSuggestionsTest
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

        private Mock<IScheduleManager> scheduleManagerMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.contextMock = new Mock<IHCMServiceContext>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.serviceBusHelperMock = new Mock<IServiceBusHelper>();
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.internalsProvider = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.configMock = new Mock<IConfigurationManager>();
            this.scheduleManagerMock = new Mock<IScheduleManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// GetMeetingSuggestions With Invalid Inputs
        /// </summary>
        [TestMethod]
        public void GetMeetingSuggestionsWithInvalidInputs()
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

                   var exception = scheduleManager.GetMeetingSuggestions(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetMeetingSuggestions With Valid Inputs
        /// </summary>
        [TestMethod]
        public void GetMeetingSuggestionsWithValidInputs()
        {
            var freeBusyTimeIdInterviewer1 = Guid.NewGuid().ToString();
            var freeBusyTimeIdInterviewer2 = Guid.NewGuid().ToString();
            var freeBusyTimeIdInterviewer3 = Guid.NewGuid().ToString();
            var freeBusyTimeIdPanel = Guid.NewGuid().ToString();
            var interviewer1 = new GraphPerson
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Interviewer1",
                Email = "interviewer1@microsoft.com",
                UserPrincipalName = "interviewer1",
            };
            var interviewer2 = new GraphPerson
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Interviewer2",
                Email = "interviewer2@microsoft.com",
                UserPrincipalName = "interviewer2",
            };
            var interviewer3 = new GraphPerson
            {
                // empty slot
            };
            var candidate = new GraphPerson
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Candidate",
                Email = "candidate@microsoft.com",
                UserPrincipalName = "candidate",
            };

            var suggestMeetingsRequest = new SuggestMeetingsRequest
            {
                InterviewStartDateSuggestion = DateTime.Now,
                InterviewEndDateSuggestion = DateTime.Now.AddDays(1),
                Timezone = null,
                PanelInterview = true,
                PrivateMeeting = false,
                TeamsMeeting = false,
                MeetingDurationInMinutes = "60",
                InterviewersList = new List<UserGroup>
                {
                    new UserGroup
                    {
                        FreeBusyTimeId = freeBusyTimeIdInterviewer1,
                        Users = new List<GraphPerson> { interviewer1 },
                    },
                    new UserGroup
                    {
                        FreeBusyTimeId = freeBusyTimeIdInterviewer2,
                        Users = new List<GraphPerson> { interviewer2 },
                    },
                    new UserGroup
                    {
                        FreeBusyTimeId = freeBusyTimeIdInterviewer3,
                        Users = new List<GraphPerson> { interviewer3 },
                    }
                },
                Candidate = candidate,
            };

            var suggestMeetingsRequestPanel = new SuggestMeetingsRequest
            {
                InterviewStartDateSuggestion = DateTime.Now,
                InterviewEndDateSuggestion = DateTime.Now.AddDays(1),
                Timezone = null,
                PanelInterview = true,
                PrivateMeeting = false,
                TeamsMeeting = false,
                MeetingDurationInMinutes = "60",
                InterviewersList = new List<UserGroup>
                {
                    new UserGroup
                    {
                        FreeBusyTimeId = freeBusyTimeIdPanel,
                        Users = new List<GraphPerson> { interviewer1, interviewer2 },
                    },
                },
                Candidate = candidate,
            };

            var findMeetingTimeResponse = new FindMeetingTimeResponse
            {
                MeetingTimeSuggestions = new List<MeetingTimeSuggestion>
                {
                    new MeetingTimeSuggestion
                    {
                        MeetingTimeSlot = new MeetingTimeSlot
                        {
                            Start = new MeetingDateTime { DateTime = DateTime.Now.AddHours(12).ToString(), TimeZone = "UTC" },
                            End = new MeetingDateTime { DateTime = DateTime.Now.AddHours(13).ToString(), TimeZone = "UTC" },
                        },
                        AttendeeAvailability = new List<MeetingAttendeeAvailability>
                        {
                            new MeetingAttendeeAvailability
                            {
                                Attendee = new MeetingAttendee { EmailAddress = new MeetingAttendeeEmailAddress { Address = interviewer1.Email } },
                                Availability = "free",
                            },
                            new MeetingAttendeeAvailability
                            {
                                Attendee = new MeetingAttendee { EmailAddress = new MeetingAttendeeEmailAddress { Address = interviewer2.Email } },
                                Availability = "free",
                            },
                            new MeetingAttendeeAvailability
                            {
                                Attendee = new MeetingAttendee { EmailAddress = new MeetingAttendeeEmailAddress { Address = candidate.Email } },
                                Availability = "free",
                            },
                        },
                    },
                },
            };

            var freeBusyResponse = new List<MeetingInfo>
            {
                new MeetingInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserGroups = new UserGroup
                    {
                        FreeBusyTimeId = Guid.NewGuid().ToString(),
                        Users = new List<GraphPerson> { interviewer1 },
                    },
                    Requester = new GraphPerson { },
                    MeetingDetails = new List<MeetingDetails>
                    {
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer1
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(0),
                            UtcEnd = DateTime.Now.AddHours(8),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer1
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(11),
                            UtcEnd = DateTime.Now.AddHours(14),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer1
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(17),
                            UtcEnd = DateTime.Now.AddHours(24),
                            Id = Guid.NewGuid().ToString(),
                        },
                    },
                    InterviewerTimeSlotId = Guid.NewGuid().ToString(),
                },
                new MeetingInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserGroups = new UserGroup
                    {
                        FreeBusyTimeId = Guid.NewGuid().ToString(),
                        Users = new List<GraphPerson> { interviewer2 },
                    },
                    Requester = new GraphPerson { },
                    MeetingDetails = new List<MeetingDetails>
                    {
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer2
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(0),
                            UtcEnd = DateTime.Now.AddHours(8),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer2
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(11),
                            UtcEnd = DateTime.Now.AddHours(15),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer2
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(17),
                            UtcEnd = DateTime.Now.AddHours(24),
                            Id = Guid.NewGuid().ToString(),
                        },
                    },
                    InterviewerTimeSlotId = Guid.NewGuid().ToString(),
                },
                new MeetingInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserGroups = new UserGroup
                    {
                        FreeBusyTimeId = Guid.NewGuid().ToString(),
                        Users = new List<GraphPerson> { candidate },
                    },
                    Requester = new GraphPerson { },
                    MeetingDetails = new List<MeetingDetails>
                    {
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = candidate
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(0),
                            UtcEnd = DateTime.Now.AddHours(8),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = candidate
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(11),
                            UtcEnd = DateTime.Now.AddHours(12),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = candidate
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(17),
                            UtcEnd = DateTime.Now.AddHours(24),
                            Id = Guid.NewGuid().ToString(),
                        },
                    },
                    InterviewerTimeSlotId = Guid.NewGuid().ToString(),
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
                   this.outlookProviderMock.Setup(a => a.FindMeetingTimes(It.IsAny<FindMeetingTimeRequest>())).Returns(Task.FromResult(findMeetingTimeResponse));
                   this.scheduleManagerMock.Setup(a => a.GetFreeBusySchedule(It.IsAny<FreeBusyRequest>())).Returns(Task.FromResult(freeBusyResponse));

                   var scheduleManager = this.GetScheduleManagerInstance();

                   var resultPanel = scheduleManager.GetMeetingSuggestions(suggestMeetingsRequestPanel);
                   Assert.IsNotNull(resultPanel);
                   Assert.IsTrue(resultPanel.IsCompleted);

                   var resultNotPanel = scheduleManager.GetMeetingSuggestions(suggestMeetingsRequest);
                   Assert.IsNotNull(resultNotPanel);
                   Assert.IsTrue(resultNotPanel.IsCompleted);

               });
        }

        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProvider.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
