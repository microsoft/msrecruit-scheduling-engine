//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.Email;
    using HR.TA.Common.Email.GraphContracts;
    using HR.TA.Common.Email.SendGridContracts;
    using HR.TA.Common.MSGraph;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ServicePlatform.Azure.Security;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.ServicePlatform.Tracing;
    using EmailAddress = HR.TA.ScheduleService.Contracts.V1.EmailAddress;

    [TestClass]
    public class NotificationClientTest
    {
        private Mock<IEmailService> emailServiceMoq;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        private Mock<HR.TA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient> azureClientMock;

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
            this.azureClientMock = new Mock<HR.TA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient>();
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
