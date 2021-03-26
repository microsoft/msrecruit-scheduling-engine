//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.CommonDataService.Instrumentation;
using HR.TA.ServicePlatform.Context;
using HR.TA.ServicePlatform.Fabric;
using HR.TA.ServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace HR.TA.ServicePlatform.Communication.Http.Internal
{
    /// <summary>
    /// Http Client Wrapper for Service Platform. This is the second streamlined version.
    /// 
    /// Note that we're using the <see cref="HttpMessageInvoker"/> instead of the <see cref="HttpClient"/>. This is
    /// because the <see cref="HttpClient"/> is heavier and disposes request messages, not allowing them to be reused.
    /// </summary>
    internal sealed class HttpCommunicationClient : HttpMessageInvoker, IHttpCommunicationClient
    {
        private readonly CancellationTokenSource pendingRequestsCts = new CancellationTokenSource();
        private readonly HttpCommunicationClientInternalOptions options;
        private readonly ILogger logger;
        private bool disposed;
        private ILoggerFactory loggerFactory;

        internal HttpCommunicationClient(
            HttpMessageHandler handler,
            HttpCommunicationClientInternalOptions options,
            ILoggerFactory loggerFactory = null)
            : base(handler, disposeHandler: false)
        {
            Contract.AssertValue(options, nameof(options));

            this.options = options;
            this.Handler = handler;
            this.logger = loggerFactory?.CreateLogger<HttpCommunicationClient>();
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Contract.CheckValue(request, nameof(request));

            if (disposed)
            {
                throw new ObjectDisposedException(nameof(HttpCommunicationClient));
            }

            if (this.logger == null)
            {
                return ServiceContext.Activity.ExecuteAsync(
                    HttpRequestActivityType.Instance,
                    async () => await this.HttpRequest(cancellationToken, request));                
            }
            
            return this.logger.ExecuteAsync(
                HttpRequestActivityType.Instance,
                async () => await this.HttpRequest(cancellationToken, request),
                new [] { typeof(HttpCommunicationException), typeof(MonitoredFabricException), typeof(NonSuccessHttpResponseException) });
        }

        private async Task<HttpResponseMessage> HttpRequest(CancellationToken cancellationToken, HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var linkedCts = CreateRequestCts(cancellationToken))
                {
                    var stopwatch = Stopwatch.StartNew();
                    response = await base.SendAsync(request, linkedCts.Token);
                    Contract.CheckValue(response, nameof(response));
                    logger?.LogMetric(DefaultMetricConstants.MetricNamespace, DefaultMetricConstants.OutgoingHttpOperationDurationMetricName, new SortedList<string, string> { { DefaultMetricConstants.HttpStatusCodeDimension, response.StatusCode.ToString() } }, stopwatch.ElapsedMilliseconds);

                    if (options.ThrowOnNonSuccessResponse && !response.IsSuccessStatusCode)
                    {
                        throw await NonSuccessHttpResponseException.CreateAsync(response, this.loggerFactory);
                    }

                    return response;
                }
            }
            catch
            {
                if (response != null)
                {
                    response.Dispose();
                    response = null;
                }

                throw;
            }
        }

        /// <summary>
        /// Test hook only. Do not use for production.
        /// </summary>
        internal HttpMessageHandler Handler { get; }

        /// <summary>
        /// Test hook only. Do not use for production.
        /// </summary>
        internal HttpCommunicationClientInternalOptions Options
        {
            get { return options; }
        }

        /// <summary>
        /// Creates a combined request cancellation token.
        /// </summary>
        private CancellationTokenSource CreateRequestCts(CancellationToken cancellationToken)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, pendingRequestsCts.Token);
            if (options.Timeout != Timeout.InfiniteTimeSpan)
            {
                linkedCts.CancelAfter(options.Timeout);
            }

            return linkedCts;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                pendingRequestsCts.Cancel();
                pendingRequestsCts.Dispose();
            }

            disposed = true;
            base.Dispose(disposing);
        }

        private sealed class HttpRequestActivityType : SingletonActivityType<HttpRequestActivityType>
        {
            public HttpRequestActivityType()
                : base("SP.HttpClient.SendAsync", ActivityKind.ClientProxy)
            {
            }
        }
    }
}
