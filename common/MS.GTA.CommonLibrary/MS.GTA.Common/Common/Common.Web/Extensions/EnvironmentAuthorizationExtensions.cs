//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Web.Extensions
{
    using Common.Web.S2SHandler;
    using ServicePlatform.Azure.AAD;
    using Microsoft.Extensions.DependencyInjection;
    using Common.Web.S2SHandler.V2;

    public static class EnvironmentAuthorizationExtensions
    {
        public static IServiceCollection SetupEnvironmentAuthorization(this IServiceCollection services)
        {
            return services
                .AddS2SHandler()
                .AddSingleton<IAzureActiveDirectoryClient, AzureActiveDirectoryClient>();
        }

        public static IServiceCollection AddS2SHandler(this IServiceCollection services)
        {
            return services
                .AddScoped<IS2SHandler, S2SHandler>();
        }
    }
}
