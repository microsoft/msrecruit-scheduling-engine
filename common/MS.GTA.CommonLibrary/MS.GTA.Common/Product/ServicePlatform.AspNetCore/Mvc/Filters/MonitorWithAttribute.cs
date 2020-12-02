//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using MS.GTA.Common.Base.Configuration;
using MS.GTA.Common.Common.Common.Base.Configuration;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Exceptions;
using MS.GTA.ServicePlatform.Hosting;
using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Action Filter Attribute that applies a monitored scope to the execution of the activity.
    /// </summary>
    public sealed class MonitorWithAttribute : Attribute, IAsyncActionFilter, IOrderedFilter
    {
        private readonly ActivityType activityTypeInstance;
        private readonly string businessActivity;
        private readonly string featureName;
        
        /// <summary>
        /// Creates a new instance of <see cref="MonitorWithAttribute"/>.
        /// </summary>
        /// <param name="singletonActivityType">Activity type. Must be a type derived from <see cref="SingletonActivityType{T}"/></param>
        public MonitorWithAttribute(Type singletonActivityType)
        {
            Contract.CheckValue(singletonActivityType, nameof(singletonActivityType));

            var baseType = singletonActivityType.BaseType;
            Contract.CheckValue(baseType, nameof(baseType));

            var instancePropertyInfo = baseType.GetProperty("Instance");
            Contract.CheckValue(instancePropertyInfo, nameof(instancePropertyInfo));

            var activityTypeInstance = instancePropertyInfo.GetValue(null) as ActivityType;
            Contract.CheckValue(activityTypeInstance, nameof(activityTypeInstance));

            this.activityTypeInstance = activityTypeInstance;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MonitorWithAttribute"/>.
        /// </summary>
        /// <param name="activityName">Name of the activity to monitor</param>
        /// <param name="businessActivity">The name of the business activity.</param>
        /// <param name="featureName">The name of the feature.</param>
        public MonitorWithAttribute(string activityName, string businessActivity = null, string featureName = null)
        {
            Contract.CheckNonWhitespace(activityName, nameof(activityName));

            this.activityTypeInstance = new RuntimeActivityType(activityName);
            this.businessActivity = businessActivity;
            this.featureName = featureName;
        }

        /// <inheritdoc />
        public int Order { get; set; }

        // Even though the attribute implements IAsyncActionFilter this will work with both 
        // async and sync controller actions. There is a slight penalty for the async context switch but 
        // in most cases the underlying actions will have an asynchronous transition anyways. 
        // 
        // The alternative is to implement IActionFilter but then we'd have to create an instance of this 
        // attribute for every request to store the context rollback IDisposable which is not desirable.
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            // Persist the RootActivityID and SessionId that was supplied in the request
            var activityRootActivityId = httpContext?.Request?.Headers?.TryGetFirstOrDefaultValue(HttpConstants.Headers.RootActivityIdHeaderName);
            var activitySessionId = httpContext?.Request?.Headers?.TryGetFirstOrDefaultValue(HttpConstants.Headers.SessionIdHeaderName);

            await ServiceContext.Activity.ExecuteAsync(
                activityTypeInstance,
                async () =>
                {
                    try
                    {
                        var resultContext = await next();
                        if (resultContext == null)
                        {
                            // Best effort if we don't have a result context, specification is not clear about this
                            ServiceContext.Activity.Current.AddHttpStatusCode(context.HttpContext.Response.StatusCode);
                        }
                        else if (resultContext.Exception != null && !resultContext.ExceptionHandled)
                        {
                            // In case of unhandled exception we have to fail the activity as well
                            ServiceContext.Activity.Current.AddHttpStatusCode(resultContext.Exception.GetHttpStatusCode());
                            ServiceContext.Activity.Current.FailWith(resultContext.Exception);
                        }
                        else
                        {
                            var statusCodeResult = resultContext.Result as StatusCodeResult;
                            if (statusCodeResult != null)
                            {
                                // Best effort for status code results
                                ServiceContext.Activity.Current.AddHttpStatusCode(statusCodeResult.StatusCode);
                            }
                            else
                            {
                                // Nothing we can do in this case. The right solution here would be to separate activity
                                // and request events instead of merging them together. This is TBD once we have support 
                                // for custom events.
                                ServiceContext.Activity.Current.AddHttpStatusCode(context.HttpContext.Response.StatusCode);
                            }
                        }
                        try
                        {
                            TelemetryClient telemetryClient = new TelemetryClient();
                            telemetryClient.InstrumentationKey = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<LoggingConfiguration>().InstrumentationKey;
                            if (!string.IsNullOrEmpty(this.businessActivity))
                            {
                            }
                            if (!string.IsNullOrEmpty(this.featureName))
                            {
                            }
                        }
                        catch (Exception)
                        {
                            //Do Nothing
                        }
                    }
                    catch (Exception ex)
                    {
                        ServiceContext.Activity.Current.AddHttpStatusCode(ex.GetHttpStatusCode());
                        throw;
                    }
                }, activityRootActivityId, activitySessionId);
        }
    }
}
