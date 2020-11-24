﻿//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Builder;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions;
using UseStatelessHttpMiddlewareMock = System.Func<Microsoft.AspNetCore.Builder.IApplicationBuilder, MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions.IStatelessHttpMiddleware, Microsoft.AspNetCore.Builder.IApplicationBuilder>;

namespace MS.GTA.ServicePlatform.AspNetCore.Builder
{
    /// <summary>
    /// Helper methods over an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class UseMiddlewareExtensions
    {
        private static UseStatelessHttpMiddlewareMock useStatelessHttpMiddlewareMock;

        /// <summary>
        /// Adds <paramref name="middleware"/> as a singleton instance into 
        /// the provided <paramref name="builder"/>.
        /// </summary>
        public static IApplicationBuilder UseStatelessHttpMiddleware(this IApplicationBuilder builder, IStatelessHttpMiddleware middleware)
        {
            Contract.AssertValue(builder, nameof(builder));
            Contract.AssertValue(middleware, nameof(middleware));

            if (useStatelessHttpMiddlewareMock != null)
                return useStatelessHttpMiddlewareMock(builder, middleware);

            return builder.Use(middleware.InvokeAsync);
        }

        /// <summary>
        /// Creates an instance of <paramref name="T"/> and adds it as a singleton middleware into 
        /// the provided <paramref name="builder"/>.
        /// </summary>
        public static IApplicationBuilder UseStatelessHttpMiddleware<T>(this IApplicationBuilder builder)
            where T : class, IStatelessHttpMiddleware, new()
        {
            Contract.AssertValue(builder, nameof(builder));

            var middleware = new T();
            return builder.Use(middleware.InvokeAsync);
        }
        // TODO
        /*
        // Test-only hooks
        public static IDisposable MockUseStatelessHttpMiddleware(UseStatelessHttpMiddlewareMock mock)
        {
            var previousMock = useStatelessHttpMiddlewareMock;
            useStatelessHttpMiddlewareMock = mock;
            return new ActionDisposable(() => useStatelessHttpMiddlewareMock = previousMock);
        }*/
    }
}
