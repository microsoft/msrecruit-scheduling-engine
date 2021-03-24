//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using CommonLibrary.Common.MSGraph.Configuration;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>
    /// Test case class for room resource provider
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RoomResourceProviderTest
    {
        private const string ServiceAccountEmail = "TestServiceAccount@test.com";

        private Mock<IEmailClient> emailClientMoq;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<ILogger<RoomResourceProvider>> loggerMoq;
        private Mock<IConfigurationManager> configQueryMoq;
        private RoomResourceProvider service;
        private Mock<ITokenCacheService> tokenCacheMoq;
        private Mock<IHttpClientFactory> httpClientMoq;

        [TestInitialize]
        [ExcludeFromCodeCoverage]
        public void BeforEach()
        {
            this.emailClientMoq = new Mock<IEmailClient>();
            this.loggerMoq = new Mock<ILogger<RoomResourceProvider>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.configQueryMoq = new Mock<IConfigurationManager>();
            this.tokenCacheMoq = new Mock<ITokenCacheService>();
            this.httpClientMoq = new Mock<IHttpClientFactory>();
            this.configQueryMoq.Setup(config => config.Get<MsGraphSetting>()).Returns(new MsGraphSetting
            {
                GraphBaseUrl = "TestBaseUrl"
            });
            this.configQueryMoq.Setup(config => config.Get<SchedulerConfiguration>()).Returns(new SchedulerConfiguration
            {
                EmailAddress = ServiceAccountEmail
            });
            this.service = new RoomResourceProvider(this.configQueryMoq?.Object, this.emailClientMoq?.Object, this.tokenCacheMoq.Object, this.loggerMoq?.Object, this.httpClientMoq.Object);
        }

        /// <summary>
        /// Get Rooms with invalid input
        /// </summary>
        [TestMethod]
        public void GetRoomsWithInvalidInput()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<RoomResourceProvider>();
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   this.emailClientMoq.Setup(client => client.GetServiceAccountTokenByEmail(ServiceAccountEmail)).Returns(Task.FromResult("Token"));
                   var exception = this.service.GetRooms(null).Exception;
                   Assert.IsInstanceOfType(exception.InnerException, typeof(ArgumentNullException));

               });
        }
    }
}
