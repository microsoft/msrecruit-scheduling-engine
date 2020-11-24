// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TalentHttpCommunicationClientFactoryExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MS.GTA.Common.Web.DelegatingHandlers;
    using MS.GTA.ServicePlatform.Communication.Http;
    using MS.GTA.ServicePlatform.Communication.Http.Routers;
    using Microsoft.Extensions.Logging;
    using MS.GTA.CommonDataService.Common;

    /// <summary>The talent http communication client factory extensions.</summary>
    public static class TalentHttpCommunicationClientFactoryExtensions
    {
        /// <summary>The create external API.</summary>
        /// <param name="httpCommunicationClientFactory">The talent http communication client factory.</param>
        /// <param name="loggerFactory">The logger Factory.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        public static IHttpCommunicationClient CreateExternalApi(
            this IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            var additionalHandlers = new List<DelegatingHandler>
                {
                    new ExternalApiHandler(loggerFactory.CreateLogger<ExternalApiHandler>())
                };

            return CreateCore(
                httpCommunicationClientFactory,
                null,
                additionalHandlers);
        }

        /// <summary>The create external API.</summary>
        /// <param name="httpCommunicationClientFactory">The http communication client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="httpRouter">The http router.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        public static IHttpCommunicationClient CreateExternalApi(
            this IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILoggerFactory loggerFactory,
            IHttpRouter httpRouter)
        {
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            Contract.CheckValue(httpRouter, nameof(httpRouter));

            var additionalHandlers = new List<DelegatingHandler>
                {
                    new ExternalApiHandler(loggerFactory.CreateLogger<ExternalApiHandler>())
                };

            return CreateCore(httpCommunicationClientFactory, null, additionalHandlers, httpRouter);
        }

        /// <summary>The create external API.</summary>
        /// <param name="httpCommunicationClientFactory">The http communication client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="httpRouter">The http router.</param>
        /// <param name="options">The http communication client options.</param>
        /// <param name="httpMessageHandler">The http message handler.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        public static IHttpCommunicationClient CreateExternalApi(
            this IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILoggerFactory loggerFactory,
            IHttpRouter httpRouter,
            HttpCommunicationClientOptions options,
            HttpMessageHandler httpMessageHandler = null)
        {
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            Contract.CheckValue(httpRouter, nameof(httpRouter));
            Contract.CheckValue(options, nameof(options));

            var additionalHandlers = new List<DelegatingHandler>
            {
                new ExternalApiHandler(loggerFactory.CreateLogger<ExternalApiHandler>())
            };

            return CreateCore(
                httpCommunicationClientFactory,
                options,
                additionalHandlers,
                httpRouter,
                httpMessageHandler);
        }

        /// <summary>The create external API with poll.</summary>
        /// <param name="httpCommunicationClientFactory">The http communication client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="getTokenFunction">The get Token Function.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        public static IHttpCommunicationClient CreateExternalApiWithPoll(
            this IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILoggerFactory loggerFactory,
            Func<Task<string>> getTokenFunction)
        {
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            Contract.CheckValue(getTokenFunction, nameof(getTokenFunction));

            var additionalHandlers = new List<DelegatingHandler>
                {
                    // In this scenario we leverage the poll handler which then leverages the external api handler for wrapping.
                    new PollHandler(loggerFactory.CreateLogger<PollHandler>(), getTokenFunction),
                    new ExternalApiHandler(loggerFactory.CreateLogger<ExternalApiHandler>())
                };

            return CreateCore(
                httpCommunicationClientFactory,
                null,
                additionalHandlers);
        }

        /// <summary>The create external API with poll.</summary>
        /// <param name="httpCommunicationClientFactory">The http communication client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="getTokenFunction">The get Token Function.</param>
        /// <param name="httpRouter">The http Router.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        public static IHttpCommunicationClient CreateExternalApiWithPoll(
            this IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILoggerFactory loggerFactory,
            Func<Task<string>> getTokenFunction,
            IHttpRouter httpRouter)
        {
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            Contract.CheckValue(getTokenFunction, nameof(getTokenFunction));
            Contract.CheckValue(httpRouter, nameof(httpRouter));

            var additionalHandlers = new List<DelegatingHandler>
                {
                    // In this scenario we leverage the poll handler which then leverages the external api handler for wrapping.
                    new PollHandler(loggerFactory.CreateLogger<PollHandler>(), getTokenFunction),
                    new ExternalApiHandler(loggerFactory.CreateLogger<ExternalApiHandler>())
                };

            return CreateCore(
                httpCommunicationClientFactory,
                null,
                additionalHandlers,
                httpRouter);
        }

        /// <summary>The create external API with poll.</summary>
        /// <param name="httpCommunicationClientFactory">The http communication client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="getTokenFunction">The get Token Function.</param>
        /// <param name="httpRouter">The http Router.</param>
        /// <param name="options">The http communication client options.</param>
        /// <param name="httpMessageHandler">The http message handler.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        public static IHttpCommunicationClient CreateExternalApiWithPoll(
            this IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILoggerFactory loggerFactory,
            Func<Task<string>> getTokenFunction,
            IHttpRouter httpRouter,
            HttpCommunicationClientOptions options,
            HttpMessageHandler httpMessageHandler = null)
        {
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
            Contract.CheckValue(getTokenFunction, nameof(getTokenFunction));
            Contract.CheckValue(options, nameof(options));

            var additionalHandlers = new List<DelegatingHandler>
            {
                // In this scenario we leverage the poll handler which then leverages the external api handler for wrapping.
                new PollHandler(loggerFactory.CreateLogger<PollHandler>(), getTokenFunction),
                new ExternalApiHandler(loggerFactory.CreateLogger<ExternalApiHandler>())
            };

            return CreateCore(
                httpCommunicationClientFactory,
                options,
                additionalHandlers,
                httpRouter,
                httpMessageHandler);
        }

        /// <summary>The create core.</summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="options">The options.</param>
        /// <param name="additionalHandlers">The additional handlers.</param>
        /// <param name="router">The router.</param>
        /// <param name="messageHandler">The message handler.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        private static IHttpCommunicationClient CreateCore(
            IHttpCommunicationClientFactory clientFactory,
            HttpCommunicationClientOptions options = null,
            List<DelegatingHandler> additionalHandlers = null,
            IHttpRouter router = null,
            HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));

            if (options == null)
            {
                options = new HttpCommunicationClientOptions();
            }

            if (additionalHandlers != null)
            {
                var originalHandlerCount = options.CustomHandlers != null ? options.CustomHandlers.Count : 0;
                var customHandlers = new List<DelegatingHandler>(originalHandlerCount + additionalHandlers.Count);

                // Original custom handlers go first
                if (options.CustomHandlers != null)
                    customHandlers.AddRange(options.CustomHandlers);

                // Then we add additional monitored/D365 handlers
                if (additionalHandlers != null)
                    customHandlers.AddRange(additionalHandlers);

                options.CustomHandlers = customHandlers;
            }

            if (router == null)
            {
                return clientFactory.CreateMonitored(options);
            }

            return clientFactory.CreateMonitored(router, options, messageHandler);
        }
    }
}