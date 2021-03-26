//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.AspNetCore.Http.Abstractions;
using TA.CommonLibrary.ServicePlatform.Exceptions;
using TA.CommonLibrary.ServicePlatform.Tracing;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Http
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
