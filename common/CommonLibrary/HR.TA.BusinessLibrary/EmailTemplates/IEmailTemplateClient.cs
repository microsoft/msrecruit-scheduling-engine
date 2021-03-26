//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.BusinessLibrary.EmailTemplates
{
    using System.Threading.Tasks;

    using HR.TA.Common.Email.Contracts;
    using System.Collections.Generic;

    public interface IEmailTemplateClient
    {
        Task<IEnumerable<EmailTemplate>> GetTemplatesByType(string templateType);

        Task<IEnumerable<EmailTemplate>> GetTemplatesByAppName(string appName);

        Task<EmailTemplate> GetTemplate(string templateId);

        Task<EmailTemplate> GetDefaultTemplate(string templateType, bool useAdminClient = false);

        Task SetDefaultTemplate(string templateType, string templateId);

        Task<EmailTemplate> CreateTemplate(EmailTemplate emailTemplate);

        Task<EmailTemplate> UpdateTemplate(string id, EmailTemplate emailTemplate);

        Task<bool> DeleteTemplate(string id);
    }
}
