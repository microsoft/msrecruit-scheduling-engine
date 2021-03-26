//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Routing.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using TA.CommonLibrary.Common.BapClient.Extensions;
    using TA.CommonLibrary.Common.Base.Security.V2;
    using TA.CommonLibrary.Common.Base.ServiceContext;
    using TA.CommonLibrary.Common.Routing;
    using TA.CommonLibrary.Common.Routing.GlobalService;
    using TA.CommonLibrary.CommonDataService.Common.Internal;


    /// <summary> Routing Extension class </summary>
    public static class RoutingExtensions
    {
        /// <summary>The add tenant resource manager.</summary>
        /// <param name="services">The services.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTenantResourceManager(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddBapClient(); //TODO: Cleanup the BAP references
            services.AddPrincipalRetriever();
            services.AddScoped<IHCMServiceContext, HCMServiceContext>();
            services.AddSingleton<IGlobalServiceClientProvider, GlobalServiceClientProvider>();
            services.AddSingleton<IRoutingClient, RoutingClient>(provider =>
            {
                IGlobalServiceClientProvider globalServiceClientProvider = provider.GetRequiredService<IGlobalServiceClientProvider>();
                return globalServiceClientProvider.GetSingletonRoutingClient();
            });
            services.AddScoped<ITenantResourceManager, TenantResourceManager>();

            return services;
        }

        /// <summary>The add global service management client.</summary>
        /// <param name="services">The services.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddGlobalServiceManagementClient(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddTenantResourceManager();
            services.AddSingleton<IGlobalServiceManagementClient, GlobalServiceManagementClient>(provider =>
            {
                IGlobalServiceClientProvider globalServiceClientProvider = provider.GetRequiredService<IGlobalServiceClientProvider>();
                return globalServiceClientProvider.GetSingletonManagementClient();
            });

            return services;
        }
    }
}
