//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
using Newtonsoft.Json;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Http
{
    // TODO - 0000: [anbencic] Add static flights and principal deserialization here
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal sealed class InternalWebApiHost : IWebApiHost
    {
        // JsonSerializer is thread safe:
        // http://stackoverflow.com/questions/36186276/is-the-json-net-jsonserializer-threadsafe/36189439
        //
        private readonly JsonSerializer jsonSerializer;

        private readonly IEnvironmentContext environmentContext;

        public InternalWebApiHost(InternalWebApiHostOptions options, IEnvironmentContext environmentContext = null)
        {
            Contract.AssertValue(options, nameof(options));
            Contract.CheckValueOrNull(environmentContext, nameof(environmentContext));

            this.jsonSerializer = ContextSerializerFactory.CreateForInternalHost(options);
            this.environmentContext = environmentContext ?? ServiceContext.Environment.Current;
        }

        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            Contract.AssertValue(httpContext, nameof(httpContext));
            Contract.AssertValue(next, nameof(next));

            var executionContext = CreateContextForInternalHost(httpContext.Request.Headers, this.jsonSerializer, this.environmentContext);

            await ServiceContext.Activity.ExecuteRootAsync(
                executionContext,
                InternalRequestHandlerActivityType.Instance,
                action: async () =>
                {
                    // Get ILogger from the request context
                    var logger = httpContext.RequestServices.GetService<ILogger<InternalWebApiHost>>();
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

        private static RootExecutionContext CreateContextForInternalHost(IDictionary<string, StringValues> headers, JsonSerializer jsonSerializer, IEnvironmentContext environmentContext)
        {
            Contract.AssertValue(headers, nameof(headers));
            Contract.AssertValue(jsonSerializer, nameof(jsonSerializer));

            var rootExecutionContext = ExecutionContextFactory.TryDeserializeServiceContext(headers.TryGetFirstOrDefaultValue(HttpConstants.Headers.ExecutionContextHeaderName), jsonSerializer, AspNetCoreTrace.Instance);
            rootExecutionContext.EnvironmentContext = environmentContext;
            // Extend the ActivityVector to facilitate correlation
            rootExecutionContext.ActivityVector = ExecutionContextFactory.GetExtendedActivityVector(rootExecutionContext.ActivityVector);
            return rootExecutionContext;
        }

        private sealed class InternalRequestHandlerActivityType : SingletonActivityType<InternalRequestHandlerActivityType>
        {
            public InternalRequestHandlerActivityType()
                : base("SP.INTRQ", ActivityKind.ServiceApi)
            {
            }
        }
    }
}
