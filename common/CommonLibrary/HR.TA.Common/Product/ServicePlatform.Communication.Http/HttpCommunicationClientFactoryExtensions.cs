//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Net.Http;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Communication.Http.Handlers;
using HR.TA.ServicePlatform.Communication.Http.Routers;
using Microsoft.Extensions.Logging;

namespace HR.TA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Convenience monitored  and context extension methods over <see cref="IHttpCommunicationClientFactory"/>.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Readability not impacted.")]
    public static class HttpCommunicationClientFactoryExtensions
    {
        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, HttpCommunicationClientOptions options)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpMessageHandler"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                },
                messageHandler);
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpMessageHandler"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateMonitored(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                    {
                        new HttpClientRoutingHandler(router, loggerFactory),
                        new HttpClientCommunicationExceptionHandler(),
                    },
                messageHandler);
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, ILogger logger)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(logger, nameof(logger));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, logger),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(logger),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, HttpCommunicationClientOptions options)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILogger logger)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(logger, nameof(logger));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(logger),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        ///     - <see cref="ILogger"/>
        ///     - <see cref="HttpMessageHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILogger logger, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(logger, nameof(logger));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, logger),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(logger),
                },
                messageHandler);
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(loggerFactory),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(loggerFactory),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(loggerFactory),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(loggerFactory),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientCorrelationHandler"/>
        ///     - <see cref="ILogger"/>
        ///     - <see cref="HttpMessageHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateGTA(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientCorrelationHandler(loggerFactory),
                },
                messageHandler);
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, ILogger logger)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(logger, nameof(logger));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, logger),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, HttpCommunicationClientOptions options)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        ///     - <see cref="ILogger"/>
        ///     - <see cref="HttpMessageHandler"/>
        /// </summary>
        [Obsolete("Please use the method with logger factory to facilitate a switch from trace source.")]
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILogger logger, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(logger, nameof(logger));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, logger),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                },
                messageHandler);
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(loggerFactory),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers in the custom pipeline:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                new HttpCommunicationClientOptions(),
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientCommunicationExceptionHandler(loggerFactory),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(loggerFactory),
                    new HttpClientInternalExecutionContextHandler(),
                });
        }

        /// <summary>
        /// Creates an HTTP communication client with the following handlers added to the end of the 
        /// custom handler pipeline provided through the <paramref name="options"/>:
        /// 
        ///     - <see cref="HttpClientRoutingHandler"/> using the provided <paramref name="router"/>
        ///     - <see cref="HttpClientCommunicationExceptionHandler"/>
        ///     - <see cref="HttpClientInternalExecutionContextHandler"/>
        ///     - <see cref="ILogger"/>
        ///     - <see cref="HttpMessageHandler"/>
        /// </summary>
        public static IHttpCommunicationClient CreateInternal(this IHttpCommunicationClientFactory clientFactory, IHttpRouter router, HttpCommunicationClientOptions options, ILoggerFactory loggerFactory, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(clientFactory, nameof(clientFactory));
            Contract.CheckValue(router, nameof(router));
            Contract.CheckValue(options, nameof(options));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));

            return CreateCore(
                clientFactory,
                options,
                new DelegatingHandler[]
                {
                    new HttpClientRoutingHandler(router, loggerFactory),
                    new HttpClientCommunicationExceptionHandler(loggerFactory),
                    new HttpClientInternalExecutionContextHandler(),
                },
                messageHandler);
        }

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
                    new ExternalApiHandler(loggerFactory)
                };

            return CreateCore(
                httpCommunicationClientFactory,
                loggerFactory,
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
                    new ExternalApiHandler(loggerFactory)
                };

            return CreateCore(httpCommunicationClientFactory, loggerFactory, null, additionalHandlers, httpRouter);
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
                new ExternalApiHandler(loggerFactory)
            };

            return CreateCore(
                httpCommunicationClientFactory,
                loggerFactory,
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
                    new ExternalApiHandler(loggerFactory)
                };

            return CreateCore(
                httpCommunicationClientFactory,
                loggerFactory,
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
                    new ExternalApiHandler(loggerFactory)
                };

            return CreateCore(
                httpCommunicationClientFactory,
                loggerFactory,
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
                new ExternalApiHandler(loggerFactory)
            };

            return CreateCore(
                httpCommunicationClientFactory,
                loggerFactory,
                options,
                additionalHandlers,
                httpRouter,
                httpMessageHandler);
        }

        /// <summary>The create core.</summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="loggerFactory">The instnce for <see cref="ILoggerFactory"/>.</param>
        /// <param name="options">The options.</param>
        /// <param name="additionalHandlers">The additional handlers.</param>
        /// <param name="router">The router.</param>
        /// <param name="messageHandler">The message handler.</param>
        /// <returns>The <see cref="IHttpCommunicationClient"/>.</returns>
        private static IHttpCommunicationClient CreateCore(
            IHttpCommunicationClientFactory clientFactory,
            ILoggerFactory loggerFactory,
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

                // Then we add additional monitored/GTA handlers
                if (additionalHandlers != null)
                    customHandlers.AddRange(additionalHandlers);

                options.CustomHandlers = customHandlers;
            }

            if (router == null)
            {
                return clientFactory.CreateMonitored(options, loggerFactory);
            }

            return clientFactory.CreateMonitored(router, options, loggerFactory, messageHandler);
        }

        private static IHttpCommunicationClient CreateCore(
            IHttpCommunicationClientFactory clientFactory,
            HttpCommunicationClientOptions options,
            DelegatingHandler[] additionalHandlers,
            HttpMessageHandler messageHandler = null)
        {
            Contract.AssertValue(clientFactory, nameof(clientFactory));
            Contract.AssertValue(options, nameof(options));
            Contract.AssertValue(additionalHandlers, nameof(additionalHandlers));

            var originalHandlerCount = options.CustomHandlers != null ? options.CustomHandlers.Count : 0;
            var customHandlers = new List<DelegatingHandler>(originalHandlerCount + additionalHandlers.Length);

            // Original custom handlers go first
            if (options.CustomHandlers != null)
                customHandlers.AddRange(options.CustomHandlers);

            // Then we add additional monitored/GTA handlers
            if (additionalHandlers != null)
                customHandlers.AddRange(additionalHandlers);

            options.CustomHandlers = customHandlers;

            return clientFactory.Create(options, messageHandler);
        }
    }
}
