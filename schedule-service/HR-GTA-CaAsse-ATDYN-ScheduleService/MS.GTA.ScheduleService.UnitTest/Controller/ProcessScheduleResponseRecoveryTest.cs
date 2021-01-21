// <copyright file="ProcessScheduleResponseRecoveryTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class ProcessScheduleResponseRecoveryTest
    {
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private Mock<INotificationManager> mockNotificationManager;
        private Mock<ILogger<RecoveryController>> loggerMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.mockNotificationManager = new Mock<INotificationManager>();
            this.loggerMock = new Mock<ILogger<RecoveryController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// Test ProcessScheduleResponses With ValidInputs
        /// </summary>
        [TestMethod]
        public void TestProcessScheduleResponsesWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<RecoveryController>();

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

                    this.mockNotificationManager.Setup(a => a.ProcessScheduleResponse()).Returns(Task.CompletedTask);
                    var recoveryController = new RecoveryController(httpContextAccessorMock, this.mockNotificationManager.Object, this.loggerMock.Object);

                    var response = recoveryController.ProcessScheduleResponses();

                    Assert.AreEqual(response.IsCompletedSuccessfully, true);
                    this.mockNotificationManager.Verify(a => a.ProcessScheduleResponse(), Times.Once);
                });
        }

        /// <summary>
        /// Test ProcessScheduleResponses With InvalidInputs
        /// </summary>
        [TestMethod]
        public void TestProcessScheduleResponsesWithInvalidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<RecoveryController>();

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

                    this.mockNotificationManager.Setup(a => a.ProcessScheduleResponse()).Returns(Task.CompletedTask);

                    var exception = Assert.ThrowsException<ArgumentNullException>(() =>
                        new RecoveryController(httpContextAccessorMock, null, this.loggerMock.Object));

                    Assert.IsTrue(exception.ParamName.Equals("notificationManager", StringComparison.OrdinalIgnoreCase));

                });
        }
    }
}
