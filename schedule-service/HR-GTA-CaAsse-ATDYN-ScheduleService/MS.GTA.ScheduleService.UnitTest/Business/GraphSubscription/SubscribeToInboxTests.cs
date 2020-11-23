namespace MS.GTA.ScheduleService.UnitTest.Business.GraphSubscription
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.ScheduleService.BusinessLibrary.Business;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class SubscribeToInboxTests
    {
        private string serviceAccountToken;
        private Mock<IOutlookProvider> mockOutlookProvider;
        private Mock<IDocDbDataAccess> mockDocDbDataAccess;
        private Mock<ITraceSource> mockTraceSource;
        private IGraphSubscriptionManager graphSubscriptionManager;
        private Mock<ILogger<GraphSubscriptionManager>> loggerMock;
        private SubscriptionViewModel subscriptionViewModel;
        private Mock<IConfigurationManager> mockConfiguarationManager;

        [TestInitialize]
        public void BeforEach()
        {
            this.mockDocDbDataAccess = new Mock<IDocDbDataAccess>();
            this.mockOutlookProvider = new Mock<IOutlookProvider>();
            this.loggerMock = new Mock<ILogger<GraphSubscriptionManager>>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.mockConfiguarationManager = new Mock<IConfigurationManager>();
            this.graphSubscriptionManager = new GraphSubscriptionManager(this.mockDocDbDataAccess.Object, this.mockOutlookProvider.Object, this.loggerMock.Object, this.mockConfiguarationManager.Object);
            this.subscriptionViewModel = this.GetMockSubscriptionViewModel();
            this.serviceAccountToken = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJpc3MiOiJodHRwczovL2p3dC1pZHAuZXhhbXBsZS5jb20iLCJzdWIiOiJtYWlsdG86bWlrZUBleGFtcGxlLmNvbSIsIm5iZiI6MTU3MDUzMTAxNiwiZXhwIjoxNTcwNTM0NjE2LCJpYXQiOjE1NzA1MzEwMTYsImp0aSI6ImlkMTIzNDU2IiwidHlwIjoiaHR0cHM6Ly9leGFtcGxlLmNvbS9yZWdpc3RlciJ9.";
        }

        [TestMethod]
        public void TestSubscribeToInboxInvalidInput()
        {
            var serviceAccountEmailNullException = this.graphSubscriptionManager.SubscribeToInbox(It.IsAny<string>(), true);

            Assert.IsInstanceOfType(serviceAccountEmailNullException.Exception, typeof(AggregateException));
            Assert.IsInstanceOfType(serviceAccountEmailNullException.Exception.InnerException, typeof(InvalidRequestDataValidationException));
        }

        [TestMethod]
        public void TestSubscribeToInboxValidInput()
        {
            this.mockDocDbDataAccess.Setup(a => a.GetSystemSubscriptionViewModelByEmail(It.IsAny<string>())).ReturnsAsync(new List<SubscriptionViewModel>());
            this.mockDocDbDataAccess.Setup(a => a.CreateSubscriptionViewModel(It.IsAny<SubscriptionViewModel>())).ReturnsAsync(this.subscriptionViewModel);
            this.mockOutlookProvider.Setup(a => a.Subscribe(It.IsAny<SubscriptionViewModel>(), It.IsAny<string>(), false)).ReturnsAsync(this.subscriptionViewModel);
            this.mockDocDbDataAccess.Setup(a => a.UpdateSubscriptionViewModel(It.IsAny<SubscriptionViewModel>())).ReturnsAsync(this.subscriptionViewModel);
            this.mockConfiguarationManager.Setup(c => c.Get<AADClientConfiguration>()).Returns(new Mock<AADClientConfiguration>().Object);

            var updatedSubscriptionStatus = this.graphSubscriptionManager.SubscribeToInbox(this.serviceAccountToken, true);

            this.subscriptionViewModel.Id = null;
            this.mockOutlookProvider.Setup(a => a.Subscribe(It.IsAny<SubscriptionViewModel>(), It.IsAny<string>(), false)).ReturnsAsync(this.subscriptionViewModel);

            var createdSubscriptionStatus = this.graphSubscriptionManager.SubscribeToInbox(this.serviceAccountToken, true);

            Assert.AreEqual(updatedSubscriptionStatus.Result, true);
            Assert.AreEqual(createdSubscriptionStatus.Result, true);
            this.mockDocDbDataAccess.Verify(a => a.GetSystemSubscriptionViewModelByEmail(It.IsAny<string>()), Times.Exactly(2));
            this.mockDocDbDataAccess.Verify(a => a.CreateSubscriptionViewModel(It.IsAny<SubscriptionViewModel>()), Times.Once);
            this.mockDocDbDataAccess.Verify(a => a.UpdateSubscriptionViewModel(It.IsAny<SubscriptionViewModel>()), Times.Once);
            this.mockOutlookProvider.Verify(a => a.Subscribe(It.IsAny<SubscriptionViewModel>(), It.IsAny<string>(), false), Times.Exactly(2));
        }

        private SubscriptionViewModel GetMockSubscriptionViewModel()
        {
            return new SubscriptionViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Subscription = new Subscription()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };
        }
    }
}
