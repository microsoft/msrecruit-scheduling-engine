//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Context;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.Communication.Http.Handlers
{
    /// <summary>
    /// Adds D365 correlation headers and logs them if not already present:
    ///     x-ms-session-id
    ///     x-ms-root-activity-id
    ///     x-ms-activity-vector
    /// 
    /// Use when calling into another application or service to maintain execution correlation
    /// </summary>
    public sealed class HttpClientCorrelationHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public HttpClientCorrelationHandler(ILogger logger = null)
        {
            this.logger = logger;
        }

        public HttpClientCorrelationHandler(ILoggerFactory loggerFactory) : this(loggerFactory.CreateLogger<HttpClientCorrelationHandler>())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Contract.CheckValue(request, nameof(request));

            AddD365ExecutionContextToRequest(request);
            logger?.LogInformation("Request Headers:{0}{1}", Environment.NewLine, GetRequestHeaders(request));
            logger?.LogInformation("Request details:{0} to {1}", request.Method, request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);
            logger?.LogInformation("Status: {0}, Response Headers:{1}{2}", (int)response.StatusCode, Environment.NewLine, GetResponseHeaders(response));
            return response;
        }

        private string GetRequestHeaders(HttpRequestMessage request)
        {
            if (request?.Headers != null)
            {
                StringBuilder logBuilder = new StringBuilder();
                AppendHeaderIfPresent(logBuilder, request.Headers, HttpConstants.Headers.SessionIdHeaderName);
                AppendHeaderIfPresent(logBuilder, request.Headers, HttpConstants.Headers.RootActivityIdHeaderName);
                AppendHeaderIfPresent(logBuilder, request.Headers, HttpConstants.Headers.ActivityIdHeaderName);

                return logBuilder.ToString();
            }

            return "<null>";
        }

        private string GetResponseHeaders(HttpResponseMessage response)
        {
            if (response.Headers != null)
            {
                StringBuilder logBuilder = new StringBuilder();
                AppendHeaderIfPresent(logBuilder, response.Headers, HttpConstants.Headers.SessionIdHeaderName);
                AppendHeaderIfPresent(logBuilder, response.Headers, HttpConstants.Headers.RootActivityIdHeaderName);
                AppendHeaderIfPresent(logBuilder, response.Headers, HttpConstants.Headers.ActivityIdHeaderName);
                AppendHeaderIfPresent(logBuilder, response.Headers, HttpConstants.Headers.ErrorPayloadHeaderName);

                return logBuilder.ToString();
            }

            return "<null>";
        }

        private void AppendHeaderIfPresent(StringBuilder sb, HttpHeaders headers, string headerName)
        {
            if (headers.Contains(headerName))
            {
                sb.AppendLine(string.Format("{0}: {1}",
                    headerName,
                    string.Join(",", headers.GetValues(headerName))));
            }
        }

        private void AddD365ExecutionContextToRequest(HttpRequestMessage requestMessage)
        {
            var sessionIdName = HttpConstants.Headers.SessionIdHeaderName;
            var rootActivityIdName = HttpConstants.Headers.RootActivityIdHeaderName;
            var activityVectorName = HttpConstants.Headers.ActivityVectorHeaderName;

            var headers = requestMessage.Headers;
            var currentActivity = ServiceContext.Activity.Current;

            if (currentActivity != null)
            {
                if (!headers.Contains(sessionIdName))
                    headers.Add(sessionIdName, currentActivity.SessionId.ToString("D"));
                if (!headers.Contains(rootActivityIdName))
                    headers.Add(rootActivityIdName, currentActivity.RootActivityId.ToString());
                if (!headers.Contains(activityVectorName))
                    headers.Add(activityVectorName, currentActivity.ActivityVector);
            }
            else
            {
                // Default the RootActivityId and SessionId to new values if we are not in a context
                if (!headers.Contains(sessionIdName))
                    headers.Add(sessionIdName, Guid.NewGuid().ToString("D"));
                if (!headers.Contains(rootActivityIdName))
                    headers.Add(rootActivityIdName, Guid.NewGuid().ToString());
            }
        }
    }
}
