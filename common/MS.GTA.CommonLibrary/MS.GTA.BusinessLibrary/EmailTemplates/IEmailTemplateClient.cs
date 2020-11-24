//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.BusinessLibrary.EmailTemplates
{
    using System.Threading.Tasks;

    using MS.GTA.Common.Email.Contracts;
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
