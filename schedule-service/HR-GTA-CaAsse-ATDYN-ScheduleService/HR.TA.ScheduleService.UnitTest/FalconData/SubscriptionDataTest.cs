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
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ServicePlatform.Tracing;
    using Common = HR.TA.Common;

    [TestClass]
    public class SubscriptionDataTest
    {
        private IDocDbDataAccess docDBAccess;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<Common.DocumentDB.IHcmDocumentClient> mockDocumentClient;

        private Mock<ILogger<DocDbDataAccess>> loggerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.loggerMock = new Mock<ILogger<DocDbDataAccess>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.mockDocumentClient = new Mock<Common.DocumentDB.IHcmDocumentClient>();
            this.falconQueryClientMock.Setup(m => m.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.mockDocumentClient.Object);
            this.docDBAccess = new DocDbDataAccess(this.falconQueryClientMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// GetSystemSubscriptionViewModelByEmailTest
        /// </summary>
        [TestMethod]
        public void GetSystemSubscriptionViewModelByEmailTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.docDBAccess.GetSystemSubscriptionViewModelByEmail(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetSystemSubscriptionViewModelByEmailTest
        /// </summary>
        [TestMethod]
        public void GetSystemSubscriptionViewModelByEmailTestWithInValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            IEnumerable<SubscriptionViewModel> subscriptionViewModels = null;
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<SubscriptionViewModel, bool>>>(), null))
                  .Returns(Task.FromResult<IEnumerable<SubscriptionViewModel>>(subscriptionViewModels));

                   var result = this.docDBAccess.GetSystemSubscriptionViewModelByEmail("Test");

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetSystemSubscriptionViewModelByEmailTest
        /// </summary>
        [TestMethod]
        public void GetSystemSubscriptionViewModelByEmailTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            IEnumerable<SubscriptionViewModel> subscriptionViewModels = new List<SubscriptionViewModel>()
            {
                new SubscriptionViewModel()
                {
                    EnvironmentId = "Test",
                    ServiceAccountEmail = "test",
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
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<SubscriptionViewModel, bool>>>(), null))
                  .Returns(Task.FromResult<IEnumerable<SubscriptionViewModel>>(subscriptionViewModels));

                   var result = this.docDBAccess.GetSystemSubscriptionViewModelByEmail("Test");

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// GetSystemSubscriptionViewModelByIdTest
        /// </summary>
        [TestMethod]
        public void GetSystemSubscriptionViewModelByIdTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.docDBAccess.GetSystemSubscriptionViewModelByIds(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// GetSystemSubscriptionViewModelByEmailTest
        /// </summary>
        [TestMethod]
        public void GetSystemSubscriptionViewModelByIdTestWithInValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            IEnumerable<SubscriptionViewModel> subscriptionViewModels = null;
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<SubscriptionViewModel, bool>>>(), null))
                  .Returns(Task.FromResult<IEnumerable<SubscriptionViewModel>>(subscriptionViewModels));

                   var result = this.docDBAccess.GetSystemSubscriptionViewModelByIds(new List<string>());

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetSystemSubscriptionViewModelByIdTest
        /// </summary>
        [TestMethod]
        public void GetSystemSubscriptionViewModelByIdTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            IEnumerable<SubscriptionViewModel> subscriptionViewModels = new List<SubscriptionViewModel>()
            {
                new SubscriptionViewModel()
                {
                    EnvironmentId = "Test",
                    ServiceAccountEmail = "test",
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
                   this.mockDocumentClient.Setup(a => a.Get(It.IsAny<Expression<Func<SubscriptionViewModel, bool>>>(), null))
                  .Returns(Task.FromResult(subscriptionViewModels));

                   var result = this.docDBAccess.GetSystemSubscriptionViewModelByIds(new List<string>() { "Test" });

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// CreateSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void CreateSubscriptionViewModelTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.docDBAccess.CreateSubscriptionViewModel(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// CreateSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void CreateSubscriptionViewModelTestWithInValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Create<SubscriptionViewModel>(It.IsAny<SubscriptionViewModel>(), null)).Throws(new Exception());

                   var result = this.docDBAccess.CreateSubscriptionViewModel(new SubscriptionViewModel());

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// CreateSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void CreateSubscriptionViewModelTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            SubscriptionViewModel sub = new SubscriptionViewModel()
            {
                EnvironmentId = "Test",
                ServiceAccountEmail = "test",
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
                   this.mockDocumentClient.Setup(a => a.Create<SubscriptionViewModel>(It.IsAny<SubscriptionViewModel>(), null)).Returns(Task.FromResult(sub));
                   var result = this.docDBAccess.CreateSubscriptionViewModel(sub);

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// UpdateSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void UpdateSubscriptionViewModelTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.docDBAccess.UpdateSubscriptionViewModel(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// UpdateSubscriptionViewModellTest
        /// </summary>
        [TestMethod]
        public void UpdateSubscriptionViewModelTestWithInValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Update<SubscriptionViewModel>(It.IsAny<SubscriptionViewModel>(), null)).Throws(new Exception());

                   var result = this.docDBAccess.UpdateSubscriptionViewModel(new SubscriptionViewModel());

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// UpdateSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void UpdateSubscriptionViewModelTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            SubscriptionViewModel sub = new SubscriptionViewModel()
            {
                EnvironmentId = "Test",
                ServiceAccountEmail = "test",
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
                   this.mockDocumentClient.Setup(a => a.Update<SubscriptionViewModel>(It.IsAny<SubscriptionViewModel>(), null)).Returns(Task.FromResult(sub));
                   var result = this.docDBAccess.UpdateSubscriptionViewModel(sub);

                   Assert.IsNotNull(result);
               });
        }

        /// <summary>
        /// DeleteSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void DeleteSubscriptionViewModelTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var exception = this.docDBAccess.DeleteSubscriptionViewModel(null).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(BadRequestStatusException));
               });
        }

        /// <summary>
        /// DeleteSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void DeleteSubscriptionViewModelTestWithInValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.mockDocumentClient.Setup(a => a.Delete<SubscriptionViewModel>(It.IsAny<string>(), null)).Throws(new Exception());
                   var result = this.docDBAccess.DeleteSubscriptionViewModel("Test");

                   Assert.IsTrue(result.Result);
               });
        }

        /// <summary>
        /// DeleteSubscriptionViewModelTest
        /// </summary>
        [TestMethod]
        public void DeleteSubscriptionViewModelTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<DocDbDataAccess>();

            SubscriptionViewModel sub = new SubscriptionViewModel()
            {
                EnvironmentId = "Test",
                ServiceAccountEmail = "test",
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
                   var result = this.docDBAccess.DeleteSubscriptionViewModel("12345");

                   Assert.IsTrue(result.Result);
               });
        }
    }
}
