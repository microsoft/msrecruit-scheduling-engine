//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using ServicePlatform.AspNetCore.Http;

namespace ServicePlatform.AspNetCore.Builder
{
    public static class UseNoSnoopHeaderExtensions
    {
        public static IApplicationBuilder UseNoSnoopHeader(this IApplicationBuilder builder) => builder.UseStatelessHttpMiddleware(new NoSnoopHeaderMiddleware());
    }
}
