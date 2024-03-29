//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.Product.ServicePlatform.AspNetCore.Builder
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;
    using HR.TA.Common.Product.ServicePlatform.AspNetCore.Http;
    using HR.TA.Common.WebNotifications;
    using HR.TA.Common.WebNotifications.Interfaces;
    using HR.TA.Common.WebNotifications.Internals;

    /// <summary>
    /// The <see cref="WebNotificationDependencyInjectionExtenstions"/> class provides extension methods to add web notifications services
    /// </summary>
    public static class WebNotificationDependencyInjectionExtenstions
    {
        /// <summary>
        /// Adds the web notifications.
        /// </summary>
        /// <param name="services">The instance for <see cref="IServiceCollection"/>.</param>
        /// <param name="configureClient">The configure client.</param>
        /// <returns>The instance for <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddWebNotifications(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
        {
            _ = services.AddTransient<TokenDelegatingHandler>();
            _ = services.AddSingleton<IWebNotificationBuilder, DefaultWebNotificationBuilder>();
            _ = services.AddHttpClient<IWebNotificationClient, DefaultWebNotificationClient>(configureClient)
                .AddHttpMessageHandler<TokenDelegatingHandler>();
            _ = services.AddTransient<IWebNotificationBuilderClient, WebNotificationBuilderClient>();
            return services;
        }
    }
}
