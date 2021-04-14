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
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ServicePlatform.Tracing;
    using Common = HR.TA.Common;

    [TestClass]
    public class ScheduleLockDataTest
    {
        private IScheduleQuery scheduleQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<ScheduleQuery>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<Common.DocumentDB.IHcmDocumentClient> mockDocumentClient;

        [TestInitialize]
        public void BeforEach()
        {
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.mockDocumentClient = new Mock<Common.DocumentDB.IHcmDocumentClient>();
            this.loggerMock = new Mock<ILogger<ScheduleQuery>>();
            this.scheduleQuery = new ScheduleQuery(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// DeleteScheduleLockItemTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleLockItemTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.scheduleQuery.DeleteScheduleLockItem(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// DeleteScheduleLockItemTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleLockItemTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<SendInvitationLock, bool>>>(), null))
                     .Returns(Task.FromResult(new SendInvitationLock()));

                   var result = this.scheduleQuery.DeleteScheduleLockItem("123456");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// DeleteScheduleLockItemsTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleLockItemsTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.scheduleQuery.DeleteScheduleLockItems(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// DeleteScheduleLockItemsTest
        /// </summary>
        [TestMethod]
        public void DeleteScheduleLockItemsTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            List<string> items = new List<string>();
            items.Add("123456");

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<SendInvitationLock, bool>>>(), null))
                     .Returns(Task.FromResult(new SendInvitationLock()));

                   var result = this.scheduleQuery.DeleteScheduleLockItems(items);
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// CreateScheduleLockItemTest
        /// </summary>
        [TestMethod]
        public void CreateScheduleLockItemTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<SendInvitationLock, bool>>>(), null))
                     .Returns(Task.FromResult(new SendInvitationLock()));

                   var result = this.scheduleQuery.CreateScheduleLockItem(new SendInvitationLock());
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetScheduleLockItemsTest
        /// </summary>
        [TestMethod]
        public void GetScheduleLockItemsTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.scheduleQuery.GetScheduleLockItems(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetScheduleLockItemsTest
        /// </summary>
        [TestMethod]
        public void GetScheduleLockItemsTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<SendInvitationLock, bool>>>(), null))
                    .Returns(Task.FromResult<IEnumerable<SendInvitationLock>>(null));

                   var result = this.scheduleQuery.GetScheduleLockItems(new List<string>());
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// CreateNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void CreateNotificationLockItemTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<NotificationMessageLock, bool>>>(), null))
                     .Returns(Task.FromResult(new NotificationMessageLock()));

                   var result = this.scheduleQuery.CreateNotificationLockItem(new NotificationMessageLock());
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// DeleteNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void DeleteNotificationLockItemTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.scheduleQuery.DeleteNotificationLockItem(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// DeleteNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void DeleteNotificationLockItemTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<NotificationMessageLock, bool>>>(), null))
                     .Returns(Task.FromResult(new NotificationMessageLock()));

                   var result = this.scheduleQuery.DeleteNotificationLockItem("123456");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void GetNotificationLockItemTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.scheduleQuery.GetNotificationLockItem(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetNotificationLockItemTest
        /// </summary>
        [TestMethod]
        public void GetNotificationLockItemTestWithInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleQuery>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.GetFirstOrDefault(It.IsAny<Expression<Func<NotificationMessageLock, bool>>>(), null))
                     .Returns(Task.FromResult(new NotificationMessageLock()));

                   var result = this.scheduleQuery.GetNotificationLockItem("123456");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
