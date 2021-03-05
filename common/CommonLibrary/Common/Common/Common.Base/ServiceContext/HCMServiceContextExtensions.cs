//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace Common.Base.ServiceContext
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// HCM Service context extensions
    /// </summary>
    public static class HCMServiceContextExtensions
    {
        /// <summary>
        /// Adds HCM Service context
        /// </summary>
        /// <param name="services">Service collection instance</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHCMServiceContext(this IServiceCollection services)
        {
            return services.AddScoped<IHCMServiceContext, HCMServiceContext>();
        }
    }
}
