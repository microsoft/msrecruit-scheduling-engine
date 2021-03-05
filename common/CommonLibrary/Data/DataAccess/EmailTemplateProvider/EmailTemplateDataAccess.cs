//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Data.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Common.Base.ServiceContext;
    using Common.DocumentDB;
    using Common.Email.Contracts;
    using Common.Routing.Client;
    using CommonDataService.Common.Internal;
    using ServicePlatform.Tracing;

    public class EmailTemplateDataAccess : IEmailTemplateDataAccess
    {
        private readonly ITraceSource trace;
        private readonly ILogger<EmailTemplateDataAccess> logger;
        private IHcmDocumentClient client;
        private readonly IHCMServiceContext hcmServiceContext;
        private IFalconQueryClient falconQueryClient { get; set; }

        public EmailTemplateDataAccess(
            ITraceSource traceSource,
            ILogger<EmailTemplateDataAccess> logger,
            IFalconQueryClient falconQueryClient,
            IHCMServiceContext hcmServiceContext
        )
        {
            Contract.CheckValue(traceSource, nameof(traceSource));
            Contract.CheckValue(logger, nameof(logger));
            Contract.CheckValue(falconQueryClient, nameof(falconQueryClient));
            Contract.CheckValue(hcmServiceContext, nameof(hcmServiceContext));

            this.trace = traceSource;
            this.logger = logger;
            this.falconQueryClient = falconQueryClient;
            this.hcmServiceContext = hcmServiceContext;
        }

        public async Task<EmailTemplate> CreateTemplate(EmailTemplate template)
        {
            this.ValidateTemplate(template);
            FalconEmailTemplate templateModel = this.ConvertFromEmailTemplate(template);

            if (template.IsDefault)
            {
                await this.ClearDefaultTemplateSettings(template.TemplateType);
            }

            if (client == null)
            {
                client = await this.falconQueryClient.GetFalconClient(hcmServiceContext.FalconCommonContainerId, hcmServiceContext.FalconDatabaseId);
            }

            FalconEmailTemplate createdTemplate = await client.Create(templateModel);

            return this.ConvertToEmailTemplate(createdTemplate);
        }

        public async Task<bool> DeleteTemplate(string templateId)
        {
            Contract.CheckNonEmpty(templateId, nameof(templateId));
            this.trace.TraceInformation($"{LoggerPrefix}: Attempting to delete template {templateId}");

            var template = await this.GetTemplate(templateId);
            if (template == null)
            {
                this.trace.TraceInformation($"{LoggerPrefix}: Template {templateId} does not exist. There is nothing to delete");
                return false;
            }

            this.trace.TraceInformation($"{LoggerPrefix}: Deleting template of type {template.TemplateType}, with isDefault = {template.IsDefault}");

            await client.Delete<FalconEmailTemplate>(templateId);

            return true;
        }

        public async Task<EmailTemplate> GetTemplate(string templateId)
        {
            Contract.CheckNonEmpty(templateId, nameof(templateId));

            if (client == null)
            {
                client = await this.falconQueryClient.GetFalconClient(hcmServiceContext.FalconCommonContainerId, hcmServiceContext.FalconDatabaseId);
            }

            FalconEmailTemplate templateModel = await client.Get<FalconEmailTemplate>(templateId);

            return this.ConvertToEmailTemplate(templateModel);
        }

        public async Task<IEnumerable<EmailTemplate>> GetTemplatesByAppName(string appName)
        {
            Contract.CheckNonEmpty(appName, nameof(appName));
            this.trace.TraceInformation($"{LoggerPrefix}: Getting templates for appName = {appName}");

            if (client == null)
            {
                client = await this.falconQueryClient.GetFalconClient(hcmServiceContext.FalconCommonContainerId, hcmServiceContext.FalconDatabaseId);
            }

            var templateModels = await this.QueryTemplatesByAppName(client, appName);

            return templateModels?.Select(t => this.ConvertToEmailTemplate(t));
        }

        public async Task<IEnumerable<EmailTemplate>> GetTemplatesByType(string templateType)
        {
            Contract.CheckNonEmpty(templateType, nameof(templateType));
            this.trace.TraceInformation($"{LoggerPrefix}: Getting templates for templateType = {templateType}");

            if (client == null)
            {
                client = await this.falconQueryClient.GetFalconClient(hcmServiceContext.FalconCommonContainerId, hcmServiceContext.FalconDatabaseId);
            }

            var templateModels = await this.QueryTemplatesByTemplateType(client, templateType);

            return templateModels?.Select(t => this.ConvertToEmailTemplate(t));
        }

        public async Task<EmailTemplate> UpdateTemplate(EmailTemplate template)
        {
            this.ValidateTemplate(template);

            if (template.IsDefault)
            {
                await this.ClearDefaultTemplateSettings(template.TemplateType);
            }

            FalconEmailTemplate templateModel = this.ConvertFromEmailTemplate(template);

            if (client == null)
            {
                client = await this.falconQueryClient.GetFalconClient(hcmServiceContext.FalconCommonContainerId, hcmServiceContext.FalconDatabaseId);
            }

            FalconEmailTemplate updatedTemplate = await client.Update<FalconEmailTemplate>(templateModel);

            return this.ConvertToEmailTemplate(updatedTemplate);
        }

        public async Task<EmailTemplate> GetDefaultTemplateForType(string templateType)
        {
            Contract.CheckNonEmpty(templateType, nameof(templateType));
            this.trace.TraceInformation($"{LoggerPrefix}: Getting default template setting for type {templateType}");

            var templates = await this.GetTemplatesByType(templateType);
            return templates?.FirstOrDefault(t => t.IsDefault);
        }

        public async Task ClearDefaultTemplateSettings(string templateType)
        {
            Contract.CheckNonEmpty(templateType, nameof(templateType));
            this.trace.TraceInformation($"{LoggerPrefix}: Setting IsDefault to false for all templates of type {templateType}");

            var templates = await this.GetTemplatesByType(templateType);

            // There should be only one task with update set to true, deleting all as a sanity check
            List<Task> updateTasks = new List<Task>();
            foreach (var template in templates?.Where(t => t.IsDefault) ?? Enumerable.Empty<EmailTemplate>())
            {
                template.IsDefault = false;
                updateTasks.Add(this.UpdateTemplate(template));
            }

            await Task.WhenAll(updateTasks);
        }

        public async Task<EmailTemplateSettings> GetEmailTemplateSettings()
        {
            this.trace.TraceInformation($"{LoggerPrefix}: GetEmailTemplateSettings");

            if (client == null)
            {
                client = await this.falconQueryClient.GetFalconClient(hcmServiceContext.FalconCommonContainerId, hcmServiceContext.FalconDatabaseId);
            }

            Talent.FalconEntities.Common.EmailTemplateSettings emailTemplateSettings = await client.GetFirstOrDefault<Talent.FalconEntities.Common.EmailTemplateSettings>(f => f.Type == "EmailTemplateSettings");

            return this.ConvertToEmailTemplateSettings(emailTemplateSettings);
        }

        private EmailTemplateSettings ConvertToEmailTemplateSettings(Talent.FalconEntities.Common.EmailTemplateSettings emailTemplateSettingsEntity)
        {
            if (emailTemplateSettingsEntity == null)
            {
                return null;
            }

            return new EmailTemplateSettings
            {
                EmailTemplateHeaderImgUrl = emailTemplateSettingsEntity.EmailTemplateHeaderImgUrl,
                EmailTemplatePrivacyPolicyLink = emailTemplateSettingsEntity.EmailTemplatePrivacyPolicyLink,
                EmailTemplateTermsAndConditionsLink = emailTemplateSettingsEntity.EmailTemplateTermsAndConditionsLink,
                ShouldDisableEmailEdits = emailTemplateSettingsEntity.ShouldDisableEmailEdits
            };
        }

        private string LoggerPrefix { get; } = "FalconEmailTemplates";

        /* In the different database contexts, we use slightly different models for saving the data.
         * These methods allow us to share a common exposed data model with the apps and translate
         * that into the format actually used in the end storage. */
        private void ValidateTemplate(EmailTemplate template)
        {
            Contract.CheckValue(template, nameof(template));
            Contract.CheckNonEmpty(template.AppName, $"{nameof(template)}.{nameof(template.AppName)}");
            Contract.CheckNonEmpty(template.TemplateType, $"{nameof(template)}.{nameof(template.TemplateType)}");
            Contract.CheckNonEmpty(template.TemplateName, $"{nameof(template)}.{nameof(template.TemplateName)}");
        }

        private EmailTemplate ConvertToEmailTemplate(FalconEmailTemplate templateModel)
        {
            if (templateModel == null)
            {
                return null;
            }

            return new EmailTemplate
            {
                Id = templateModel.Id,
                TemplateName = templateModel.TemplateName,
                AppName = templateModel.AppName,
                TemplateType = templateModel.TemplateType,
                IsGlobal = false,
                To = templateModel.To,
                Cc = templateModel.Cc,
                AdditionalCc = templateModel.AdditionalCc,
                Bcc = templateModel.Bcc,
                Subject = templateModel.Subject,
                Header = templateModel.Header,
                Body = templateModel.Body,
                Closing = templateModel.Closing,
                Footer = templateModel.Footer,
                IsDefault = templateModel.IsDefault,
                Creator = templateModel.CreatedBy,
                Language = templateModel.Language,
            };
        }

        private FalconEmailTemplate ConvertFromEmailTemplate(EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                return null;
            }

            return new FalconEmailTemplate
            {
                Id = emailTemplate.Id,
                TemplateName = emailTemplate.TemplateName,
                AppName = emailTemplate.AppName,
                TemplateType = emailTemplate.TemplateType,
                To = emailTemplate.To,
                Cc = emailTemplate.Cc,
                AdditionalCc = emailTemplate.AdditionalCc,
                Bcc = emailTemplate.Bcc,
                Subject = emailTemplate.Subject,
                Header = emailTemplate.Header,
                Body = emailTemplate.Body,
                Closing = emailTemplate.Closing,
                Footer = emailTemplate.Footer,
                IsDefault = emailTemplate.IsDefault,
                CreatedBy = emailTemplate.Creator,
                Language = emailTemplate.Language,
            };
        }

        /* Normal linq queries cannot be written at this level.  Queries written here operate on the interface
         * and not the concrete types, so they cannot access the proper DataMember Name of the concrete types.
         * These methods delegate those operations to the concrete implementations. */

        private Task<IEnumerable<FalconEmailTemplate>> QueryTemplatesByAppName(IHcmDocumentClient documentClient, string appName)
            => documentClient.Get<FalconEmailTemplate>(t => t.AppName == appName);

        private Task<IEnumerable<FalconEmailTemplate>> QueryTemplatesByTemplateType(IHcmDocumentClient documentClient, string templateType)
            => documentClient.Get<FalconEmailTemplate>(t => t.TemplateType == templateType);
    }
}
