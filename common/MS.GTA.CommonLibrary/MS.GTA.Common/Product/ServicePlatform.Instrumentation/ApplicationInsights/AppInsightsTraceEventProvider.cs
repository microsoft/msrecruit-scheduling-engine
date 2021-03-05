//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Product.ServicePlatform.Instrumentation.ApplicationInsights
{
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using CommonDataService.Common;

    /// <summary>
    /// The <see cref="AppInsightsTraceEventProvider"/> defines the trace event provider.
    /// </summary>
    /// <seealso cref="Common.Product.ServicePlatform.Instrumentation.ITraceEventProvider" />
    internal sealed class AppInsightsTraceEventProvider : ITraceEventProvider
    {
        private static string instrumentationKey = string.Empty;
        private static readonly object instanceLock = new object();
        private static ITraceEventProvider instance;
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Prevents a default instance of the <see cref="AppInsightsTraceEventProvider"/> class from being created.
        /// </summary>
        private AppInsightsTraceEventProvider()
        {
            this.telemetryClient = new TelemetryClient(new TelemetryConfiguration(InstrumentationKey));
        }

        /// <summary>
        /// Gets or sets the instrumentation key.
        /// </summary>
        /// <value>
        /// The instrumentation key.
        /// </value>
        public static string InstrumentationKey
        {
            get
            {
                return instrumentationKey;
            }
            set
            {
                lock (instanceLock)
                {
                    instrumentationKey = value;
                }
            }
        }

        /// <summary>
        /// Gets an singleton instance of an <see cref="ITraceEventProvider"/>.
        /// </summary>
        /// <param name="instrumentationKey">The instrumention key.</param>
        /// <returns>An event provider.</returns>
        public static ITraceEventProvider GetInstance(string instrumentationKey)
        {            
            Contract.CheckValueOrNull(instrumentationKey, nameof(instrumentationKey));

            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        InstrumentationKey = instrumentationKey;
                        instance = new AppInsightsTraceEventProvider();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// Logs the operation.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="service">The service.</param>
        /// <param name="codePackageVersion">The code package version.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="rootActivityId">The root activity identifier.</param>
        /// <param name="activityVector">The activity vector.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="callerIpAddress">The caller IP address.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <param name="operationVersion">The operation version.</param>
        /// <param name="resultDescription">The result description.</param>
        /// <param name="resultSignature">The result signature.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="targetEndpointAddress">The target endpoint address.</param>
        /// <param name="durationMs">The duration in milliseconds.</param>
        /// <param name="exceptionTypeName">Name of the exception type.</param>
        /// <param name="exceptionCustomData">The exception custom data.</param>
        /// <param name="userId">User Id</param>
        public void LogOperation(string application,
            string service,
            string codePackageVersion,
            string operationName,
            string sessionId,
            string rootActivityId,
            string activityVector,
            string resourceId,
            string resourceType,
            string callerIpAddress,
            string operationType,
            string operationVersion,
            string resultDescription,
            string resultSignature, string resultType, string targetEndpointAddress, uint durationMs, string exceptionTypeName, string exceptionCustomData, string userId)
        {
            Dictionary<string, string> custProperties = new Dictionary<string, string>();
            custProperties.Add("Application", application);
            custProperties.Add("Service", service);
            custProperties.Add("CodePackageVersion", codePackageVersion);
            custProperties.Add("SessionId", sessionId);
            custProperties.Add("RootActivityId", rootActivityId);
            custProperties.Add("ActivityVector", activityVector);
            custProperties.Add("ResourceId", resourceId);
            custProperties.Add("ResourceType", resourceType);
            custProperties.Add("CallerIpAddress", callerIpAddress);
            custProperties.Add("ResultDescription", resultDescription);
            custProperties.Add("ResultType", resultType);
            custProperties.Add("DurationMs", durationMs.ToString());
            custProperties.Add("ExceptionTypeName", exceptionTypeName);
            custProperties.Add("ExceptionCustomData", exceptionCustomData);
            custProperties.Add("UserId", userId);
            this.telemetryClient.TrackEvent(operationName, custProperties);
        }

        /// <summary>
        /// Logs the trace meesage.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="service">The service.</param>
        /// <param name="codePackageVersion">The code package version.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="rootActivityId">The root activity identifier.</param>
        /// <param name="activityVector">The activity vector.</param>
        /// <param name="activityType">Type of the activity.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="sourceId">The source identifier.</param>
        /// <param name="traceLevel">The trace level.</param>        
        /// <param name="traceMessage">The trace message.</param> 
        /// <param name="userId">User Id</param>
        public void LogTrace(string application, string service, string codePackageVersion, string sessionId, string rootActivityId, string activityVector, string activityType, string sourceName, string sourceId, string traceLevel,string traceMessage, string userId)
        {
            Dictionary<string, string> custProperties = new Dictionary<string, string>();
            custProperties.Add("Application", application);
            custProperties.Add("Service", service);
            custProperties.Add("CodePackageVersion", codePackageVersion);
            custProperties.Add("SessionId", sessionId);
            custProperties.Add("RootActivityId", rootActivityId);
            custProperties.Add("ActivityVector", activityVector);
            custProperties.Add("ActivityType", activityType);
            custProperties.Add("SourceName", sourceName);
            custProperties.Add("SourceId", sourceId);
            custProperties.Add("UserId", userId);
            this.telemetryClient.TrackTrace(traceMessage, GetSeverityLevelFromString(traceLevel), custProperties);
        }

        private SeverityLevel GetSeverityLevelFromString(string traceLevel)
        {
            switch (traceLevel.Trim())
            {
                case "Fatal":
                    return SeverityLevel.Critical;
                case "Error":
                    return SeverityLevel.Error;
                case "Warning":
                    return SeverityLevel.Warning;
                case "Info":
                    return SeverityLevel.Information;
                case "Verbose":
                default:
                    return SeverityLevel.Verbose;
            }
        }
    }
}
