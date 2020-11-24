//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions
{
    /// <summary>
    /// A strongly-typed abstraction for a stateless HTTP middleware. This is
    /// to be used with <see cref="UseMiddlewareExtensions"/> in favor of un-typed
    /// middleware classes or raw middleware delegates.
    /// </summary>
    public interface IStatelessHttpMiddleware
    {
        /// <summary>
        /// Invoked in ASP.NET Core pipeline where the stateless middleware is used.
        /// </summary>
        Task InvokeAsync(HttpContext httpContext, Func<Task> next);
    }
}
