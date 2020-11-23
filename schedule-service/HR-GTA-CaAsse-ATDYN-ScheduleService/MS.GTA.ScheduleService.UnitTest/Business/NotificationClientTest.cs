namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Email;
    using MS.GTA.Common.Email.GraphContracts;
    using MS.GTA.Common.Email.SendGridContracts;
    using MS.GTA.Common.MSGraph;
    using MS.GTA.ScheduleService.BusinessLibrary.Business.V1;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using EmailAddress = MS.GTA.ScheduleService.Contracts.V1.EmailAddress;

    [TestClass]
    public class NotificationClientTest
    {
        private Mock<IEmailService> emailServiceMoq;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<MS.GTA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient> azureClientMock;

        private Mock<ILogger<NotificationClient>> loggerMoq;
        private Mock<IConfigurationManager> configQueryMoq;
        private Mock<IMsGraphProvider> msgraphMoq;
        private NotificationClient service;
        private string subject = string.Empty;
        private Mock<ISecretManager> secretMoq;
        private Mock<ITokenCacheService> tokenCacheMoq;
        private Mock<IHttpClientFactory> httpClientMoq;

        [TestInitialize]
        public void BeforEach()
        {
            this.emailServiceMoq = new Mock<IEmailService>();
            this.loggerMoq = new Mock<ILogger<NotificationClient>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
            this.configQueryMoq = new Mock<IConfigurationManager>();
            this.msgraphMoq = new Mock<IMsGraphProvider>();
            this.secretMoq = new Mock<ISecretManager>();
            this.azureClientMock = new Mock<MS.GTA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient>();
            this.emailServiceMoq.Setup(serv => serv.DefaultDomainToSendFrom)
                 .Returns("cooldomain");
            this.tokenCacheMoq = new Mock<ITokenCacheService>();
            this.httpClientMoq = new Mock<IHttpClientFactory>();
            this.service = new NotificationClient(this.configQueryMoq?.Object, this.emailServiceMoq?.Object, this.msgraphMoq?.Object, this.azureClientMock?.Object, this.secretMoq?.Object, this.loggerMoq?.Object, this.tokenCacheMoq.Object, this.httpClientMoq.Object);
        }

        /// <summary>
        /// TransformFromEmail
        /// </summary>
        [TestMethod]
        public void TransformFromEmailTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<NotificationClient>();
            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var result = this.service.TransformFromEmail("Test@microsoft.com");
                   Assert.IsNotNull(result);
               });
        }
    }
}
