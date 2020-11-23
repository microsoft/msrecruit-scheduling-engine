// <copyright file="QueueMessageToServiceBusTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Business
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.ScheduleService.BusinessLibrary.Helpers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ServicePlatform.Azure.AAD;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class QueueMessageToServiceBusTests
    {
        private Mock<MS.GTA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient> mockAzureActiveDirectoryClient;
        private ServiceBusHelper serviceBusHelper;
        private Mock<ISecretManager> secretManager;
        private Mock<ITraceSource> mockTraceSource;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockAzureActiveDirectoryClient = new Mock<MS.GTA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient>();
            this.secretManager = new Mock<ISecretManager>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.serviceBusHelper = new ServiceBusHelper(this.mockAzureActiveDirectoryClient?.Object, this.secretManager?.Object, this.mockTraceSource?.Object);
        }

        [TestMethod]
        public void TestQueueMessageToServiceBusInvalidInput()
        {
            var serviceBusMessageNullException = this.serviceBusHelper.QueueMessageToNotificationWorker(null);

            Assert.IsInstanceOfType(serviceBusMessageNullException.Exception, typeof(AggregateException));
            Assert.IsInstanceOfType(serviceBusMessageNullException.Exception.InnerException, typeof(InvalidRequestDataValidationException));
        }
    }
}
