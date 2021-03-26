//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Product.ServicePlatform.Instrumentation.ApplicationInsights
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.CommonDataService.Instrumentation;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Internal;
    using TA.CommonLibrary.Common.Base.Exceptions;
    using TA.CommonLibrary.Common.Base.Security;
    using TA.CommonLibrary.CommonDataService.Common.Internal;
    using TA.CommonLibrary.CommonDataService.Instrumentation.Privacy;
    using TA.CommonLibrary.ServicePlatform.Context;
    using Newtonsoft.Json;

    /// <summary>
    /// The <see cref="AppInsightsLogger"/> implements the logging.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class AppInsightsLogger : ILogger
    {
        private readonly string categoryName;
        private readonly bool enableCustomTraceProperties;
        private readonly ConcurrentDictionary<string, int> concurrentActivityCounts;
        private readonly ITraceEventProvider traceEventProvider;
        private readonly IPrivacyMarker privacyMarker = new XmlPrivacyMarker(escapeContent: false);

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInsightsLogger"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        public AppInsightsLogger(string categoryName, string instrumentationKey) : this(categoryName, instrumentationKey, AppInsightsTraceEventProvider.GetInstance(instrumentationKey))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInsightsLogger"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        /// <param name="traceEventProvider">The trace event provider.</param>
        internal AppInsightsLogger(string categoryName, string instrumentationKey, ITraceEventProvider traceEventProvider)
        {
            Contract.CheckNonEmpty(categoryName, nameof(categoryName));
            Contract.CheckNonEmpty(instrumentationKey, nameof(instrumentationKey));

            this.categoryName = categoryName;
            this.traceEventProvider = traceEventProvider;
            this.enableCustomTraceProperties = true; // TO-DO: Set this based on the consumer needs.
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">Current state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return VoidDisposable.Instance;
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <returns>
        ///   <c>true</c> if enabled.
        /// </returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true; // TO-DO: Can be enhanced to filter the log for only specific names.
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">State being passed along.</typeparam>
        /// <param name="logLevel">Entry will be written on this <see cref="LogLevel"/> level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Contract.AssertValueOrNull(exception, nameof(exception));
            Contract.AssertValue(formatter, nameof(formatter));

            switch ((object)state)
            {                
                case OperationLogData operationLogData:
                    LogOperation(operationLogData);
                    return;
                case ActivityLogData activityLogData:
                    LogActivity(logLevel, eventId, activityLogData, exception);
                    return;
                default:
                    LogTrace(logLevel, eventId, state, exception, formatter);
                    return;
            }
        }

        private void LogTrace<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Contract.AssertValueOrNull(exception, nameof(exception));
            Contract.AssertValue(formatter, nameof(formatter));

            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message;

            // Reason "is" cannot be used here without a cast: https://github.com/dotnet/roslyn/issues/16195
            if ((object)state is FormattedLogValues formattedLogValues && formattedLogValues.Count > 1)
            {
                message = PrivacyLogValuesFormatter.CreateAndFormat(formattedLogValues, this.privacyMarker);
            }
            else
            {
                message = formatter(state, exception);
            }

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception.ToString();
            }

            var activity = ServiceContext.Activity.Current ?? ServiceContext.Activity.Empty;
            var appPrincipal = ServiceContext.Principal.Current as HCMApplicationPrincipal;
            var environmentContext = ServiceContext.Environment.Current;

            
            var application = environmentContext.Application;
            var service = environmentContext.Service;
            var codePackageVersion = environmentContext.CodePackageVersion;
            var sessionId = activity.SessionId.ToString();
            var rootActivityId = activity.RootActivityId.ToString();
            var activityVector = activity.ActivityVector;
            var activityType = activity.ActivityType;
            var sourceName = categoryName;

            // Using only integer part of EventId in order to have predictable values in Kusto.
            var sourceId = eventId.Id.ToString();

            var traceLevel = GetTraceLevelString(logLevel);
            traceEventProvider.LogTrace(application, service, codePackageVersion, sessionId, rootActivityId, activityVector, activityType, sourceName, sourceId, traceLevel, message, appPrincipal?.UserObjectId ?? string.Empty);
        }

        private string GetTraceLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return "Fatal";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Warning:
                    return "Warning";
                case LogLevel.Information:
                    return "Info";
                case LogLevel.Debug:
                    return "Verbose";
                default:
                    return "Verbose";
            }
        }

        private string PropertyBagToJson(IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                return string.Empty;
            }

            return PropertyBagToJson(properties.Select(kv => new KeyValuePair<string, object>(kv.Key, kv.Value)));
        }

        private string PropertyBagToJson(IEnumerable<KeyValuePair<string, object>> properties)
        {
            if (properties == null)
            {
                return string.Empty;
            }

            // Serializing key/value pairs manually using JsonTextWriter in order to optimize performance.
            string serialized;
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                jsonWriter.WriteStartObject();
                foreach (var nameValuePair in properties)
                {
                    jsonWriter.WritePropertyName(nameValuePair.Key ?? string.Empty, escape: true);

                    object propertyValue = nameValuePair.Value;

                    if (propertyValue is IPrivateDataContainer privateDataContainer)
                    {
                        propertyValue = this.privacyMarker.ToCompliantValue(privateDataContainer);
                    }

                    if (propertyValue == null || IsPrimitiveType(propertyValue.GetType()))
                    {
                        jsonWriter.WriteValue(propertyValue);
                    }
                    else
                    {
                        // This is a fall back for cases when complex types are passed as trace arguments. Serialization via JsonConvert.SerializeObject() affect overall logging peformance by 2-3% (CPU and memory usage)
                        // compared to JsonTextWriter.WriteValue(). Considering rare usage of complex types as logging arguments this should not result any perf issues.
                        // However if it does it is likely due to complexity of argument type and in that case it is recommended to change the caller to do to string conversion explicitly and pass resulting string as argument.
                        jsonWriter.WriteValue(JsonConvert.SerializeObject(propertyValue, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
                    }
                }

                jsonWriter.WriteEndObject();
                serialized = stringWriter.ToString();
            }

            return serialized;
        }

        private static bool IsPrimitiveType(Type type)
        {
            // Json.NET logic to detect primitive types is internal so we cannot reuse it (https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/ConvertUtils.cs).
            // Below implementation should cover all primitive types supported by Json.NET.
            if (type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(Guid)) || type.Equals(typeof(Uri)) || type.Equals(typeof(decimal)) || type.Equals(typeof(DateTimeOffset)) || type.Equals(typeof(TimeSpan)) || type.Equals(typeof(DateTime)))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsPrimitiveType(type.GetGenericArguments()[0]);
            }

            return false;
        }

        private void LogOperation(OperationLogData operationLogData)
        {
            Contract.CheckValue(operationLogData, nameof(operationLogData));


            var activity = ServiceContext.Activity.Current ?? ServiceContext.Activity.Empty;    
            var appPrincipal = ServiceContext.Principal.Current as HCMApplicationPrincipal;
            var environmentContext = ServiceContext.Environment.Current;

            var application = environmentContext.Application;
            var service = environmentContext.Service;
            var codePackageVersion = environmentContext.CodePackageVersion;
            var sessionId = activity.SessionId.ToString();
            var rootActivityId = activity.RootActivityId.ToString();
            var activityVector = activity.ActivityVector;
            var activityType = activity.ActivityType;

            traceEventProvider.LogOperation(
                application,
                service,
                codePackageVersion,
                operationLogData.OperationName ?? activityType,
                sessionId,
                rootActivityId,
                activityVector,
                operationLogData.ResourceId,
                operationLogData.ResourceType,
                operationLogData.CallerIpAddress,
                operationLogData.OperationType,
                operationLogData.OperationVersion,
                operationLogData.ResultDescription,
                operationLogData.ResultSignature,
                operationLogData.ResultType,
                operationLogData.TargetEndpointAddress,
                operationLogData.DurationMs,
                operationLogData.ExceptionTypeName,
                operationLogData.ExceptionCustomData,
                appPrincipal?.UserObjectId ?? string.Empty);
        }

        private void LogActivity(LogLevel logLevel, EventId eventId, ActivityLogData activityLogData, Exception exception)
        {
            var activity = ServiceContext.Activity.Current ?? ServiceContext.Activity.Empty; 
            var appPrincipal = ServiceContext.Principal.Current as HCMApplicationPrincipal;
            var environmentContext = ServiceContext.Environment.Current;

            var application = environmentContext.Application;
            var service = environmentContext.Service;
            var codePackageVersion = environmentContext.CodePackageVersion;
            var sessionId = activity.SessionId.ToString();
            var rootActivityId = activity.RootActivityId.ToString();
            var activityVector = activity.ActivityVector;
            var activityType = activity.ActivityType;
            var sourceName = categoryName;
            var message = string.Empty;

            // TODO: consider using json format which can be natively queried in Kusto (https://kusto.azurewebsites.net/docs/queryLanguage/query_language_parsejsonfunction.html)
            var customProperties = activity.GetCustomProperties();

            switch (activityLogData.LogKind)
            {
                case ActivityLogKind.Start:
                    message = "ActivityStarted";

                    // Increment the count for this activity type.
                    if (!activityLogData.ActivityType.OptOutOfConcurrentMetric)
                    {
                        concurrentActivityCounts?.AddOrUpdate(activityType, 1, (key, value) => value + 1);
                    }
                    break;
                case ActivityLogKind.End:
                    var elapsedMilliseconds = activityLogData.Stopwatch.ElapsedMilliseconds;
                    string completionStatus;
                    string activityCustomPropertiesString;
                    string exceptionTypeName;
                    string exceptionCustomPropertiesString;
                    string failingChildActivityType;
                    string rootActivityType;

                    activity.GetCompletionState(
                        this,
                        exception,
                        activityLogData.ExceptionTypesToIgnore,
                        out logLevel,
                        out completionStatus,
                        out activityCustomPropertiesString,
                        out exceptionTypeName,
                        out exceptionCustomPropertiesString,
                        out failingChildActivityType,
                        out rootActivityType);

                    // TODO: migrate below code to use json-serialized property bag and 'dynamic' column in Kusto.
                    // Construct the trace message.
                    string allCustomPropertiesString = string.Empty;
                    if (!string.IsNullOrEmpty(activityCustomPropertiesString) && !string.IsNullOrEmpty(exceptionCustomPropertiesString))
                    {
                        allCustomPropertiesString = string.Concat(exceptionCustomPropertiesString, ", ", activityCustomPropertiesString);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(activityCustomPropertiesString))
                            allCustomPropertiesString = activityCustomPropertiesString;

                        if (!string.IsNullOrEmpty(exceptionCustomPropertiesString))
                            allCustomPropertiesString = exceptionCustomPropertiesString;
                    }

                    if (string.IsNullOrEmpty(failingChildActivityType))
                    {
                        if (exceptionTypeName == null)
                        {
                            message = $"ActivityCompleted: HowEnded={completionStatus}, Duration={elapsedMilliseconds} [ms], {allCustomPropertiesString}";
                        }
                        else
                        {
                            message = $"ActivityCompleted: HowEnded={completionStatus}, Duration={elapsedMilliseconds} [ms], Exception={exceptionTypeName}, {allCustomPropertiesString}";
                        }
                    }
                    else
                    {
                        if (exceptionTypeName == null)
                        {
                            message = $"ActivityCompleted: HowEnded={completionStatus}, Duration={elapsedMilliseconds} [ms], FailingChildActivity={failingChildActivityType}, {allCustomPropertiesString}";
                        }
                        else
                        {
                            message = $"ActivityCompleted: HowEnded={completionStatus}, Duration={elapsedMilliseconds} [ms], Exception={exceptionTypeName}, FailingChildActivity={failingChildActivityType}, {allCustomPropertiesString}";
                        }
                    }


                    // Decrement the count for this activity type.
                    if (!activityLogData.ActivityType.OptOutOfConcurrentMetric)
                    {
                        concurrentActivityCounts?.AddOrUpdate(activityType, 0, (key, value) => value - 1);
                    }

                    // TO-DO: Evaludate the need of logging the duration metric and uncomment the below code
                    /*
                    // Log the duration metric.
                    if (!activityLogData.ActivityType.OptOutOfDurationMetric)
                    {
                        if (!string.IsNullOrEmpty(mdmAccount) && !string.IsNullOrEmpty(activityMetricNamespace))
                        {
                            // Add default activity dimensions.
                            var customDimensions = new SortedList<string, string>()
                            {
                                { ActivityTypeDimension, activityType },
                                { CompletionStatusDimension, completionStatus },
                            };

                            if (!string.IsNullOrEmpty(rootActivityType))
                            {
                                customDimensions.Add(RootActivityType, rootActivityType);
                            }

                            if (!string.IsNullOrEmpty(failingChildActivityType))
                            {
                                customDimensions.Add(FailingChildActivityType, failingChildActivityType);
                            }

                            // Add the exceptionTypeName if not null.
                            if (exceptionTypeName != null)
                            {
                                customDimensions.Add(ExceptionTypeDimension, exceptionTypeName);
                            }

                            if (customProperties != null)
                            {
                                if (activityLogData.ActivityType.OptInPropertiesAsDimensions)
                                {
                                    // Add all custom properties as custom dimensions.
                                    foreach (var property in customProperties)
                                    {
                                        customDimensions.Add(property.Key, property.Value);
                                    }
                                }
                                else
                                {
                                    // Add only the HTTP status code custom property as a custom dimension.
                                    if (customProperties.TryGetValue(ActivityContextExtensions.HttpStatusCodeProperty, out var httpStatusCode))
                                    {
                                        customDimensions.Add(ActivityContextExtensions.HttpStatusCodeProperty, httpStatusCode);
                                    }
                                }
                            }

                            try
                            {
                                LogMetricWithDimensions(eventId, activityMetricNamespace, ActivityDurationMetricName, customDimensions, elapsedMilliseconds);
                            }
                            catch (Exception e)
                            {
                                this.LogError($"An error occurred while logging metric {ActivityDurationMetricName}: {e.ToString()}");
                            }
                        }
                        else
                        {
                            this.LogError($"{activityType} does not opt out of the {ActivityDurationMetricName} metric, but the mdmAcccount or activityMetricNamespace which are required to log metrics was not provided. Please verify that you are supplying the mdmAccount and activityMetricNamespace when adding IfxLoggerProvider to LoggerFactory.");
                        }
                    } */

                    // Log the operation by calling operationEventProvider.LogOperation() from here directly, rather than calling this.LogOperation(),
                    // because otherwise we would have to access the current activity several times.
                    traceEventProvider.LogOperation(
                        application,
                        service,
                        codePackageVersion,
                        activityType,
                        sessionId,
                        rootActivityId,
                        activityVector,
                        resourceId: string.Empty,
                        resourceType: string.Empty,
                        callerIpAddress: string.Empty,
                        operationType: string.Empty,
                        operationVersion: string.Empty,
                        resultDescription: string.Empty,
                        resultSignature: completionStatus,
                        resultType: completionStatus,
                        targetEndpointAddress: string.Empty,
                        durationMs: (uint)elapsedMilliseconds,
                        exceptionTypeName: exceptionTypeName ?? string.Empty,
                        exceptionCustomData: exceptionCustomPropertiesString ?? string.Empty,
                        userId: appPrincipal?.UserObjectId ?? string.Empty);

                    break;
            }

            // Using only integer part of EventId in order to have predictable values in Kusto.
            var sourceId = eventId.Id.ToString();
            var traceLevel = GetTraceLevelString(logLevel);

            // Call traceEventProvider.LogTrace() from here directly, rather than calling this.LogTrace(),
            // because otherwise we would have to access the current activity several times.
            // This also allows the activity's state to affect the log level.
            traceEventProvider.LogTrace(application, service, codePackageVersion, sessionId, rootActivityId, activityVector, activityType, sourceName, sourceId, traceLevel, message, appPrincipal?.UserObjectId ?? string.Empty);
        }
    }
}
