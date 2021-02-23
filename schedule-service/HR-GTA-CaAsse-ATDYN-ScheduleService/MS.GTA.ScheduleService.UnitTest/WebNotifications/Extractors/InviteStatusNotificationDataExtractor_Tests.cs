//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.WebNotifications.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.Common.WebNotifications;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Extractors;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.Data.Models;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.TalentEntities.Enum;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class InviteStatusNotificationDataExtractor_Tests
    {
        private readonly string jobApplicationId = "Job Application Id #1";
        private readonly string jobTitle = "Software Engineer II";
        private readonly string interviewerName = "I'm Interviewer.";
        private readonly string interviewerOid = "Interviewer Oid";
        private readonly string interviewerEmail = "abc@xyz.com";
        private readonly string schedulerName = "I'm scheduler.";
        private readonly string schedulerOid = "Scheduler Oid";
        private readonly string schedulerEmail = "scheduler@xyz.com";
        private readonly string delegateName = "I'm Delegate.";
        private readonly string delegateOid = "Delegate Oid";
        private readonly string delegateEmail = "delegate@xyz.com";
        private readonly string recruiterEmail = Guid.NewGuid().ToString("N");
        private readonly string recruiterOid = Guid.NewGuid().ToString();
        private readonly string recruiterName = Guid.NewGuid().ToString("N");

        private readonly DeepLinksConfiguration deepLinksConfiguration = new DeepLinksConfiguration
        {
            BaseUrl = "https://www.domain.com",
            ScheduleSummaryUrl = "https://www.domain.com/iv/{JobApplicationId}"
        };

        private IEnumerable<InterviewerInviteResponseInfo> interviewerInviteResponseInfos;

        private JobApplicationParticipants jobApplicationParticipants;

        private InviteStatusNotificationDataExtractor dataExtractor;

        private List<Delegation> activeDelegations;

        private Mock<IScheduleQuery> scheduleQueryMock;


        [TestInitialize]
        public void BeforeEach()
        {
            this.jobApplicationParticipants = new JobApplicationParticipants
            {
                Application = new JobApplication
                {
                    JobApplicationID = this.jobApplicationId,
                    JobOpening = new JobOpening
                    {
                        PositionTitle = this.jobTitle,
                    },
                    JobApplicationParticipants = new List<JobApplicationParticipant>
                    {
                        new JobApplicationParticipant
                        {
                            OID = this.interviewerOid,
                            Role = JobParticipantRole.Interviewer,
                        },
                        new JobApplicationParticipant
                        {
                            OID = this.schedulerOid,
                            Role = JobParticipantRole.Contributor,
                        },
                        new JobApplicationParticipant
                        {
                            OID = this.recruiterOid,
                            Role = JobParticipantRole.Recruiter
                        }
                    },
                },
            };

            this.jobApplicationParticipants.Participants.AddRange(new List<IVWorker>
            {
                new IVWorker
                {
                    EmailPrimary = this.interviewerEmail,
                    WorkerId = this.interviewerOid,
                    Name = new PersonName
                    {
                        GivenName = this.interviewerName,
                    }
                },
                new IVWorker
                {
                    EmailPrimary = this.schedulerEmail,
                    WorkerId = this.schedulerOid,
                    Name = new PersonName
                    {
                        GivenName = this.schedulerName,
                    }
                },
                new IVWorker
                {
                    EmailPrimary = this.recruiterEmail,
                    WorkerId = this.recruiterOid,
                    OfficeGraphIdentifier = this.recruiterOid,
                    Name = new PersonName
                    {
                        GivenName = this.recruiterName
                    }
                }
            });

            this.interviewerInviteResponseInfos = new List<InterviewerInviteResponseInfo>
            {
                new InterviewerInviteResponseInfo
                {
                    ApplicationParticipants = this.jobApplicationParticipants,
                    InterviewerMessage = null,
                    ResponseNotification = new InterviewerResponseNotification
                    {
                        InterviewerOid = this.interviewerOid,
                        JobApplicationId = this.jobApplicationId,
                        ResponseStatus = InvitationResponseStatus.Declined,
                        ScheduleId = Guid.NewGuid().ToString(),
                    }
                }
            };

            Delegation delegation = new Delegation()
            {
                To = new Worker()
                {
                    OfficeGraphIdentifier = this.delegateOid,
                    EmailPrimary = this.delegateEmail,
                    Name = new PersonName()
                    {
                        GivenName = this.delegateName
                    }
                }
            };
            this.activeDelegations = new List<Delegation>();
            this.activeDelegations.Add(delegation);
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.dataExtractor = new InviteStatusNotificationDataExtractor(this.interviewerInviteResponseInfos, this.deepLinksConfiguration, this.scheduleQueryMock.Object, NullLogger<InviteStatusNotificationDataExtractor>.Instance);
        }

        [TestMethod]
        public void Ctor_NullInterviewerMessageResponseJobApplicationsObjects()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new InviteStatusNotificationDataExtractor(interviewerInviteResponseInfos: null, deepLinksConfiguration: this.deepLinksConfiguration, scheduleQuery: this.scheduleQueryMock.Object, logger: NullLogger<InviteStatusNotificationDataExtractor>.Instance));
            Assert.IsTrue(ex.ParamName.Equals("interviewerInviteResponseInfos", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Ctor_NullDeepLinksConfiguration()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new InviteStatusNotificationDataExtractor(this.interviewerInviteResponseInfos, deepLinksConfiguration: null, scheduleQuery: null, logger: NullLogger<InviteStatusNotificationDataExtractor>.Instance));
            Assert.IsTrue(ex.ParamName.Equals("deepLinksConfiguration", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Ctor_NullLogger()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new InviteStatusNotificationDataExtractor(this.interviewerInviteResponseInfos, deepLinksConfiguration: this.deepLinksConfiguration, scheduleQuery: this.scheduleQueryMock.Object, logger: null));
            Assert.IsTrue(ex.ParamName.Equals("logger", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Ctor_ValidInput()
        {
            Assert.IsInstanceOfType(this.dataExtractor, typeof(InviteStatusNotificationDataExtractor));
        }

        [TestMethod]
        public void Extract_InvokeInterviewerResponseSimultaneously()
        {
            // Arrange
            var responseInfos = new List<InterviewerInviteResponseInfo>
            {
                new InterviewerInviteResponseInfo
                {
                    ApplicationParticipants = this.jobApplicationParticipants,
                    InterviewerMessage = null,
                    ResponseNotification = new InterviewerResponseNotification
                    {
                        InterviewerOid = this.interviewerOid,
                        JobApplicationId = this.jobApplicationId,
                        ResponseStatus = InvitationResponseStatus.Declined,
                        ScheduleId = this.interviewerInviteResponseInfos.First().ResponseNotification.ScheduleId,
                    }
                }
            };
            InviteStatusNotificationDataExtractor localDataExtractor = new InviteStatusNotificationDataExtractor(responseInfos, this.deepLinksConfiguration, this.scheduleQueryMock.Object, NullLogger<InviteStatusNotificationDataExtractor>.Instance);

            // Act
            var notificationsPropertiesTask = this.dataExtractor.Extract();
            var localnotificationsPropertiesTask = localDataExtractor.Extract();
            var localnotificationsProperties = localnotificationsPropertiesTask.Result;
            var notificationsProperties = notificationsPropertiesTask.Result;
            // Assert
            Assert.IsTrue(notificationsProperties.Count() == 0 || localnotificationsProperties.Count() == 0);
        }

        [TestMethod]
        public void Extract_InvokeInterviewerResponseWithoutMessage()
        {
            var notificationsProperties = this.dataExtractor.Extract().Result;
            Assert.IsNotNull(notificationsProperties);
            Assert.AreEqual(notificationsProperties.Count(), 2);
            var properties = notificationsProperties.First();
            var secondNotificationProperties = notificationsProperties.ElementAt(1);
            Assert.AreEqual(properties[NotificationConstants.JobTitle], this.jobTitle);
            Assert.AreEqual(properties[WebNotificationConstants.InterviewerName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientName], this.schedulerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientEmail], this.schedulerEmail);
            Assert.AreEqual(properties[NotificationConstants.RecipientObjectId], this.schedulerOid);
            Assert.AreEqual(properties[NotificationConstants.SenderEmail], this.interviewerEmail);
            Assert.AreEqual(properties[NotificationConstants.SenderName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.SenderObjectId], this.interviewerOid);
            Assert.AreEqual(properties[WebNotificationConstants.StatusVerb], "declined");
            Assert.IsTrue(properties.ContainsKey(NotificationConstants.AppNotificationType));
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientName], this.recruiterName);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientEmail], this.recruiterEmail);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientObjectId], this.recruiterOid);
        }

        [TestMethod]
        public void Extract_InvokeInterviewerResponseNoMessageResponseWithProposedTime()
        {
            var currentDate = DateTime.UtcNow.AddDays(1);
            this.interviewerInviteResponseInfos.First().ResponseNotification.ProposedNewTime = new MeetingTimeSpan
            {
                Start = new MeetingDateTime
                {
                    DateTime = currentDate.ToString(),
                    TimeZone = "UTC",
                },
                End = new MeetingDateTime
                {
                    DateTime = currentDate.AddMinutes(30).ToString(),
                    TimeZone = "UTC",
                },
            };
            var notificationsProperties = this.dataExtractor.Extract().Result;
            Assert.IsNotNull(notificationsProperties);
            Assert.AreEqual(notificationsProperties.Count(), 2);
            var properties = notificationsProperties.First();
            var secondNotificationProperties = notificationsProperties.ElementAt(1);
            Assert.AreEqual(properties[NotificationConstants.JobTitle], this.jobTitle);
            Assert.AreEqual(properties[WebNotificationConstants.InterviewerName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientName], this.schedulerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientEmail], this.schedulerEmail);
            Assert.AreEqual(properties[NotificationConstants.RecipientObjectId], this.schedulerOid);
            Assert.AreEqual(properties[NotificationConstants.SenderEmail], this.interviewerEmail);
            Assert.AreEqual(properties[NotificationConstants.SenderName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.SenderObjectId], this.interviewerOid);
            Assert.AreEqual(properties[WebNotificationConstants.StatusVerb], "declined");
            Assert.AreEqual(properties[WebNotificationConstants.ProposedStartTime], currentDate.ToString() + "Z");
            Assert.AreEqual(properties[WebNotificationConstants.ProposedEndTime], currentDate.AddMinutes(30).ToString() + "Z");
            Assert.IsTrue(properties.ContainsKey(NotificationConstants.AppNotificationType));
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientName], this.recruiterName);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientEmail], this.recruiterEmail);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientObjectId], this.recruiterOid);
        }

        [TestMethod]
        public void Extract_InvokeInterviewerResponseWithMessage()
        {
            this.interviewerInviteResponseInfos.First().InterviewerMessage = "This is an interviewer message.";
            var notificationsProperties = this.dataExtractor.Extract().Result;
            Assert.IsNotNull(notificationsProperties);
            Assert.AreEqual(notificationsProperties.Count(), 2);
            var properties = notificationsProperties.First();
            var secondNotificationProperties = notificationsProperties.ElementAt(1);
            Assert.AreEqual(properties[NotificationConstants.JobTitle], this.jobTitle);
            Assert.AreEqual(properties[WebNotificationConstants.InterviewerName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientName], this.schedulerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientEmail], this.schedulerEmail);
            Assert.AreEqual(properties[NotificationConstants.RecipientObjectId], this.schedulerOid);
            Assert.AreEqual(properties[NotificationConstants.SenderEmail], this.interviewerEmail);
            Assert.AreEqual(properties[NotificationConstants.SenderName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.SenderObjectId], this.interviewerOid);
            Assert.AreEqual(properties[WebNotificationConstants.MessageResponse], "This is an interviewer message.");
            Assert.AreEqual(properties[WebNotificationConstants.StatusVerb], "declined");
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientName], this.recruiterName);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientEmail], this.recruiterEmail);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientObjectId], this.recruiterOid);
            Assert.IsTrue(properties.ContainsKey(NotificationConstants.AppNotificationType));
        }

        [TestMethod]
        public void Extract_InvokeInterviewerResponseWithMessageNewTime()
        {
            var currentDate = DateTime.UtcNow.AddDays(1);
            this.interviewerInviteResponseInfos.First().ResponseNotification.ProposedNewTime = new MeetingTimeSpan
            {
                Start = new MeetingDateTime
                {
                    DateTime = currentDate.ToString(),
                    TimeZone = "UTC",
                },
                End = new MeetingDateTime
                {
                    DateTime = currentDate.AddMinutes(30).ToString(),
                    TimeZone = "UTC",
                },
            };

            this.interviewerInviteResponseInfos.First().InterviewerMessage = "This is an interviewer message.";
            var notificationsProperties = this.dataExtractor.Extract().Result;
            Assert.IsNotNull(notificationsProperties);
            Assert.AreEqual(notificationsProperties.Count(), 2);
            var properties = notificationsProperties.First();
            var secondNotificationProperties = notificationsProperties.ElementAt(1);
            Assert.AreEqual(properties[NotificationConstants.JobTitle], this.jobTitle);
            Assert.AreEqual(properties[WebNotificationConstants.InterviewerName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientName], this.schedulerName);
            Assert.AreEqual(properties[NotificationConstants.RecipientEmail], this.schedulerEmail);
            Assert.AreEqual(properties[NotificationConstants.RecipientObjectId], this.schedulerOid);
            Assert.AreEqual(properties[NotificationConstants.SenderEmail], this.interviewerEmail);
            Assert.AreEqual(properties[NotificationConstants.SenderName], this.interviewerName);
            Assert.AreEqual(properties[NotificationConstants.SenderObjectId], this.interviewerOid);
            Assert.AreEqual(properties[WebNotificationConstants.MessageResponse], "This is an interviewer message.");
            Assert.AreEqual(properties[WebNotificationConstants.StatusVerb], "declined");
            Assert.AreEqual(properties[WebNotificationConstants.ProposedStartTime], currentDate.ToString() + "Z");
            Assert.AreEqual(properties[WebNotificationConstants.ProposedEndTime], currentDate.AddMinutes(30).ToString() + "Z");
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientName], this.recruiterName);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientEmail], this.recruiterEmail);
            Assert.AreEqual(secondNotificationProperties[NotificationConstants.RecipientObjectId], this.recruiterOid);
            Assert.IsTrue(properties.ContainsKey(NotificationConstants.AppNotificationType));
        }

        [TestMethod]
        public void Extract_InvokeInterviewerResponseForWobUser()
        {
            this.scheduleQueryMock.Setup(a => a.GetWobUsersDelegationAsync(It.IsAny<List<string>>())).ReturnsAsync(this.activeDelegations);
            this.dataExtractor = new InviteStatusNotificationDataExtractor(this.interviewerInviteResponseInfos, this.deepLinksConfiguration, this.scheduleQueryMock.Object, NullLogger<InviteStatusNotificationDataExtractor>.Instance);
            var notificationsProperties = this.dataExtractor.Extract().Result;
            Assert.IsNotNull(notificationsProperties);
            Assert.AreEqual(notificationsProperties.Count(), 3);
            var wobProperties = notificationsProperties.ElementAt(2);
            Assert.AreEqual(wobProperties[NotificationConstants.RecipientName], this.delegateName);
            Assert.AreEqual(wobProperties[NotificationConstants.RecipientEmail], this.delegateEmail);
            Assert.AreEqual(wobProperties[NotificationConstants.RecipientObjectId], this.delegateOid);
            Assert.AreEqual(wobProperties[NotificationConstants.IsWobContext], "True");
        }
    }
}
