// //----------------------------------------------------------------------------
// // <copyright company="Microsoft Corporation" file="SecretManagerExtensions.cs">
// // Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// // </copyright>
// //----------------------------------------------------------------------------
namespace MS.GTA.Common.Base.Utilities
{
    using CommonDataService.Common.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using ServicePlatform.AspNetCore.Security;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Azure.Extensions;

    /// <summary>
    /// Secret Manager Extension class
    /// </summary>
    public static class SecretManagerExtensions
    {
        /// <summary>
        /// Add Service Platform Secret Manager and dependencies
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddServicePlatformSecretManager(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));
            services.AddCertificateManager();
            services.AddAzureActiveDirectoryClient();
            services.AddSecretManager();
            return services;
        }
    }
}
