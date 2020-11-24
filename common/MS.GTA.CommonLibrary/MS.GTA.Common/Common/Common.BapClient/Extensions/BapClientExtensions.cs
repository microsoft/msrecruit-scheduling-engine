// //----------------------------------------------------------------------------
// // <copyright company="Microsoft Corporation" file="BapClientExtensions.cs">
// // Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// // </copyright>
// //----------------------------------------------------------------------------
namespace MS.GTA.Common.BapClient.Extensions
{
    using CommonDataService.Common.Internal;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary> BAP Client Extension class </summary>
    public static class BapClientExtensions
    {
        /// <summary>
        /// Adds the bap client
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddBapClient(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddSingleton<IBapHttpRetriever, BapHttpRetriever>();
            services.AddSingleton<IBapServiceClient, BapServiceClient>();

            return services;
        }
    }
}
