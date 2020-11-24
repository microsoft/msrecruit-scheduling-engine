//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Configuration;
using MS.GTA.ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace MS.GTA.ServicePlatform.Azure.AAD
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
