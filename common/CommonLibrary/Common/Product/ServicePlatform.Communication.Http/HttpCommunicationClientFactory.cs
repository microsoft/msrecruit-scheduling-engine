//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net.Http;
using CommonDataService.Common.Internal;
using CommonDataService.Instrumentation;
using ServicePlatform.Communication.Http.Internal;
using Microsoft.Extensions.Logging;

namespace ServicePlatform.Communication.Http
{
    /// <summary>
    /// This is developer interface to create ServicePlatform HttpServiceClient
    /// </summary>
    public sealed class HttpCommunicationClientFactory : IHttpCommunicationClientFactory
    {
        private readonly HttpMessageHandler sharedHttpMessageHandler = new HttpClientHandler();
        private readonly ILoggerFactory loggerFactory;
        private bool disposed;

        public HttpCommunicationClientFactory(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public IHttpCommunicationClient Create()
        {
            return Create(new HttpCommunicationClientOptions());
        }

        /// <inheritdoc />
        public IHttpCommunicationClient Create(HttpCommunicationClientOptions options, HttpMessageHandler messageHandler = null)
        {
            Contract.CheckValue(options, nameof(options));

            if (disposed)
            {
                throw new ObjectDisposedException(nameof(HttpCommunicationClientFactory));
            }

            var communicationClientHandler = HttpCommunicationClientUtil.CombineHandlers(messageHandler ?? this.sharedHttpMessageHandler, options.CustomHandlers);
            var communicationClientOptions = new HttpCommunicationClientInternalOptions(options);

            return new HttpCommunicationClient(communicationClientHandler, communicationClientOptions, loggerFactory);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                this.sharedHttpMessageHandler?.Dispose();

                disposed = true;
            }
        }
    }
}
