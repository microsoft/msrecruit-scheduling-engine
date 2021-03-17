//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.AspNetCore.Http.Abstractions;
using CommonLibrary.ServicePlatform.Communication;
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.AspNetCore.Http
{
    /// <summary>
    /// Adds session id, root activity id, and activity vector on outbound response.
    /// </summary>
    internal sealed class AddCorrelationInfoMiddleware : IStatelessHttpMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            Contract.AssertValue(httpContext, nameof(httpContext));
            Contract.AssertValue(next, nameof(next));

            // Save activity during middleware invocation. The Response.OnStarting method will execute at different 
            // times in the request lifecycle depending on which middleware writes to the response. This means it could
            // occur earlier in nested activity or later in the root activity both which would have different guids and vector and
            // return incorrect information
            var serviceActivity = ServiceContext.Activity.Current;

            httpContext.Response.OnStarting(() =>
            {
                AddHeader(httpContext, HttpConstants.Headers.SessionIdHeaderName, serviceActivity?.SessionId.ToString("D"));
                AddHeader(httpContext, HttpConstants.Headers.RootActivityIdHeaderName, serviceActivity?.RootActivityId.ToString("D"));
                AddHeader(httpContext, HttpConstants.Headers.ActivityVectorHeaderName, serviceActivity?.ActivityVector);
                return Task.FromResult(0);
            });

            await next();
        }

        private static void AddHeader(HttpContext context, string headerName, string headerValue)
        {
            Contract.AssertValue(context, nameof(context));
            Contract.AssertNonEmpty(headerName, nameof(headerName));
            Contract.AssertValue(headerValue, nameof(headerValue));

            if (!context.Response.Headers.ContainsKey(headerName))
            {
                context.Response.Headers.Add(headerName, headerValue);
            }
        }
    }
}
