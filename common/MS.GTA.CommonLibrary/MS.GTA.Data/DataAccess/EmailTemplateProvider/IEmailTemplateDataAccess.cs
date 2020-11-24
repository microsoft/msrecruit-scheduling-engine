//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Data.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.Common.Email.Contracts;

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