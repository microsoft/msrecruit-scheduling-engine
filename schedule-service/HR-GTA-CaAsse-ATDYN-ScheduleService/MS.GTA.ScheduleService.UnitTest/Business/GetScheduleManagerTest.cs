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
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;

    [TestClass]
    public class GetScheduleManagerTest
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
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.outlookProviderMock = new Mock<IOutlookProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.emailHelperMock = new Mock<IEmailHelper>();
            this.notificationClientMock = new Mock<INotificationClient>();
            this.internalsProviderMock = new Mock<IInternalsProvider>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.loggerMock = new Mock<ILogger<ScheduleManager>>();
            this.contextMock = new Mock<IHCMServiceContext>();
            this.configMock = new Mock<IConfigurationManager>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// GetScheduleByScheduleIdTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdTestWithNullInputs()
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

                   var exception = scheduleManager.GetScheduleByScheduleId(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// GetScheduleByScheduleIdTest
        /// </summary>
        [TestMethod]
        public void GetScheduleByScheduleIdTestWithValidInputs()
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

                   var result = scheduleManager.GetScheduleByScheduleId("12345");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetSchedules
        /// </summary>
        [TestMethod]
        public void GetSchedulesTestWithNullInputs()
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

                   var exception = scheduleManager.GetSchedulesByFreeBusyRequest(null, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetSchedules
        /// </summary>
        [TestMethod]
        public void GetSchedulesTestWithValidInputs()
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
                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.GetSchedulesByFreeBusyRequest(meetingInfo, "123456");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationId
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithNullInputs()
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

                   var exception = scheduleManager.GetSchedulesByJobApplicationId(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BusinessRuleViolationException));
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationId
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithInValidInputs()
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
                   var scheduleManager = this.GetScheduleManagerInstance();

                   var result = scheduleManager.GetSchedulesByJobApplicationId("123456");

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleByJobApplicationId
        /// </summary>
        [TestMethod]
        public void GetScheduleByJobApplicationIdTestWithValidInputs()
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
                   var scheduleManager = this.GetScheduleManagerInstance();
                   this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));

                   var result = scheduleManager.GetSchedulesByJobApplicationId(jobId);

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
