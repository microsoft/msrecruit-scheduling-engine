//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Azure.AAD;
using CommonLibrary.ServicePlatform.Azure.Security;
using CommonLibrary.ServicePlatform.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ServicePlatform.Azure.Extensions
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
