//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.CommonLibrary.UnitTest.Common.Common.Email.EmailTemplates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Email.Contracts;
    using MS.GTA.BusinessLibrary.EmailTemplates;
    using MS.GTA.Data.DataAccess;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class EmailTemplateClientTest
    {
        private readonly Mock<ITraceSource> traceMock;
        private readonly Mock<ILogger<EmailTemplateClient>> loggerMock;
        private readonly Mock<IEmailTemplateDataAccess> emailTemplateDataAccessMock;
        private readonly EmailTemplateClient emailTemplateClient;
        public EmailTemplateClientTest()
        {
            traceMock = new Mock<ITraceSource>();
            loggerMock = new Mock<ILogger<EmailTemplateClient>>();
            emailTemplateDataAccessMock = new Mock<IEmailTemplateDataAccess>();
            emailTemplateClient = new EmailTemplateClient(traceMock.Object, loggerMock.Object, emailTemplateDataAccessMock.Object);
        }

        [TestMethod]
        public void GetDefaultTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            EmailTemplateSettings emailTemplateSettings = new EmailTemplateSettings
            {
                EmailTemplateHeaderImgUrl = "imageUrl",
                EmailTemplatePrivacyPolicyLink = "policyLink",
                EmailTemplateTermsAndConditionsLink = "TnCLink",
                ////Id = "EmailTemplateSettings",
                ShouldDisableEmailEdits = false
            };

            emailTemplateDataAccessMock.Setup(x => x.GetDefaultTemplateForType(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.GetEmailTemplateSettings()).ReturnsAsync(emailTemplateSettings);
            var result = emailTemplateClient.GetDefaultTemplate("templateType");

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void GetDefaultTemplateTrueAdminTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            EmailTemplateSettings emailTemplateSettings = new EmailTemplateSettings
            {
                EmailTemplateHeaderImgUrl = "imageUrl",
                EmailTemplatePrivacyPolicyLink = "policyLink",
                EmailTemplateTermsAndConditionsLink = "TnCLink",
                ////Id = "EmailTemplateSettings",
                ShouldDisableEmailEdits = false
            };

            emailTemplateDataAccessMock.Setup(x => x.GetDefaultTemplateForType(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.GetEmailTemplateSettings()).ReturnsAsync(emailTemplateSettings);
            var result = emailTemplateClient.GetDefaultTemplate("templateType", true);

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void GetDefaultTemplateEmptyEmailSettingsTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            EmailTemplateSettings emailTemplateSettings = new EmailTemplateSettings();

            emailTemplateDataAccessMock.Setup(x => x.GetDefaultTemplateForType(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.GetEmailTemplateSettings()).ReturnsAsync(emailTemplateSettings);
            var result = emailTemplateClient.GetDefaultTemplate("templateType");

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void GetDefaultTemplateEmailSettingsExceptionTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            EmailTemplateSettings emailTemplateSettings = new EmailTemplateSettings
            {
                EmailTemplateHeaderImgUrl = "imageUrl",
                EmailTemplatePrivacyPolicyLink = "policyLink",
                EmailTemplateTermsAndConditionsLink = "TnCLink",
                ////Id = "EmailTemplateSettings",
                ShouldDisableEmailEdits = false
            };

            emailTemplateDataAccessMock.Setup(x => x.GetDefaultTemplateForType(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.GetEmailTemplateSettings()).ThrowsAsync(new Exception("Not found"));
            var result = emailTemplateClient.GetDefaultTemplate("templateType");

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void GetTemplatesByTypeTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            emailTemplates.Add(emailTemplate);

            emailTemplateDataAccessMock.Setup(x => x.GetTemplatesByType(It.IsAny<string>())).ReturnsAsync(emailTemplates);
            var result = emailTemplateClient.GetTemplatesByType("templateType");

            Assert.AreEqual(emailTemplates.Count, result.Result.Count());
        }

        [TestMethod]
        public void GetTemplatesByAppNameTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            emailTemplates.Add(emailTemplate);

            emailTemplateDataAccessMock.Setup(x => x.GetTemplatesByAppName(It.IsAny<string>())).ReturnsAsync(emailTemplates);
            var result = emailTemplateClient.GetTemplatesByAppName("templateType");

            Assert.AreEqual(emailTemplates.Count, result.Result.Count());
        }

        [TestMethod]
        public void GetTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            emailTemplateDataAccessMock.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            var result = emailTemplateClient.GetTemplate("templateType");

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void SetDefaultTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true,
                TemplateType = "templateType"
            };

            emailTemplateDataAccessMock.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.ClearDefaultTemplateSettings(It.IsAny<string>())).Returns(Task.CompletedTask);
            var result = emailTemplateClient.SetDefaultTemplate("templateType", "templateId");

            emailTemplateDataAccessMock.VerifyAll();
        }

        [TestMethod]
        public void SetDefaultTemplateNotGlobalTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = false,
                TemplateType = "templateType"
            };

            emailTemplateDataAccessMock.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.UpdateTemplate(It.IsAny<EmailTemplate>())).ReturnsAsync(emailTemplate);
            var result = emailTemplateClient.SetDefaultTemplate("templateType", "templateId");

            emailTemplateDataAccessMock.VerifyAll();
        }

        [TestMethod]
        public void SetDefaultTemplateThrowsExceptionTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = false,
                TemplateType = "templateTypeInvalid"
            };

            emailTemplateDataAccessMock.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.UpdateTemplate(It.IsAny<EmailTemplate>())).ReturnsAsync(emailTemplate);
            var result = emailTemplateClient.SetDefaultTemplate("templateType", "templateId");

            Assert.AreEqual(1, result.Exception.InnerExceptions.Count);
        }

        [TestMethod]
        public void SetDefaultTemplateThrowsExceptionNullTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = false,
                TemplateType = "templateTypeInvalid"
            };

            emailTemplateDataAccessMock.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync((EmailTemplate)null);
            var result = emailTemplateClient.SetDefaultTemplate("templateType", "templateId");

            Assert.AreEqual(1, result.Exception.InnerExceptions.Count);
        }

        [TestMethod]
        public void CreateRegionalTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            emailTemplateDataAccessMock.Setup(x => x.CreateTemplate(It.IsAny<EmailTemplate>())).ReturnsAsync(emailTemplate);
            var result = emailTemplateClient.CreateTemplate(emailTemplate);

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void UpdateRegionalTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true
            };

            emailTemplateDataAccessMock.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate);
            emailTemplateDataAccessMock.Setup(x => x.UpdateTemplate(It.IsAny<EmailTemplate>())).ReturnsAsync(emailTemplate);
            var result = emailTemplateClient.UpdateTemplate("templateId", emailTemplate);

            Assert.AreEqual(emailTemplate, result.Result);
        }

        [TestMethod]
        public void DeleteRegionalTemplateTest()
        {
            emailTemplateDataAccessMock.Setup(x => x.DeleteTemplate(It.IsAny<string>())).ReturnsAsync(true);
            var result = emailTemplateClient.DeleteTemplate("templateId");

            Assert.IsTrue(result.Result);
        }
    }
}
