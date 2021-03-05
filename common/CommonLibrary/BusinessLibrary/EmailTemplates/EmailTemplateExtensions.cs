//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace BusinessLibrary.EmailTemplates
{
    using CommonDataService.Common.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Data.DataAccess;

    /// <summary>Email Template Extensions class</summary>
    public static class EmailTemplateExtensions
    {
        public static IServiceCollection AddEmailTemplateClient(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddScoped<IEmailTemplateDataAccess, EmailTemplateDataAccess>();
            services.AddScoped<IEmailTemplateClient, EmailTemplateClient>();

            return services;
        }
    }
}
