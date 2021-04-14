//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using System.Collections.Generic;
    using HR.TA.Talent.EnumSetModel;
    using System.Threading.Tasks;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class JobApplicationTimeZoneTest
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
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.serviceBusHelperMock = new Mock<IServiceBusHelper>();
            this.contextMock = new Mock<IHCMServiceContext>();
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// UpdateTimezoneForJobApplicationTest
        /// </summary>
        [TestMethod]
        public void UpdateTimezoneForJobApplicationTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            string userID = Guid.NewGuid().ToString();
            string jobId = Guid.NewGuid().ToString();

            JobApplication jobApplication = new JobApplication()
            {
                JobApplicationID = jobId,
                JobApplicationParticipants = new List<JobApplicationParticipant>()
                {
                    new JobApplicationParticipant()
                    {
                        Role = TalentEntities.Enum.JobParticipantRole.Contributor,
                        OID = userID
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

                   var result = scheduleManager.UpdateTimezoneForJobApplication(jobId, new Timezone());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetTimezoneByJobApplicationIdTest
        /// </summary>
        [TestMethod]
        public void GetTimezoneByJobApplicationIdTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleManager>();

            string userID = Guid.NewGuid().ToString();
            string jobId = Guid.NewGuid().ToString();

            JobApplication jobApplication = new JobApplication()
            {
                JobApplicationID = jobId,
                JobApplicationParticipants = new List<JobApplicationParticipant>()
                {
                    new JobApplicationParticipant()
                    {
                        Role = TalentEntities.Enum.JobParticipantRole.Contributor,
                        OID = userID
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

                   var result = scheduleManager.GetTimezoneByJobApplicationId(jobId);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        private ScheduleManager GetScheduleManagerInstance()
        {
            return new ScheduleManager(this.contextMock.Object, this.outlookProviderMock.Object, this.scheduleQueryMock.Object, this.falconQuery, this.serviceBusHelperMock.Object, this.graphSubscriptionManagerMock.Object, this.emailClientMock.Object, this.notificationClientMock.Object, this.emailHelperMock.Object, this.internalsProviderMock.Object, this.userDetailsProviderMock.Object, this.configMock.Object, this.loggerMock.Object);
        }
    }
}
