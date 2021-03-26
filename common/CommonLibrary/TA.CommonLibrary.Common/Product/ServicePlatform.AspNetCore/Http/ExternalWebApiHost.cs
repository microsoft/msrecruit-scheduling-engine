//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.CommonDataService.Instrumentation;
using TA.CommonLibrary.ServicePlatform.AspNetCore.Http.Abstractions;
using TA.CommonLibrary.ServicePlatform.Communication;
using TA.CommonLibrary.ServicePlatform.Context;
using TA.CommonLibrary.ServicePlatform.Exceptions;
using TA.CommonLibrary.ServicePlatform.Hosting;
using TA.CommonLibrary.ServicePlatform.Tracing;
using TA.CommonLibrary.ServicePlatform.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Http
{
    // TODO - 0000: [anbencic] Add principal and flights provider here
    public sealed class ExternalWebApiHost : IWebApiHost
    {
        private readonly IEnvironmentContext environmentContext;

        public ExternalWebApiHost(IEnvironmentContext environmentContext = null)
        {
            Contract.CheckValueOrNull(environmentContext, nameof(environmentContext));

            this.environmentContext = environmentContext ?? ServiceContext.Environment.Current;
        }

        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            Contract.AssertValue(httpContext, nameof(httpContext));
            Contract.AssertValue(next, nameof(next));

            var executionContext = CreateContextForExternalHost(httpContext.Request.Headers, this.environmentContext);

            await ServiceContext.Activity.ExecuteRootAsync(
                executionContext,
                ExternalRequestHandlerActivityType.Instance,
                action: async () =>
                {
                    // Get logger from the request context
                    var logger = httpContext.RequestServices.GetService<ILogger<ExternalWebApiHost>>();
                    var stopwatch = Stopwatch.StartNew();

                    try
                    {
                        await next();
                        ServiceContext.Activity.Current.AddHttpStatusCode(httpContext.Response.StatusCode);
                        logger?.LogMetric(DefaultMetricConstants.MetricNamespace, DefaultMetricConstants.IncomingHttpOperationDurationMetricName, new SortedList<string, string>() { { DefaultMetricConstants.HttpStatusCodeDimension, httpContext.Response.StatusCode.ToString() } }, stopwatch.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        ServiceContext.Activity.Current.AddHttpStatusCode(ex.GetHttpStatusCode());
                        logger?.LogMetric(DefaultMetricConstants.MetricNamespace, DefaultMetricConstants.IncomingHttpOperationDurationMetricName, new SortedList<string, string>() { { DefaultMetricConstants.HttpStatusCodeDimension, ex.GetHttpStatusCode().ToString() } }, stopwatch.ElapsedMilliseconds);
                        
                        throw;
                    }
                });
        }

        private static RootExecutionContext CreateContextForExternalHost(IDictionary<string, StringValues> headers, IEnvironmentContext environmentContext)
        {
            Contract.AssertValue(headers, nameof(headers));

            string sessionIdHeader = headers.TryGetFirstOrDefaultValue(HttpConstants.Headers.SessionIdHeaderName);
            string activityIdHeader = headers.TryGetFirstOrDefaultValue(HttpConstants.Headers.RootActivityIdHeaderName);
            string activityVectorHeader = headers.TryGetFirstOrDefaultValue(HttpConstants.Headers.ActivityVectorHeaderName);

            var rootExecutionContext = ExecutionContextFactory.CreateForExternalHost(sessionIdHeader, activityIdHeader, activityVectorHeader, AspNetCoreTrace.Instance);
            rootExecutionContext.EnvironmentContext = environmentContext;

            return rootExecutionContext;
        }

        private sealed class ExternalRequestHandlerActivityType : SingletonActivityType<ExternalRequestHandlerActivityType>
        {
            public ExternalRequestHandlerActivityType()
                : base("SP.EXTRQ", ActivityKind.ServiceApi)
            {
            }
        }
    }
}
