//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions;
using MS.GTA.ServicePlatform.Exceptions;
using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ServicePlatform.AspNetCore.Http
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
