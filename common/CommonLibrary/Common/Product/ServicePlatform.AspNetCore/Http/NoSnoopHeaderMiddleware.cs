//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CommonDataService.Common.Internal;
using ServicePlatform.AspNetCore.Http.Abstractions;
using ServicePlatform.Exceptions;
using ServicePlatform.Tracing;

namespace ServicePlatform.AspNetCore.Http
{
    internal sealed class NoSnoopHeaderMiddleware : IStatelessHttpMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            Contract.AssertValue(httpContext, nameof(httpContext));
            Contract.AssertValue(next, nameof(next));

            httpContext.Response.OnStarting(() =>
            {
                return AddHeader(httpContext);
            });

            await next();            
        }

        private static Task AddHeader(HttpContext context)
        {
            const string ContentTypeOptions = "X-Content-Type-Options";

            Contract.AssertValue(context, nameof(context));

            if (!context.Response.Headers.ContainsKey(ContentTypeOptions))
            {
                context.Response.Headers.Add(ContentTypeOptions, "nosniff");
            }

            return Task.FromResult(0);
        }
    }
}
