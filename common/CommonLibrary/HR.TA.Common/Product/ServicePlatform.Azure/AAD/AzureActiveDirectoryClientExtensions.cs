//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Configuration;
using HR.TA.ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace HR.TA.ServicePlatform.Azure.AAD
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
