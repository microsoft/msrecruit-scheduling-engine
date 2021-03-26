//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Azure.AAD;
using HR.TA.ServicePlatform.Azure.Security;
using HR.TA.ServicePlatform.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.TA.ServicePlatform.Azure.Extensions
{
    public static class SecretManagerExtensions
    {
        public static void AddSecretManager(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddSingleton<ISecretManager>(serviceProvider =>
            {
                var azureActiveDirectoryClient = serviceProvider.GetRequiredService<IAzureActiveDirectoryClient>();
                var configurationManager = serviceProvider.GetRequiredService<IConfigurationManager>();

                return new SecretManager(azureActiveDirectoryClient, configurationManager);
            });
        }
    }
}
