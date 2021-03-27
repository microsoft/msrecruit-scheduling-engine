//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Email.Contracts;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ServicePlatform.Tracing;
    using HR.TA.Talent.FalconEntities.Attract;
    using HR.TA.Talent.FalconEntities.IV.Entity;
    using Common = HR.TA.Common;

    [TestClass]
    public class FalconQueryTest
    {
        private FalconQuery falconQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<FalconQuery>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<Common.DocumentDB.IHcmDocumentClient> mockDocumentClient;

        [TestInitialize]
        public void BeforEach()
        {
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.mockDocumentClient = new Mock<Common.DocumentDB.IHcmDocumentClient>();
            this.loggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// GetTemplate Test
        /// </summary>
        [TestMethod]
        public void GetTemplateTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var result = this.falconQuery.GetTemplate(string.Empty);

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetTemplate Test
        /// </summary>
        [TestMethod]
        public void GetTemplateTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);

                   this.mockDocumentClient.Setup(a => a.Get<FalconEmailTemplate>(It.IsAny<string>(), null)).Returns(Task.FromResult(falconEmailTemplate));

                   var result = this.falconQuery.GetTemplate("Candidate");

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// GetRoleAssignment Test
        /// </summary>
        [TestMethod]
        public void GetRoleAssignmentTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var result = this.falconQuery.GetRoleAssignment(null);

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// GetRoleAssignment Test
        /// </summary>
        [TestMethod]
        public void GetRoleAssignmentTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            JobIVUser iVUser = new JobIVUser();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);

                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<JobIVUser, bool>>>(), null))
               .Returns(Task.FromResult(iVUser));

                   var result = this.falconQuery.GetRoleAssignment("Candidate");

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// GetWorker Test
        /// </summary>
        [TestMethod]
        public void GetWorkerTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var result = this.falconQuery.GetWorker(string.Empty);

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetWorkers Test
        /// </summary>
        [TestMethod]
        public void GetWorkersTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var result = this.falconQuery.GetWorkers(null);

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetWorkers Test
        /// </summary>
        [TestMethod]
        public void GetWorkersTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<FalconQuery>();

            List<string> list = new List<string>();
            list.Add("test");
            Worker workers = new Worker();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);

                   this.mockDocumentClient.Setup(a => a.Get<Worker>(It.IsAny<string>(), null)).Returns(Task.FromResult(workers));

                   var result = this.falconQuery.GetWorkers(list);

                   Assert.IsNotNull(result);
               });
        }
    }
}
