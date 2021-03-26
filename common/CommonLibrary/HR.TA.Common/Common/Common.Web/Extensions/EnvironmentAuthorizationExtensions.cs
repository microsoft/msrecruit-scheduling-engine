//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Web.Extensions
{
    using HR.TA.Common.Web.S2SHandler;
    using HR.TA.ServicePlatform.Azure.AAD;
    using Microsoft.Extensions.DependencyInjection;
    using HR.TA.Common.Web.S2SHandler.V2;

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
