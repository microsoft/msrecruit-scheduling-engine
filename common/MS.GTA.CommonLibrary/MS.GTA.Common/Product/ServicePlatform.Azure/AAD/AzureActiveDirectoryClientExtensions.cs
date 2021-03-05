//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonDataService.Common.Internal;
using ServicePlatform.Configuration;
using ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePlatform.Azure.AAD
{
    public static class AzureActiveDirectoryClientExtensions
    {
        public static void AddAzureActiveDirectoryClient(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddSingleton<IAzureActiveDirectoryClient>(serviceProvider =>
            {
                var configurationManager = serviceProvider.GetRequiredService<IConfigurationManager>();
                var certificateManager = serviceProvider.GetRequiredService<ICertificateManager>();

                return new AzureActiveDirectoryClient(configurationManager, certificateManager);
            });
        }
    }
}
