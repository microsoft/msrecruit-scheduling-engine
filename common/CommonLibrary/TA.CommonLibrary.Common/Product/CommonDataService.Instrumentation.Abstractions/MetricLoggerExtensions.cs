//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TA.CommonLibrary.CommonDataService.Instrumentation
{
    /// <summary>
    /// Extensions to ILogger for IFX Metrics using MDM Multi-Dimensional Monitoring
    /// </summary>
    public static class MetricLoggerExtensions
    {
        public static void LogMetric(this ILogger logger, string metricName, long value)
        {
            logger.LogMetric(Events.DEFAULT_METRIC, metricName, value);
        }

        public static void LogMetric(this ILogger logger, EventId eventId, string metricName, long value)
        {
            var metricLogData = new MetricLogData(metricName, value);

            logger.Log(LogLevel.Information, eventId, metricLogData, null, DefaultFormatter);
        }

        public static void LogMetric(this ILogger logger, string metricName, IDictionary<string, string> metricDimensions, long value)
        {
            logger.LogMetric(Events.DEFAULT_METRIC, metricName, metricDimensions, value);
        }

        public static void LogMetric(this ILogger logger, EventId eventId, string metricName, IDictionary<string, string> metricDimensions, long value)
        {
            var metricLogData = new MetricLogData(metricName, metricDimensions, value);

            logger.Log(LogLevel.Information, eventId, metricLogData, null, DefaultFormatter);
        }

        internal static void LogMetric(this ILogger logger, string metricNamespace, string metricName, IDictionary<string, string> metricDimensions, long value)
        {
            var metricLogData = new MetricLogData(metricName, metricNamespace, metricDimensions, value);

            logger.Log(LogLevel.Information, Events.DEFAULT_METRIC, metricLogData, null, DefaultFormatter);
        }

        internal static string DefaultFormatter(MetricLogData metricLogData, Exception exception)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"ServicePlatform Metric Name: {metricLogData.MetricName}");

            // If custom dimensions were provided add those to message.
            if (metricLogData.MetricDimensions != null && metricLogData.MetricDimensions.Count > 0)
            {
                stringBuilder.AppendLine($"Dimensions:");
                foreach (var dimension in metricLogData.MetricDimensions)
                {
                    stringBuilder.AppendLine($"{dimension.Key}: {dimension.Value}");
                }
            }

            // Don't need to log exception, it is already included in logs by ConsoleLogger and DebugLogger outside of formatter.
            // Also, the extension methods don't take in Exception as parameter to log.
            // See: https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/ConsoleLogger.cs#L157
            //      https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Debug/DebugLogger.cs#L77

            return stringBuilder.ToString();
        }
    }
}
