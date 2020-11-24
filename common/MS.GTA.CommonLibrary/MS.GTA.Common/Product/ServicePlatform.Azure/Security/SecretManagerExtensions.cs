//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Azure.AAD;
using MS.GTA.ServicePlatform.Azure.Security;
using MS.GTA.ServicePlatform.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MS.GTA.ServicePlatform.Azure.Extensions
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
