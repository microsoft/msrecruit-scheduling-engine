namespace MS.GTA.ScheduleService.UnitTest.FalconData
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Email.Contracts;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.FalconEntities.IV.Entity;
    using Common = MS.GTA.Common;

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
