// <copyright file="DefaultWebNotificationBuilder_Tests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MS.GTA.CommonLibrary.UnitTest.Common.WebNotifications.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.WebNotifications;
    using MS.GTA.Common.WebNotifications.Interfaces;
    using MS.GTA.Common.WebNotifications.Internals;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DefaultWebNotificationBuilder_Tests
    {
        public IEnumerable<Dictionary<string, string>> NotificationsData { get; } = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                { NotificationConstants.Title, "Feedback Received"},
                { NotificationConstants.JobTitle, "Software Engineer" },
                { NotificationConstants.CandidateName, "Candidate 1" },
                { NotificationConstants.DeepLink, "https://www.domain.com" },
                { "InterviewerName", "Interviewer 1" },
                { "OnBehalfOfUser", "Actual Interviewer" },
                { NotificationConstants.SenderName, "Sender 1" },
                { NotificationConstants.SenderEmail, "sender1@somedomain.com" },
                { NotificationConstants.SenderObjectId, Guid.NewGuid().ToString() },
                { NotificationConstants.RecipientName, "Recipient 1" },
                { NotificationConstants.RecipientEmail, "recipient1@somedomain.com" },
                { NotificationConstants.RecipientObjectId, Guid.NewGuid().ToString() },
                { NotificationConstants.AppNotificationType, "Test Type" },
            },
            new Dictionary<string, string>
            {
                { NotificationConstants.Title, "Feedback Received"},
                { NotificationConstants.JobTitle, "Software Engineer II" },
                { NotificationConstants.CandidateName, "Candidate 2" },
                { NotificationConstants.DeepLink, "https://www.domain.com" },
                { "InterviewerName", "Interviewer 2" },
                { NotificationConstants.SenderName, "Sender 1" },
                { NotificationConstants.SenderEmail, "sender2@somedomain.com" },
                { NotificationConstants.SenderObjectId, Guid.NewGuid().ToString() },
                { NotificationConstants.RecipientName, "Recipient 2" },
                { NotificationConstants.RecipientEmail, "recipient2@somedomain.com" },
                { NotificationConstants.RecipientObjectId, Guid.NewGuid().ToString() },
                { NotificationConstants.AppNotificationType, "Test Type" },
            },
        };

        private Mock<IWebNotificationDataExtractor> notificationDataExtractorMock;

        private DefaultWebNotificationBuilder notificationBuilder;

        private Mock<IWebNotificationTemplateProvider> templateProviderMock;

        [TestInitialize]
        public void BeforeEach()
        {
            this.templateProviderMock = new Mock<IWebNotificationTemplateProvider>();
            this.notificationDataExtractorMock = new Mock<IWebNotificationDataExtractor>();
            this.notificationBuilder = new DefaultWebNotificationBuilder(NullLogger<DefaultWebNotificationBuilder>.Instance);
        }

        [TestMethod]
        public void Ctor_NullValueForLogger()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new DefaultWebNotificationBuilder(logger: null));
            Assert.AreEqual("logger", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public void Ctor_ValidLogger()
        {
            var notificationBuilder = new DefaultWebNotificationBuilder(NullLogger<DefaultWebNotificationBuilder>.Instance);
            Assert.IsInstanceOfType(notificationBuilder, typeof(DefaultWebNotificationBuilder));
        }

        [TestMethod]
        public async Task Build_NullDataExtractor()
        {
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.notificationBuilder.Build(notificationDataExtractor: null, templateProvider: this.templateProviderMock.Object).ConfigureAwait(false)).ConfigureAwait(false);
            Assert.AreEqual("notificationDataExtractor", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public async Task Build_NullTemplateProvider()
        {
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.notificationBuilder.Build(notificationDataExtractor: this.notificationDataExtractorMock.Object, templateProvider: null).ConfigureAwait(false)).ConfigureAwait(false);
            Assert.AreEqual("templateProvider", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public async Task Build_ValidInput()
        {
            this.templateProviderMock.Setup(tpm => tpm.ProvideTemplate(It.IsAny<Dictionary<string, string>>())).Returns("#InterviewerName# provided feedback on behalf of #OnBehalfOfUser# for #CandidateName# for the #JobTitle# job.");
            this.notificationDataExtractorMock.Setup(ndx => ndx.Extract()).ReturnsAsync(this.NotificationsData);
            var notifications = await this.notificationBuilder.Build(notificationDataExtractor: this.notificationDataExtractorMock.Object, templateProvider: this.templateProviderMock.Object).ConfigureAwait(false);
            Assert.AreEqual(this.NotificationsData.Count() - 1, notifications.Count());
            Assert.IsTrue(notifications.First().Body.Contains("on behalf", StringComparison.Ordinal));
            Assert.IsFalse(this.NotificationsData.First().ContainsKey(NotificationConstants.SenderObjectId));
            Assert.IsFalse(this.NotificationsData.First().ContainsKey(NotificationConstants.AppNotificationType));
            Assert.AreEqual(notifications.First().Properties[NotificationConstants.JobTitle], this.NotificationsData.First()[NotificationConstants.JobTitle], ignoreCase: false);
            Assert.IsTrue(notifications.First().Properties[NotificationConstants.DeepLink].IndexOf("ref=webnotification", StringComparison.Ordinal) > 0);
        }
    }
}
