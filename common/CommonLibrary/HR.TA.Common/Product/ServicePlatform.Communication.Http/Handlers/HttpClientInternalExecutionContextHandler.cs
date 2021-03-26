//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Context;

namespace HR.TA.ServicePlatform.Communication.Http.Handlers
{
    /// <summary>
    /// Adds the entire execution context to the request. Use when calling into an
    /// application's internal nano-service when transfer of the entire execution 
    /// context is desired.
    /// </summary>
    public sealed class HttpClientInternalExecutionContextHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Contract.CheckValue(request, nameof(request));

            AddInternalExecutionContextToRequest(request);

            return base.SendAsync(request, cancellationToken);
        }

        private void AddInternalExecutionContextToRequest(HttpRequestMessage requestMessage)
        {
            var header = requestMessage.Headers;

            if (header.Contains(HttpConstants.Headers.ExecutionContextHeaderName))
                return;

            // CaptureRoot will never return null
            var rootExecutionContext = ServiceContext.CaptureRoot();
            var jsonString = HttpJsonUtil.JsonSerialize(rootExecutionContext);
            requestMessage.Headers.Add(HttpConstants.Headers.ExecutionContextHeaderName, jsonString);
        }
    }
}
