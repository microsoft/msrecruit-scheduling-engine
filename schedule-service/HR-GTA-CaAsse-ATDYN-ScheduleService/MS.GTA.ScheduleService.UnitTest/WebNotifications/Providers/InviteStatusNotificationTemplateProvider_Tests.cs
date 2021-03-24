//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.UnitTest.WebNotifications.Roviders
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using CommonLibrary.Common.WebNotifications.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Templates.Providers;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InviteStatusNotificationTemplateProvider_Tests
    {
        private Dictionary<string, string> properties;

        private InviteStatusNotificationTemplateProvider templateProvider;

        [TestInitialize]
        public void BeforeEach()
        {
            this.properties = new Dictionary<string, string>
            {
                {WebNotificationConstants.InterviewerName, "InterviewerName" },
            };

            this.templateProvider = new InviteStatusNotificationTemplateProvider(NullLogger<InviteStatusNotificationTemplateProvider>.Instance);
        }

        [TestMethod]
        public void Ctor_NullLoggerReference()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new InviteStatusNotificationTemplateProvider(null));
            Assert.IsTrue(ex.ParamName.Equals("logger", StringComparison.Ordinal));
        }

        [TestMethod]
        public void ProvideTemplate_NullDictionary()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => this.templateProvider.ProvideTemplate(null));
            Assert.IsTrue(ex.ParamName.Equals("notificationProperties", StringComparison.Ordinal));
        }

        [TestMethod]
        public void ProvideTemplate_EmptyDictionary_ThrowsException()
        {
            var ex = Assert.ThrowsException<WebNotificationException>(() => this.templateProvider.ProvideTemplate(new Dictionary<string, string>()));
        }

        [TestMethod]
        public void ProvideTemplate_ValidDictionary()
        {
            var templateText = this.templateProvider.ProvideTemplate(this.properties);
            Assert.IsTrue(!templateText.Contains("#MessageResponse#", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("#InterviewerName#", StringComparison.Ordinal));
        }

        [TestMethod]
        public void ProvideTemplate_ValidDictionaryWithOnlyProposedTime()
        {
            var currentDate = DateTime.UtcNow.AddDays(1);
            this.properties.Add(WebNotificationConstants.ProposedStartTime, currentDate.ToString());
            this.properties.Add(WebNotificationConstants.ProposedEndTime, currentDate.AddMinutes(30).ToString());
            var templateText = this.templateProvider.ProvideTemplate(this.properties);
            Assert.IsTrue(!templateText.Contains("#MessageResponse#", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("#InterviewerName#", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("__ProposedStartTime__", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("__ProposedEndTime__", StringComparison.Ordinal));
        }

        [TestMethod]
        public void ProvideTemplate_ValidDictionaryWithMessageResponse()
        {
            this.properties.Add(WebNotificationConstants.MessageResponse, "This is message.");
            var templateText = this.templateProvider.ProvideTemplate(this.properties);
            Assert.IsTrue(templateText.Contains("#MessageResponse#", StringComparison.Ordinal));
        }

        [TestMethod]
        public void ProvideTemplate_ValidDictionaryWithMessageResponseProposedNewTime()
        {
            var currentDate = DateTime.UtcNow.AddDays(1);
            this.properties.Add(WebNotificationConstants.MessageResponse, "This is message.");
            this.properties.Add(WebNotificationConstants.ProposedStartTime, currentDate.ToString());
            this.properties.Add(WebNotificationConstants.ProposedEndTime, currentDate.AddMinutes(30).ToString());
            var templateText = this.templateProvider.ProvideTemplate(this.properties);
            Assert.IsTrue(templateText.Contains("#MessageResponse#", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("#InterviewerName#", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("__ProposedStartTime__", StringComparison.Ordinal));
            Assert.IsTrue(templateText.Contains("__ProposedEndTime__", StringComparison.Ordinal));
        }
    }
}
