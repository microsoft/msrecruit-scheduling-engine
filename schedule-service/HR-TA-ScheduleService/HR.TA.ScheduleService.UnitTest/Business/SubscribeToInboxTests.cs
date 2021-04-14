//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using Castle.Core.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.BusinessLibrary.Business;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;

    [TestClass]
    public class SubscribeToInboxTests
    {
        private Mock<IDocDbDataAccess> mockDocDbDataAccess;
        private Mock<IOutlookProvider> mockOutlookProvider;
        private IGraphSubscriptionManager graphSubscriptionManager;
        private string serviceAccountToken;
        private SubscriptionViewModel subscriptionViewModel;
        private Mock<IConfigurationManager> mockConfiguarationManager;

        private Mock<ILogger<GraphSubscriptionManager>> loggerMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockDocDbDataAccess = new Mock<IDocDbDataAccess>();
            this.mockOutlookProvider = new Mock<IOutlookProvider>();
            this.loggerMock = new Mock<ILogger<GraphSubscriptionManager>>();
            this.mockConfiguarationManager = new Mock<IConfigurationManager>();
            this.graphSubscriptionManager = new GraphSubscriptionManager(this.mockDocDbDataAccess.Object, this.mockOutlookProvider.Object, this.loggerMock.Object, this.mockConfiguarationManager.Object);
            this.serviceAccountToken = Guid.NewGuid().ToString();
            this.subscriptionViewModel = this.GetSubscriptionViewModel(this.serviceAccountToken);
        }

        [TestMethod]
        public void SubscribeToInboxWithInvalidInput()
        {
            var serviceAccountTokenNullException = this.graphSubscriptionManager.SubscribeToInbox(It.IsAny<string>(), true);

            Assert.IsInstanceOfType(serviceAccountTokenNullException.Exception, typeof(AggregateException));
            Assert.IsInstanceOfType(serviceAccountTokenNullException.Exception.InnerException, typeof(InvalidRequestDataValidationException));
        }

        private SubscriptionViewModel GetSubscriptionViewModel(string serviceAccountEmail, bool isSystemServiceAccount = true)
        {
            var subscriptionViewModel = new SubscriptionViewModel
            {
                ServiceAccountEmail = serviceAccountEmail,
                TenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47",
                IsSystemServiceAccount = isSystemServiceAccount,
                Subscription = new Subscription()
                {
                    Id = Guid.NewGuid().ToString(),
                    ExpirationDateTime = DateTime.Now.AddHours(5)
                }
            };
            return subscriptionViewModel;
        }
    }
}
