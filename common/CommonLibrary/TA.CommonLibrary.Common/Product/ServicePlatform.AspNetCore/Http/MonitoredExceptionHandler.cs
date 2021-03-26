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
using TA.CommonLibrary.ServicePlatform.Communication;
using TA.CommonLibrary.ServicePlatform.Exceptions;
using TA.CommonLibrary.ServicePlatform.Tracing;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Http
{
    internal sealed class MonitoredExceptionHandler : IStatelessHttpMiddleware
    {
        private readonly IServiceExceptionHandlingProvider exceptionHandlingProvider;

        public MonitoredExceptionHandler(IServiceExceptionHandlingProvider exceptionHandlingProvider)
        {
            Contract.CheckValue(exceptionHandlingProvider, nameof(exceptionHandlingProvider));
            this.exceptionHandlingProvider = exceptionHandlingProvider;
        }

        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            Contract.AssertValue(httpContext, nameof(httpContext));
            Contract.AssertValue(next, nameof(next));

            try
            {
                await next();
            }
            catch (Exception ex) when (!this.exceptionHandlingProvider.IsExceptionFatal(ex))
            {
                if (!httpContext.Response.HasStarted)
                {
                    AspNetCoreTrace.Instance.TraceError(
                        "An exception was encountered while while processing the request. " +
                        "The error will be written to the response as it has not started yet.");

                    using (var errorResponse = new MemoryStream())
                    {
                        this.exceptionHandlingProvider.Serialize(ex, errorResponse);

                        httpContext.Response.StatusCode = this.exceptionHandlingProvider.GetHttpStatusCode(ex);                        
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.ContentLength = errorResponse.Length;

                        httpContext.Response.Headers.Add(HttpConstants.Headers.ErrorPayloadHeaderName, "Error");

                        errorResponse.Position = 0;
                        await errorResponse.CopyToAsync(httpContext.Response.Body);
                    }
                }
                else
                {
                    AspNetCoreTrace.Instance.TraceError(
                        "An exception was encountered while while processing the request. " +
                        "The error will NOT be written to the response as it has already started.");
                }
            }
        }
    }
}
