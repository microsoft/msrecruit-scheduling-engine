//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.CommonLibrary.UnitTest.Common.Common.Email.EmailTemplates
{
    using Microsoft.Azure.Documents;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using HR.TA.Data.DataAccess;
    using HR.TA.Common.DocumentDB;
    using HR.TA.Common.Email.Contracts;
    using HR.TA.Common.Routing.Client;
    using HR.TA.ServicePlatform.Tracing;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System;
    using HR.TA.Common.Base.ServiceContext;

    [TestClass]
    public class EmailTemplateDataAccessTest
    {
        private readonly Mock<ITraceSource> traceMock;
        private readonly Mock<ILogger<EmailTemplateDataAccess>> loggerMock;
        private readonly EmailTemplateDataAccess emailTemplateDataAccess;
        private readonly Mock<IFalconQueryClient> falconQueryClientMock;
        private readonly Mock<IDocumentClient> documentClientMock;
        private readonly Mock<IHcmDocumentClient> hcmDocumentClient;
        private readonly Mock<IHCMServiceContext> hcmServiceContextMock;

        public EmailTemplateDataAccessTest()
        {
            traceMock = new Mock<ITraceSource>();
            loggerMock = new Mock<ILogger<EmailTemplateDataAccess>>();
            falconQueryClientMock = new Mock<IFalconQueryClient>();
            documentClientMock = new Mock<IDocumentClient>();
            hcmDocumentClient = new Mock<IHcmDocumentClient>();
            hcmServiceContextMock = new Mock<IHCMServiceContext>();
            falconQueryClientMock.Setup(x => x.GetFalconClient(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hcmDocumentClient.Object);
            emailTemplateDataAccess = new EmailTemplateDataAccess(traceMock.Object, loggerMock.Object, falconQueryClientMock.Object, hcmServiceContextMock.Object);
        }

        [TestMethod]
        public void CreateTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<string>(), null)).ReturnsAsync(falconEmailTemplate);
            var result = emailTemplateDataAccess.CreateTemplate(emailTemplate);

            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void DeleteTemplateTest()
        {
            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<string>(), null)).ReturnsAsync(falconEmailTemplate);
            var result = emailTemplateDataAccess.DeleteTemplate("templateId");

            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public void DeleteTemplateNullTemplateTest()
        {
            FalconEmailTemplate falconEmailTemplate = null;

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<string>(), null)).ReturnsAsync(falconEmailTemplate);
            var result = emailTemplateDataAccess.DeleteTemplate("templateId");

            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public void GetTemplateTest()
        {
            var result = emailTemplateDataAccess.GetTemplate("templateId");

            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void GetTemplatesByAppNameTest()
        {
            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            List<FalconEmailTemplate> falconEmailTemplates = new List<FalconEmailTemplate>();
            falconEmailTemplates.Add(falconEmailTemplate);

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<Expression<Func<FalconEmailTemplate, bool>>>(), null)).ReturnsAsync(falconEmailTemplates);
            var result = emailTemplateDataAccess.GetTemplatesByAppName("templateId");

            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void GetTemplatesByTypeTest()
        {
            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            List<FalconEmailTemplate> falconEmailTemplates = new List<FalconEmailTemplate>();
            falconEmailTemplates.Add(falconEmailTemplate);

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<Expression<Func<FalconEmailTemplate, bool>>>(), null)).ReturnsAsync(falconEmailTemplates);
            var result = emailTemplateDataAccess.GetTemplatesByType("templateId");

            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void UpdateTemplateTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            List<FalconEmailTemplate> falconEmailTemplates = new List<FalconEmailTemplate>();
            falconEmailTemplates.Add(falconEmailTemplate);

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<Expression<Func<FalconEmailTemplate, bool>>>(), null)).ReturnsAsync(falconEmailTemplates);
            var result = emailTemplateDataAccess.UpdateTemplate(emailTemplate);

            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void GetDefaultTemplateForTypeTest()
        {
            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<string>(), null)).ReturnsAsync(falconEmailTemplate);
            var result = emailTemplateDataAccess.GetDefaultTemplateForType("templateId");

            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void ClearDefaultTemplateSettingsTest()
        {
            EmailTemplate emailTemplate = new EmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                IsGlobal = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            FalconEmailTemplate falconEmailTemplate = new FalconEmailTemplate
            {
                AppName = "Attract",
                Body = "Body",
                IsDefault = true,
                TemplateType = "templateType",
                TemplateName = "Name"
            };

            List<FalconEmailTemplate> falconEmailTemplates = new List<FalconEmailTemplate>();
            falconEmailTemplates.Add(falconEmailTemplate);

            hcmDocumentClient.Setup(x => x.Get<FalconEmailTemplate>(It.IsAny<Expression<Func<FalconEmailTemplate, bool>>>(), null)).ReturnsAsync(falconEmailTemplates);
            var result = emailTemplateDataAccess.ClearDefaultTemplateSettings("templateType");

            hcmDocumentClient.Verify();
        }

        [TestMethod]
        public void GetEmailTemplateSettingsTest()
        {
            Talent.FalconEntities.Common.EmailTemplateSettings emailTemplateSettings = new Talent.FalconEntities.Common.EmailTemplateSettings
            {
                EmailTemplateHeaderImgUrl = "imageUrl",
                EmailTemplatePrivacyPolicyLink = "policyLink",
                EmailTemplateTermsAndConditionsLink = "TnCLink",
                ShouldDisableEmailEdits = false
            };

            hcmDocumentClient.Setup(x => x.GetFirstOrDefault<Talent.FalconEntities.Common.EmailTemplateSettings>(It.IsAny<Expression<Func<Talent.FalconEntities.Common.EmailTemplateSettings, bool>>>(), null)).ReturnsAsync(emailTemplateSettings);
            var result = emailTemplateDataAccess.GetEmailTemplateSettings();

            Assert.IsNotNull(result.Result);
        }
    }

}
