//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.BusinessLibrary.EmailTemplates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Email.Contracts;
    using MS.GTA.Common.Email.Exceptions;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Data.DataAccess;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Common.Email;
    using MS.GTA.Common.Common.Common.Email;

    /// <summary>Email Template Client class</summary>
    /// <seealso cref="MS.GTA.Common.Email.EmailTemplates.IEmailTemplateClient" />
    public class EmailTemplateClient : IEmailTemplateClient
    {
        private const string LoggerPrefix = "EmailTemplateClient";

        private readonly ITraceSource trace;
        private readonly ILogger<EmailTemplateClient> logger;
        private readonly IEmailTemplateDataAccess emailTemplateDataAccess;

        public EmailTemplateClient(
            ITraceSource traceSource,
            ILogger<EmailTemplateClient> logger,
            IEmailTemplateDataAccess emailTemplateDataAccess
            )
        {
            Contract.CheckValue(traceSource, nameof(traceSource));
            Contract.CheckValue(logger, nameof(logger));
            Contract.CheckValue(emailTemplateDataAccess, nameof(emailTemplateDataAccess));

            this.trace = traceSource;
            this.logger = logger;
            this.emailTemplateDataAccess = emailTemplateDataAccess;
        }

        public Task<EmailTemplate> GetDefaultTemplate(string templateType, bool useAdminClient = false)
        {
            return this.logger.ExecuteAsync(
                $"{LoggerPrefix}_{nameof(GetDefaultTemplate)}",
                async () =>
                {
                    Contract.CheckNonEmpty(templateType, nameof(templateType));
                    this.trace.TraceInformation($"Getting regional default template for {templateType}");

                    var defaultTemplate = await this.emailTemplateDataAccess.GetDefaultTemplateForType(templateType);
                    if (defaultTemplate == null)
                    {
                        this.trace.TraceInformation($"No default template found");
                    }

                    if (defaultTemplate != null)
                    {
                        defaultTemplate.IsDefault = true;
                        await this.InjectHeaderAndFooter(defaultTemplate);
                    }

                    return defaultTemplate;
                });
        }

        public Task<IEnumerable<EmailTemplate>> GetTemplatesByType(string templateType)
        {
            return this.logger.ExecuteAsync<IEnumerable<EmailTemplate>>(
                $"{LoggerPrefix}_{nameof(GetTemplatesByType)}",
                async () =>
                {
                    Contract.CheckNonEmpty(templateType, nameof(templateType));

                    var regionalTemplatesTask = this.emailTemplateDataAccess.GetTemplatesByType(templateType);

                    var results = await Task.WhenAll(regionalTemplatesTask);
                    var allTemplates = results.SelectMany(template => template).OrderBy(t => t.TemplateName).ToList();

                    this.SetGlobalTemplatesToDefault(allTemplates);
                    await this.InjectHeaderAndFooter(allTemplates);

                    return allTemplates;
                });
        }

        public Task<IEnumerable<EmailTemplate>> GetTemplatesByAppName(string appName)
        {
            return this.logger.ExecuteAsync<IEnumerable<EmailTemplate>>(
                $"{LoggerPrefix}_{nameof(GetTemplatesByAppName)}",
                async () =>
                {
                    Contract.CheckNonEmpty(appName, nameof(appName));

                    var regionalTemplatesTask = this.emailTemplateDataAccess.GetTemplatesByAppName(appName);

                    var results = await Task.WhenAll(regionalTemplatesTask);
                    var allTemplates = results.SelectMany(template => template)
                        .OrderBy(t => t.TemplateType)
                        .ThenBy(t => t.TemplateName)
                        .ToList();

                    this.SetGlobalTemplatesToDefault(allTemplates);
                    await this.InjectHeaderAndFooter(allTemplates);

                    return allTemplates;
                });
        }

        public Task<EmailTemplate> GetTemplate(string templateId)
        {
            return this.logger.ExecuteAsync(
                $"{LoggerPrefix}_{nameof(GetTemplate)}",
                async () =>
                {
                    Contract.CheckNonEmpty(templateId, nameof(templateId));

                    var template = await this.emailTemplateDataAccess.GetTemplate(templateId);
                    if (template == null)
                    {
                        this.trace.TraceInformation($"Template {templateId} not found in database");
                        return null;
                    }

                    await this.InjectHeaderAndFooter(template);
                    return template;
                });
        }

        public Task SetDefaultTemplate(string templateType, string templateId)
        {
            return this.logger.ExecuteAsync(
                $"{LoggerPrefix}_{nameof(SetDefaultTemplate)}",
                async () =>
                {
                    Contract.CheckNonEmpty(templateType, nameof(templateType));
                    Contract.CheckNonEmpty(templateId, nameof(templateId));
                    this.trace.TraceInformation($"Setting default template for {templateType} to {templateId}");

                    var template = await this.GetTemplate(templateId);
                    if (template == null)
                    {
                        throw new EmailTemplateNotFoundException(nameof(template));
                    }

                    if (template.TemplateType != templateType)
                    {
                        throw new EmailTemplateInvalidOperationException($"Template {templateId} is not type {templateType} (has value {template.TemplateType}");
                    }

                    if (template.IsGlobal)
                    {
                        await this.emailTemplateDataAccess.ClearDefaultTemplateSettings(templateType);
                    }
                    else
                    {
                        template.IsDefault = true;
                        await this.emailTemplateDataAccess.UpdateTemplate(template);
                    }
                });
        }

        public Task<EmailTemplate> CreateTemplate(EmailTemplate emailTemplate)
        {
            return this.logger.ExecuteAsync(
                $"{LoggerPrefix}_{nameof(CreateTemplate)}",
                async () =>
                {
                    Contract.CheckValue(emailTemplate, nameof(emailTemplate));

                    emailTemplate.Id = null;
                    emailTemplate.IsGlobal = false;
                    return await this.emailTemplateDataAccess.CreateTemplate(emailTemplate);
                });
        }

        public Task<EmailTemplate> UpdateTemplate(string templateId, EmailTemplate emailTemplate)
        {
            return this.logger.ExecuteAsync(
                $"{LoggerPrefix}_{nameof(UpdateTemplate)}",
                async () =>
                {
                    Contract.CheckValue(emailTemplate, nameof(emailTemplate));
                    Contract.CheckNonEmpty(templateId, nameof(templateId));

                    var oldTemplateValue = await this.emailTemplateDataAccess.GetTemplate(templateId);
                    if (oldTemplateValue == null)
                    {
                        throw new EmailTemplateNotFoundException(templateId);
                    }

                    emailTemplate.Id = templateId;
                    emailTemplate.IsGlobal = false;
                    return await this.emailTemplateDataAccess.UpdateTemplate(emailTemplate);
                });
        }
        
        public Task<bool> DeleteTemplate(string templateId)
        {
            return this.logger.ExecuteAsync(
                $"{LoggerPrefix}_{nameof(DeleteTemplate)}",
                async () =>
                {
                    Contract.CheckNonEmpty(templateId, nameof(templateId));

                    return await this.emailTemplateDataAccess.DeleteTemplate(templateId);
                });
        }

        private void SetGlobalTemplatesToDefault(IList<EmailTemplate> templates)
        {
            if (templates == null)
            {
                return;
            }

            foreach (var globalTemplate in templates.Where(t => t.IsGlobal))
            {
                globalTemplate.IsDefault = !templates.Any(t => t.TemplateType == globalTemplate.TemplateType && t.IsDefault);
            }
        }

        private async Task InjectHeaderAndFooter(EmailTemplate template)
        {
            if (template == null)
            {
                return;
            }

            var emailSettings = await this.GetEmailTemplateSettings();

            template.Header = this.FormatHeader(emailSettings);
            template.Footer = this.FormatFooter(Constants.CompanyName, emailSettings);
        }

        private async Task InjectHeaderAndFooter(IList<EmailTemplate> templates)
        {
            if (templates == null)
            {
                return;
            }

            var emailSettings = await this.GetEmailTemplateSettings();

            string header = this.FormatHeader(emailSettings);
            string footer = this.FormatFooter(Constants.CompanyName, emailSettings);
            foreach (var template in templates)
            {
                template.Header = header;
                template.Footer = footer;
            }
        }

        private async Task<EmailTemplateSettings> GetEmailTemplateSettings()
        {
            try
            {
                return await this.emailTemplateDataAccess.GetEmailTemplateSettings();
            }
            catch (Exception e)
            {
                trace.TraceError($"Failed to get email template settings. Exception message: {e.Message}");
            }

            return null;
        }

        private string FormatHeader(EmailTemplateSettings settings)
        {
            if (string.IsNullOrEmpty(settings?.EmailTemplateHeaderImgUrl))
            {
                return string.Empty;
            }

            return EmailStrings.EmailTemplate_HeaderHtml.Replace("{Header_Image_Url}", settings.EmailTemplateHeaderImgUrl);
        }

        private string FormatFooter(string companyName, EmailTemplateSettings settings)
        {
            if (settings == null)
            {
                return string.Empty;
            }

            var footerStrings = new List<string>();
            if (!string.IsNullOrEmpty(companyName) && !string.IsNullOrEmpty(settings.EmailTemplatePrivacyPolicyLink))
            {
                footerStrings.Add(EmailStrings.EmailTemplate_PrivacyHtml
                    .Replace("{Company_Name}", companyName)
                    .Replace("{Privacy_Policy_Link}", settings.EmailTemplatePrivacyPolicyLink));
            }

            if (!string.IsNullOrEmpty(settings.EmailTemplateTermsAndConditionsLink))
            {
                footerStrings.Add(EmailStrings.EmailTemplate_TermsHtml
                    .Replace("{Terms_And_Conditions_Link}", settings.EmailTemplateTermsAndConditionsLink));
            }

            if (footerStrings.Count > 0)
            {
                return EmailStrings.EmailTemplate_FooterHtml.Replace("{Footer_Text}", string.Join(" ", footerStrings));
            }

            return string.Empty;
        }
    }

}
