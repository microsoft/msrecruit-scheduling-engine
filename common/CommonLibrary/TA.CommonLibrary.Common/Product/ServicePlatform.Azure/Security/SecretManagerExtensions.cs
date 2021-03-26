//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Azure.AAD;
using TA.CommonLibrary.ServicePlatform.Azure.Security;
using TA.CommonLibrary.ServicePlatform.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TA.CommonLibrary.ServicePlatform.Azure.Extensions
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
