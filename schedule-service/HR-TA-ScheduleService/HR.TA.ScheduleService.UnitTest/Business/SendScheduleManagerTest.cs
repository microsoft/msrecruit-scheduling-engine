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
    using HR.TA.ScheduleService.BusinessLibrary.Exceptions;
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
    using HR.TA.Talent.EnumSetModel;
    using HR.TA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class SendScheduleManagerTest
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
        /// QueueScheduleTest
        /// </summary>
        [TestMethod]
        public void QueueScheduleTestWithNullInputs()
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
                   var exception = scheduleManager.QueueSchedule(string.Empty, ScheduleStatus.Queued, null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// QueueScheduleTest
        /// </summary>
        [TestMethod]
        public void QueueScheduleTestWithInValidInputs()
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
                   var result = scheduleManager.QueueSchedule("12345", ScheduleStatus.Queued, null, Guid.NewGuid().ToString());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// QueueScheduleTest
        /// </summary>
        [TestMethod]
        public void QueueScheduleTestWithValidInputs()
        {
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

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = ScheduleStatus.Queued;
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

                   this.scheduleQueryMock.Setup(a => a.GetJobApplicationIdForSchedule(It.IsAny<string>())).Returns(Task.FromResult(jobId));
                   this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));

                   this.scheduleQueryMock.Setup(a => a.UpdateScheduleEmailStatus(It.IsAny<string>(), ScheduleStatus.Queued, It.IsAny<EmailTemplate>())).Returns(Task.FromResult(meetingInfo));

                   var result = scheduleManager.QueueSchedule(meetingInfo.Id, ScheduleStatus.Queued, null, Guid.NewGuid().ToString());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// QueueScheduleTestWhenJobApplicationIsdispositionedAsync
        /// </summary>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [TestMethod]
        [ExpectedException(typeof(BusinessRuleViolationException))]
        public async Task QueueScheduleTestWhenJobApplicationIsdispositionedAsync()
        {
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
                ,
                Status = TalentEntities.Enum.JobApplicationStatus.Closed
            };

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = ScheduleStatus.Queued;
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();
            var scheduleManager = this.GetScheduleManagerInstance();
            this.scheduleQueryMock.Setup(a => a.GetJobApplicationIdForSchedule(It.IsAny<string>())).Returns(Task.FromResult(jobId));
            this.scheduleQueryMock.Setup(a => a.GetJobApplicationWithStatus(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));
            this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));
            this.scheduleQueryMock.Setup(a => a.UpdateScheduleEmailStatus(It.IsAny<string>(), ScheduleStatus.Queued, It.IsAny<EmailTemplate>())).Returns(Task.FromResult(meetingInfo));
            await scheduleManager.QueueSchedule(meetingInfo.Id, ScheduleStatus.Queued, null, Guid.NewGuid().ToString());

        }

        /// <summary>
        /// QueueScheduleTestWhenJobApplicationIsOfferedAndThrowBussinessException
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(BusinessRuleViolationException))]
        public async Task QueueScheduleTestWhenJobApplicationIsOfferedAndThrowBussinessException()
        {
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
                },
                Status = TalentEntities.Enum.JobApplicationStatus.Offered
            };

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();
            meetingInfo.ScheduleStatus = ScheduleStatus.Queued;
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();
            var scheduleManager = this.GetScheduleManagerInstance();
            this.scheduleQueryMock.Setup(a => a.GetJobApplicationWithStatus(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));
            this.scheduleQueryMock.Setup(a => a.GetJobApplicationIdForSchedule(It.IsAny<string>())).Returns(Task.FromResult(jobId));
            this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));
            this.scheduleQueryMock.Setup(a => a.UpdateScheduleEmailStatus(It.IsAny<string>(), ScheduleStatus.Queued, It.IsAny<EmailTemplate>())).Returns(Task.FromResult(meetingInfo));
            await scheduleManager.QueueSchedule(meetingInfo.Id, ScheduleStatus.Queued, null, Guid.NewGuid().ToString());

        }


        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProvider.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
