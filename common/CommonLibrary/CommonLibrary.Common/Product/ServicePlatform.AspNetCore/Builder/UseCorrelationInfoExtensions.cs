//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using CommonLibrary.ServicePlatform.AspNetCore.Http;

namespace CommonLibrary.ServicePlatform.AspNetCore.Builder
{
    public static class UseCorrelationInfoExtensions
    {
        public static IApplicationBuilder UseCorrelationInfo(this IApplicationBuilder builder) => builder.UseStatelessHttpMiddleware(new AddCorrelationInfoMiddleware());
    }
}
