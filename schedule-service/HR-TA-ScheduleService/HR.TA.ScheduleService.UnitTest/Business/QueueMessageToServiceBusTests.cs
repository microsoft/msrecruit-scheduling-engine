//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.ScheduleService.BusinessLibrary.Helpers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ServicePlatform.Azure.AAD;
    using HR.TA.ServicePlatform.Azure.Security;
    using HR.TA.ServicePlatform.Tracing;

    [TestClass]
    public class QueueMessageToServiceBusTests
    {
        private Mock<HR.TA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient> mockAzureActiveDirectoryClient;
        private ServiceBusHelper serviceBusHelper;
        private Mock<ISecretManager> secretManager;
        private Mock<ITraceSource> mockTraceSource;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockAzureActiveDirectoryClient = new Mock<HR.TA.ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient>();
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
