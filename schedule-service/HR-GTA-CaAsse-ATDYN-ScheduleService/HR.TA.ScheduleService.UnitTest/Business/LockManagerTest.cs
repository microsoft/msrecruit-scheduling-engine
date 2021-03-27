//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;

    [TestClass]
    public class LockManagerTest
    {
        private Mock<IScheduleQuery> scheduleQueryMock;

        private Mock<ILogger<LockManager>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforEach()
        {
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.loggerMock = new Mock<ILogger<LockManager>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// CreateScheduleLockItem
        /// </summary>
        [TestMethod]
        public void CreateScheduleLockItemTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager( this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.CreateScheduleLockItem(new SendInvitationLock());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleLockItems
        /// </summary>
        [TestMethod]
        public void GetScheduleLockItemsTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager(this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.GetScheduleLockItems(new List<string>());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// DeleteScheduleLockItem
        /// </summary>
        [TestMethod]
        public void DeleteScheduleLockItemTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager(this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.DeleteScheduleLockItem(string.Empty);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// DeleteScheduleLockItemsTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleLockItemsTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager(this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.DeleteScheduleLockItems(new List<string>());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// CreateNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void CreateNotificationLockItemTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager(this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.CreateNotificationLockItem(new NotificationMessageLock());

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// DeleteNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void DeleteNotificationLockItemTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager(this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.DeleteNotificationLockItem(string.Empty);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void GetNotificationLockItemTest()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<LockManager>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var lockManager = new LockManager(this.scheduleQueryMock.Object, this.loggerMock.Object);

                   var result = lockManager.GetNotificationLockItem(string.Empty);

                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
