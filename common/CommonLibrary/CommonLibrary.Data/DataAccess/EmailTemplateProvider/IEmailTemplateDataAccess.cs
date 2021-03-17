//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Data.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CommonLibrary.Common.Email.Contracts;

    public interface IEmailTemplateDataAccess
    {
        Task<IEnumerable<EmailTemplate>> GetTemplatesByType(string templateType);

        Task<IEnumerable<EmailTemplate>> GetTemplatesByAppName(string appName);

        Task<EmailTemplate> GetDefaultTemplateForType(string templateType);

        Task ClearDefaultTemplateSettings(string templateType);

        Task<EmailTemplate> GetTemplate(string templateId);

        Task<EmailTemplate> UpdateTemplate(EmailTemplate template);

        Task<EmailTemplate> CreateTemplate(EmailTemplate template);

        Task<bool> DeleteTemplate(string templateId);

        Task<EmailTemplateSettings> GetEmailTemplateSettings();
    }
}
