//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using TA.CommonLibrary.ServicePlatform.AspNetCore.Http;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Builder
{
    public static class UseNoSnoopHeaderExtensions
    {
        public static IApplicationBuilder UseNoSnoopHeader(this IApplicationBuilder builder) => builder.UseStatelessHttpMiddleware(new NoSnoopHeaderMiddleware());
    }
}
