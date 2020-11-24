//----------------------------------------------------------------------------
// <copyright company="Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.Extensions
{
    using MS.GTA.Common.Web.S2SHandler;
    using MS.GTA.ServicePlatform.Azure.AAD;
    using Microsoft.Extensions.DependencyInjection;
    using MS.GTA.Common.Web.S2SHandler.V2;

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
