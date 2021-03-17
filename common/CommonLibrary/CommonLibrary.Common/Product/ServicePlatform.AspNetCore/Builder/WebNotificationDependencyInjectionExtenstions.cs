//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.Product.ServicePlatform.AspNetCore.Builder
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;
    using CommonLibrary.Common.Product.ServicePlatform.AspNetCore.Http;
    using CommonLibrary.Common.WebNotifications;
    using CommonLibrary.Common.WebNotifications.Interfaces;
    using CommonLibrary.Common.WebNotifications.Internals;

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
