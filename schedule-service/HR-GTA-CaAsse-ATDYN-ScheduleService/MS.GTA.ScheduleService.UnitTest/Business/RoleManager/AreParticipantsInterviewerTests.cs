//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.ScheduleService.UnitTest.Business.RoleManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AreParticipantsInterviewerTests
    {
        private Mock<IScheduleQuery> scheduleQueryMock;

        private FalconQuery falconQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<FalconQuery>> falconLoggerMock;

        private ILogger<RoleManager> logger;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforeEach()
        {
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.logger = NullLogger<RoleManager>.Instance;
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// AreParticipantsInterviewer Test with valid inputs
        /// </summary>
        [TestMethod]
        public void AreParticipantsInterviewerTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<RoleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var roleManager = this.GetRoleManagerInstance();
                   var participantOid = "testParticipantOid";
                   var participantOids = new List<string> { participantOid };
                   var jobApplicationId = "testJobApplicationId";
                   var jobApplication = new JobApplication
                   {
                       JobApplicationID = jobApplicationId,
                       JobApplicationParticipants = new List<JobApplicationParticipant>
                       {
                           new JobApplicationParticipant { OID = participantOid, Role = TalentEntities.Enum.JobParticipantRole.Interviewer },
                       },
                   };

                   this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));

                   var result = roleManager.AreParticipantsInterviewer(participantOids, jobApplicationId);

                   Assert.IsTrue(result.Result);
                   Assert.IsTrue(result.IsCompleted);

                   jobApplication = new JobApplication
                   {
                       JobApplicationID = jobApplicationId,
                       JobApplicationParticipants = new List<JobApplicationParticipant>
                       {
                           new JobApplicationParticipant { OID = participantOid, Role = TalentEntities.Enum.JobParticipantRole.HiringManager },
                       },
                   };

                   this.scheduleQueryMock.Setup(a => a.GetJobApplication(It.IsAny<string>())).Returns(Task.FromResult(jobApplication));

                   result = roleManager.AreParticipantsInterviewer(participantOids, jobApplicationId);

                   Assert.IsFalse(result.Result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// AreParticipantsInterviewer Test with invalid inputs
        /// </summary>
        [TestMethod]
        public void AreParticipantsInterviewerTestWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<RoleManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var roleManager = this.GetRoleManagerInstance();
                   var participantOids = new List<string> { "testParticipantOid" };
                   var jobApplicationId = "testJobApplicationId";

                   var exception = roleManager.AreParticipantsInterviewer(null, jobApplicationId).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));

                   exception = roleManager.AreParticipantsInterviewer(participantOids, string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        private RoleManager GetRoleManagerInstance()
        {
            return new RoleManager(this.scheduleQueryMock.Object, this.falconQuery, this.logger);
        }
    }
}
