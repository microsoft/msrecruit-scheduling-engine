//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonDataService.Common.Internal;
using ServicePlatform.Azure.AAD;
using ServicePlatform.Azure.Security;
using ServicePlatform.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePlatform.Azure.Extensions
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
