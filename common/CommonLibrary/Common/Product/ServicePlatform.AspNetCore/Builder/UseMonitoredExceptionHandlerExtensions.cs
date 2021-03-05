//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using ServicePlatform.AspNetCore.Http;
using ServicePlatform.Exceptions;
using Microsoft.Extensions.Logging;

namespace ServicePlatform.AspNetCore.Builder
{
    public static class UseMonitoredExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseMonitoredExceptionHandler(this IApplicationBuilder builder)
        {
            // we first query the application services for a registered version of IServiceExceptionHandlingProvider
            // this way the service can inject its own implementation
            IServiceExceptionHandlingProvider exceptionHandlingProvider = (IServiceExceptionHandlingProvider)(builder.ApplicationServices.GetService(typeof(IServiceExceptionHandlingProvider)))
                ?? new MonitoredExceptionHandlingProvider((ILoggerFactory)builder.ApplicationServices.GetService(typeof(ILoggerFactory)));

            return builder.UseStatelessHttpMiddleware(new MonitoredExceptionHandler(exceptionHandlingProvider));
        }
    }
}
