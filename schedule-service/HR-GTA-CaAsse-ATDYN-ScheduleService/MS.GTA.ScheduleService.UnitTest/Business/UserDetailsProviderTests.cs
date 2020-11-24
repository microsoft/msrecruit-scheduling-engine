// <copyright file="UserDetailsProviderTests.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers.User;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;

    [TestClass]
    public class UserDetailsProviderTests
    {
        private Mock<IScheduleQuery> scheduleQueryMock;

        private FalconQuery falconQuery;

        private Mock<IFalconQueryClient> falconQueryClientMock;

        private Mock<ILogger<FalconQuery>> falconLoggerMock;

        private Mock<IGraphSubscriptionManager> graphSubscriptionManagerMock;

        private Mock<IEmailClient> emailClientMock;

        private Mock<IHttpClientFactory> httpClientMock;

        private Mock<IUserDetailsProvider> userDetailsProviderMock;

        private Mock<ITokenCacheService> tokenCacheServiceMock;

        private Mock<ILogger<UserDetailsProvider>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<IHCMServiceContext> contextMock;

        private Mock<IConfigurationManager> configMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.contextMock = new Mock<IHCMServiceContext>();
            this.scheduleQueryMock = new Mock<IScheduleQuery>();
            this.falconQueryClientMock = new Mock<IFalconQueryClient>();
            this.falconLoggerMock = new Mock<ILogger<FalconQuery>>();
            this.falconQuery = new FalconQuery(this.falconQueryClientMock.Object, this.falconLoggerMock.Object);
            this.graphSubscriptionManagerMock = new Mock<IGraphSubscriptionManager>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.emailClientMock = new Mock<IEmailClient>();
            this.tokenCacheServiceMock = new Mock<ITokenCacheService>();
            this.loggerMock = new Mock<ILogger<UserDetailsProvider>>();
            this.configMock = new Mock<IConfigurationManager>();
            this.httpClientMock = new Mock<IHttpClientFactory>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// GetUserAsyncTest
        /// </summary>
        [TestMethod]
        public void GetUserAsyncTestWithNullInputs()
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
                   var userDetailsManager = this.GetUserDetailsProviderInstance();
                   var exception = userDetailsManager.GetUserAsync(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetUserPhotoAsyncTest
        /// </summary>
        [TestMethod]
        public void GetUserPhotoAsyncTestWithNullInputs()
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
                   var userDetailsManager = this.GetUserDetailsProviderInstance();
                   var exception = userDetailsManager.GetUserPhotoAsync(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetUserAsyncTest
        /// </summary>
        [TestMethod]
        public void GetUserAsyncTestWithInvalidToken()
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
                   var userDetailsManager = this.GetUserDetailsProviderInstance();
                   this.emailClientMock.Setup(a => a.GetServiceAccountTokenByEmail(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

                   var result = userDetailsManager.GetUserAsync("12345");

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetUserPhotoAsyncTest
        /// </summary>
        [TestMethod]
        public void GetUserPhotoAsyncTestWithInvalidToken()
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
                   var userDetailsManager = this.GetUserDetailsProviderInstance();

                   this.emailClientMock.Setup(a => a.GetServiceAccountTokenByEmail(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

                   var result = userDetailsManager.GetUserPhotoAsync("123456");

                   Assert.IsNull(result.Result);
               });
        }

        /// <summary>
        /// GetUserAsyncTest
        /// </summary>
        [TestMethod]
        public void GetUserAsyncTestWithValidToken()
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
                   var userDetailsManager = this.GetUserDetailsProviderInstance();
                   this.emailClientMock.Setup(a => a.GetServiceAccountTokenByEmail(It.IsAny<string>())).Returns(Task.FromResult("Token"));

                   var exception = userDetailsManager.GetUserAsync("123456").Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(NullReferenceException));
               });
        }

        /// <summary>
        /// GetUserPhotoAsyncTest
        /// </summary>
        [TestMethod]
        public void GetUserPhotoAsyncTestWithValidToken()
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
                   var userDetailsManager = this.GetUserDetailsProviderInstance();

                   this.emailClientMock.Setup(a => a.GetServiceAccountTokenByEmail(It.IsAny<string>())).Returns(Task.FromResult("Token"));

                   var exception = userDetailsManager.GetUserPhotoAsync("123456").Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(NullReferenceException));
               });
        }

        private UserDetailsProvider GetUserDetailsProviderInstance()
        {
            return new UserDetailsProvider(this.configMock.Object, this.httpClientMock.Object, this.emailClientMock.Object, this.tokenCacheServiceMock.Object, this.loggerMock.Object);
        }
    }
}
