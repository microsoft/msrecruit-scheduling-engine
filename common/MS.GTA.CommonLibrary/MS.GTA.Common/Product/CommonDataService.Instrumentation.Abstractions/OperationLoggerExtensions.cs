// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Text;
using Microsoft.Extensions.Logging;
using MS.GTA.CommonDataService.Instrumentation;

namespace Microsoft.CommonDataService.Instrumentation
{
    public static class OperationLoggerExtensions
    {
        public static void LogOperation(
            this ILogger logger,
            string operationName,
            string resultSignature,
            string resultType,
            uint durationMs,
            string customProperties,
            string exceptionTypeName,
            string exceptionCustomData)
        {
            logger.LogOperation(
                Events.DEFAULT_OPERATION,
                operationName,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                resultSignature,
                resultType,
                string.Empty,
                durationMs,
                customProperties,
                exceptionTypeName,
                exceptionCustomData);
        }

        public static void LogOperation(
            this ILogger logger,
            string operationName,
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
            string customProperties,
            string exceptionTypeName,
            string exceptionCustomData)
        {
            logger.LogOperation(
                Events.DEFAULT_OPERATION,
                operationName,
                resourceId,
                resourceType,
                callerIpAddress,
                operationType,
                operationVersion,
                resultDescription,
                resultSignature,
                resultType,
                targetEndpointAddress,
                durationMs,
                customProperties,
                exceptionTypeName,
                exceptionCustomData);
        }

        public static void LogOperation(
            this ILogger logger,
            EventId eventId,
            string operationName,
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
            string customProperties,
            string exceptionTypeName,
            string exceptionCustomData)
        {
            var operationLogData = new OperationLogData(
                operationName,
                resourceId,
                resourceType,
                callerIpAddress,
                operationType,
                operationVersion,
                resultDescription,
                resultSignature,
                resultType,
                targetEndpointAddress,
                durationMs,
                customProperties,
                exceptionTypeName,
                exceptionCustomData);

            logger.Log(LogLevel.Information, eventId, operationLogData, null, DefaultFormatter);
        }

        internal static string DefaultFormatter(OperationLogData operationLogData, Exception exception)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"ServicePlatform Operation:");
            stringBuilder.AppendLine(operationLogData.ToString());

            return stringBuilder.ToString();
        }
    }
}
