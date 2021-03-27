//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.ScheduleService.UnitTest.Business
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Common.WebNotifications.Interfaces;
    using HR.TA.Data.DataAccess;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Providers;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.Data.Models;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Tracing;

    [TestClass]
    public class ProcessNotificationTest
    {
        private readonly ILoggerFactory loggerFactory = new LoggerFactory();
        private readonly string subscriptionId = Guid.NewGuid().ToString();
        private readonly string clientState = Guid.NewGuid().ToString();
        private Mock<IDocDbDataAccess> mockDocDbDataAccess;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IOutlookProvider> mockOutlookProvider;
        private Mock<IScheduleQuery> mockScheduleQuery;
        private Mock<ITraceSource> mockTraceSource;
        private Mock<INotificationClient> mockNotificationClient;
        private Mock<IEmailTemplateDataAccess> mockEmailTemplateDataAccess;
        private Mock<IConfigurationManager> mockConfiguarationManager;
        private Mock<ILogger<NotificationManager>> loggerMock;
        private Mock<IEmailManager> mockEmailManager;
        private Mock<IWebNotificationBuilderClient> webNotificationsBuilderClientMock;
        private Mock<IWebNotificationInternalsProvider> webNotificationINternalsProviderMock;

        [TestInitialize]
        public void BeforEach()
        {
            this.webNotificationsBuilderClientMock = new Mock<IWebNotificationBuilderClient>();
            this.webNotificationINternalsProviderMock = new Mock<IWebNotificationInternalsProvider>();
            this.mockDocDbDataAccess = new Mock<IDocDbDataAccess>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockScheduleQuery = new Mock<IScheduleQuery>();
            this.mockOutlookProvider = new Mock<IOutlookProvider>();
            this.mockTraceSource = new Mock<ITraceSource>();
            this.mockNotificationClient = new Mock<INotificationClient>();
            this.mockEmailTemplateDataAccess = new Mock<IEmailTemplateDataAccess>();
            this.mockConfiguarationManager = new Mock<IConfigurationManager>();
            this.mockEmailManager = new Mock<IEmailManager>();
            this.loggerMock = new Mock<ILogger<NotificationManager>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// Test ProcessNotification With InvalidInputs
        /// </summary>
        [TestMethod]
        public void TestProcessNotificationWithInvalidInputs()
        {
            var notificationManager = new NotificationManager(this.mockDocDbDataAccess.Object, this.mockEmailClient.Object, this.mockOutlookProvider.Object, this.mockScheduleQuery.Object, this.mockNotificationClient.Object, this.mockConfiguarationManager.Object, this.mockEmailTemplateDataAccess.Object, this.mockEmailManager.Object, this.webNotificationINternalsProviderMock.Object, this.webNotificationsBuilderClientMock.Object, this.loggerMock.Object);

            var notificationContentNullException = notificationManager.ProcessNotificationContent(null);

            Assert.IsInstanceOfType(notificationContentNullException.Exception.InnerException, typeof(InvalidRequestDataValidationException));
        }

        /// <summary>
        /// Test ProcessNotification With InvalidInputs
        /// </summary>
        [TestMethod]
        public void TestProcessNotificationWithValidInputs()
        {
            var notificationContents = this.GetNotificationContent();
            var notificationManager = new NotificationManager(this.mockDocDbDataAccess.Object, this.mockEmailClient.Object, this.mockOutlookProvider.Object, this.mockScheduleQuery.Object, this.mockNotificationClient.Object, this.mockConfiguarationManager.Object, this.mockEmailTemplateDataAccess.Object, this.mockEmailManager.Object, this.webNotificationINternalsProviderMock.Object, this.webNotificationsBuilderClientMock.Object, this.loggerMock.Object);
            this.mockDocDbDataAccess.Setup(a => a.GetSystemSubscriptionViewModelByIds(It.IsAny<List<string>>())).ReturnsAsync(this.GetSubscriptionViewModel());
            this.mockEmailClient.Setup(a => a.GetServiceAccountTokenByEmail(It.IsAny<string>())).ReturnsAsync(Guid.NewGuid().ToString());
            this.mockOutlookProvider.Setup(a => a.GetMessageById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Message());
            this.mockScheduleQuery.Setup(a => a.UpdateScheduleWithResponse(It.IsAny<Message>(), It.IsAny<Microsoft.Graph.User>(), It.IsAny<Microsoft.Graph.User>())).ReturnsAsync(new InterviewerInviteResponseInfo());
            this.mockOutlookProvider.Setup(a => a.SearchUserByEmail(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new GraphUserResponse());
            var notification = notificationManager.ProcessNotificationContent(notificationContents);

            Assert.IsNotNull(notification);
            this.mockDocDbDataAccess.Verify(a => a.GetSystemSubscriptionViewModelByIds(It.IsAny<List<string>>()), Times.Once);
            this.mockEmailClient.Verify(a => a.GetServiceAccountTokenByEmail(It.IsAny<string>()), Times.Once);
            this.mockOutlookProvider.Verify(a => a.GetMessageById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        private List<SubscriptionViewModel> GetSubscriptionViewModel()
        {
            return new List<SubscriptionViewModel>()
            {
                new SubscriptionViewModel()
                {
                     Id = this.subscriptionId,
                     Subscription = new Subscription()
                     {
                         Id = this.subscriptionId,
                         ClientState = this.clientState
                     },
                     ServiceAccountEmail = Guid.NewGuid().ToString()
                }
            };
        }

        private NotificationContent GetNotificationContent()
        {
            return new NotificationContent()
            {
                Value = new List<Notification>()
                {
                    new Notification()
                    {
                        SubscriptionId = this.subscriptionId,
                        ChangeType = "updated",
                        ClientState = this.clientState,
                        ResourceData = new ResourceData()
                        {
                            ODataType = "#Microsoft.Graph.Message"
                        }
                    }
                }
            };
        }
    }
}
