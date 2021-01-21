namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.TalentContracts.ScheduleService;

    [TestClass]
    public class SuggestAndCreateSchedulesTest
    {
        private IHttpContextAccessor httpContextAccessorMock;

        private Mock<IHCMServiceContext> hCMServiceContextMock;

        private Mock<IScheduleManager> scheduleManagerMock;

        private Mock<IRoleManager> roleManagerMock;

        private Mock<ILogger<ScheduleServiceController>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforeEach()
        {
            this.httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

            this.hCMServiceContextMock = new Mock<IHCMServiceContext>();
            this.scheduleManagerMock = new Mock<IScheduleManager>();
            this.loggerMock = new Mock<ILogger<ScheduleServiceController>>();
            this.roleManagerMock = new Mock<IRoleManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SuggestAndCreateSchedules With Invalid Inputs
        /// </summary>
        [TestMethod]
        public void SuggestAndCreateSchedulesWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();
            var jobApplicationId = Guid.NewGuid().ToString();

            var suggestMeetingsRequest = new SuggestMeetingsRequest
            {
                InterviewStartDateSuggestion = DateTime.Now,
                InterviewEndDateSuggestion = DateTime.Now.AddDays(1),
                Timezone = null,
                PanelInterview = false,
                PrivateMeeting = false,
                TeamsMeeting = false,
                MeetingDurationInMinutes = "60",
                InterviewersList = new List<UserGroup>
                {
                    new UserGroup
                    {
                        FreeBusyTimeId = Guid.NewGuid().ToString(),
                        Users = new List<GraphPerson>
                        {
                            new GraphPerson
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Interviewer1",
                                Email = "interviewer1@microsoft.com",
                                UserPrincipalName = "interviewer1",
                            }
                        }
                    },
                    new UserGroup
                    {
                        FreeBusyTimeId = Guid.NewGuid().ToString(),
                        Users = new List<GraphPerson>
                        {
                            new GraphPerson
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Interviewer2",
                                Email = "interviewer2@microsoft.com",
                                UserPrincipalName = "interviewer2",
                            }
                        }
                    },
                    new UserGroup
                    {
                        // empty slot
                        FreeBusyTimeId = Guid.NewGuid().ToString(),
                        Users = new List<GraphPerson>()
                    },
                },
                Candidate = new GraphPerson
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Candidate",
                    Email = "candidate@microsoft.com",
                    UserPrincipalName = "candidate",
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
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();
                   var controller = this.GetControllerInstance();

                   var exception = controller.SuggestAndCreateSchedules(suggestMeetingsRequest, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));

                   exception = controller.SuggestAndCreateSchedules(null, jobApplicationId).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// SuggestAndCreateSchedules With Valid Inputs
        /// </summary>
        [TestMethod]
        public void SuggestAndCreateSchedulesWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();
            var jobApplicationId = Guid.NewGuid().ToString();
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

            var suggestMeetingsRequest = new SuggestMeetingsRequest
            {
                InterviewStartDateSuggestion = DateTime.Now,
                InterviewEndDateSuggestion = DateTime.Now.AddDays(1),
                Timezone = null,
                PanelInterview = false,
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
                Candidate = new GraphPerson
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Candidate",
                    Email = "candidate@microsoft.com",
                    UserPrincipalName = "candidate",
                },
            };

            IList<MeetingInfo> getMeetingSuggestionsResponse = new List<MeetingInfo>
            {
                new MeetingInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserGroups = new UserGroup
                    {
                        FreeBusyTimeId = freeBusyTimeIdPanel,
                        Users = new List<GraphPerson> { interviewer1, interviewer2 },
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
                            UtcStart = DateTime.Now.AddHours(5),
                            UtcEnd = DateTime.Now.AddHours(6),
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
                            UtcStart = DateTime.Now.AddHours(6),
                            UtcEnd = DateTime.Now.AddHours(7),
                            Id = Guid.NewGuid().ToString(),
                        },
                        new MeetingDetails
                        {
                            Attendees = new List<Attendee>
                            {
                                new Attendee
                                {
                                    User = interviewer3
                                },
                            },
                            UtcStart = DateTime.Now.AddHours(7),
                            UtcEnd = DateTime.Now.AddHours(8),
                            Id = Guid.NewGuid().ToString(),
                        },
                    },
                    InterviewerTimeSlotId = Guid.NewGuid().ToString(),
                }
            };
            MeetingInfo suggestedSchedule = getMeetingSuggestionsResponse[0];

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

                   this.scheduleManagerMock.Setup(a => a.GetMeetingSuggestions(It.IsAny<SuggestMeetingsRequest>())).Returns(Task.FromResult(getMeetingSuggestionsResponse));
                   this.scheduleManagerMock.Setup(a => a.CreateSchedule(It.IsAny<MeetingInfo>(), It.IsAny<string>())).Returns(Task.FromResult(suggestedSchedule));
                   var controller = this.GetControllerInstance();

                   var result = controller.SuggestAndCreateSchedules(suggestMeetingsRequest, jobApplicationId);
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        private ScheduleServiceController GetControllerInstance()
        {
            return new ScheduleServiceController(this.httpContextAccessorMock, this.hCMServiceContextMock.Object, this.scheduleManagerMock.Object, this.roleManagerMock.Object, this.loggerMock.Object);
        }
    }
}
