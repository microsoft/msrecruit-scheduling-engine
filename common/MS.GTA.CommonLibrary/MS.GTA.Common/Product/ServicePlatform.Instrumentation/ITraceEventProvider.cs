//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Product.ServicePlatform.Instrumentation
{
    /// <summary>
    /// The <see cref="ITraceEventProvider"/> provides mechanism to log.
    /// </summary>
    internal interface ITraceEventProvider
    {
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
        /// <param name="userId">Caller (User) Graph Id</param>

        void LogTrace(            
            string application,
            string service,
            string codePackageVersion,
            string sessionId,
            string rootActivityId,
            string activityVector,
            string activityType,
            string sourceName,
            string sourceId,
            string traceLevel,            
            string traceMessage,
            string userId
            );

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
        void LogOperation(
            string application,
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
            string resultSignature,
            string resultType,
            string targetEndpointAddress,
            uint durationMs,
            string exceptionTypeName,
            string exceptionCustomData,
            string userId);
    }
}
