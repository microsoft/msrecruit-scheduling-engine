//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Web.Utils
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using TA.CommonLibrary.ServicePlatform.Tracing;

    /// <summary>
    /// Handler class to handle http client call retries
    /// </summary>
    public class HttpRetryMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Number of times to attempt retries
        /// </summary>
        private const int MaxRetries = 3;

        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRetryMessageHandler" /> class.
        /// </summary>
        /// <param name="innerHandler">Http message handler</param>
        /// <param name="trace">Trace source instance</param>
        public HttpRetryMessageHandler(HttpMessageHandler innerHandler, ITraceSource trace)
        : base(innerHandler)
        {
            Contract.CheckValue(innerHandler, "innerHandler should be provided");
            Contract.CheckValue(trace, "trace should be provided");

            this.trace = trace;
        }

        /// <summary>
        /// Override sendAsync method to handle retries
        /// </summary>
        /// <param name="request">HTTP Request message</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The HTTP Response Message</returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Contract.CheckValue(request, nameof(request), "request should be provided");
            Contract.Check(cancellationToken != null, "cancellationToken should be provided");

            for (int i = 0; i < MaxRetries - 1; i++)
            {
                this.trace.TraceInformation($"HttpRetryMessageHelper: Attempt number {i + 1} to endpoint: {request.RequestUri.ToString()}");
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    this.trace.TraceInformation($"HttpRetryMessageHelper: Succeeded at attempt number {i + 1} to endpoint: {request.RequestUri.ToString()}");
                    return response;
                }
            }

            this.trace.TraceInformation($"HttpRetryMessageHelper: Attempt number: {MaxRetries} to endpoint: {request.RequestUri.ToString()}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
