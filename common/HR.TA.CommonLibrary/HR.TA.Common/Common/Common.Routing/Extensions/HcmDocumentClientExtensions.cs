//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.Routing.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using HR.TA.Common.Routing.DocumentDb;
    using HR.TA.CommonDataService.Common.Internal;

    /// <summary>The HCM document client extensions.</summary>
    public static class HcmDocumentClientExtensions
    {
        /// <summary>The add HCM document client.</summary>
        /// <param name="services">The services.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDocumentClientGenerator(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            return services.AddScoped<IDocumentClientGenerator, DocumentClientGenerator>();
        }

        /// <summary>The add HCM document client.</summary>
        /// <param name="services">The services.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHcmDocumentClient(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddScoped<IDocumentClientGenerator, DocumentClientGenerator>();
            return services.AddSingleton<IDocumentClientStore, DocumentClientStore>();
        }
    }
}
