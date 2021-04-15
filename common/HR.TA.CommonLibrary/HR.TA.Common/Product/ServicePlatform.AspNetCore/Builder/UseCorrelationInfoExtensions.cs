//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using HR.TA.ServicePlatform.AspNetCore.Http;

namespace HR.TA.ServicePlatform.AspNetCore.Builder
{
    public static class UseCorrelationInfoExtensions
    {
        public static IApplicationBuilder UseCorrelationInfo(this IApplicationBuilder builder) => builder.UseStatelessHttpMiddleware(new AddCorrelationInfoMiddleware());
    }
}
