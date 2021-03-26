//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.ServicePlatform.Exceptions
{
    using TA.CommonLibrary.ServicePlatform.Tracing;
    using Microsoft.Extensions.Logging;

    public static class MonitoredExceptionExtensions
    {
        /// <summary>
        /// Trace (once and only once) to <see cref="ServicePlatformTrace"/>. 
        /// </summary>
        /// <param name="monitoredException">this</param>
        /// <returns>The current <see cref="MonitoredException"/>.</returns>
        public static T EnsureTraced<T>(this T monitoredException) where T : MonitoredException => EnsureTraced(monitoredException, ServicePlatformTrace.Instance);

        /// <summary>
        /// Trace (once and only once) to the provided <see cref="ITraceSource"/>. 
        /// </summary>
        /// <param name="monitoredException">this</param>
        /// <param name="traceSource">The <see cref="ITraceSource"/> to trace to.</param>
        /// <returns>The current <see cref="MonitoredException"/>.</returns>
        public static T EnsureTraced<T>(this T monitoredException, ITraceSource traceSource) where T : MonitoredException
        {
            if (monitoredException.MarkTraced())
            {
                TraceVerbosity verbosity = FinalizeTraceVerbosity(monitoredException);
                traceSource.Trace(verbosity, monitoredException.ToString());
            }

            return monitoredException;
        }

        /// <summary>
        /// Logs the exception details.
        /// </summary>
        /// <typeparam name="T">The instance of  <see cref="MonitoredException"/> or of derived types.</typeparam>
        /// <param name="monitoredException">The instance for <see cref="MonitoredException"/>.</param>
        /// <param name="logger">Yje instance for <see cref="ILogger"/>.</param>
        /// <returns>The instance for <see cref="MonitoredException"/>.</returns>
        public static T EnsureLogged<T>(this T monitoredException, ILogger logger) where T : MonitoredException
        {
            LogLevel logLevel;
            if (monitoredException.MarkTraced())
            {
                logLevel = FinalizeLogLevel(monitoredException);
                logger.Log(logLevel, monitoredException.ToString());
            }

            return monitoredException;
        }


        private static TraceVerbosity FinalizeTraceVerbosity<T>(T monitoredException) where T : MonitoredException
        {
            TraceVerbosity verbosity;
            switch (monitoredException.Kind)
            {
                case MonitoredExceptionKind.Benign:
                    verbosity = TraceVerbosity.Warning;
                    break;
                case MonitoredExceptionKind.Remote:
                case MonitoredExceptionKind.Service:
                case MonitoredExceptionKind.Error:
                default:
                    verbosity = TraceVerbosity.Error;
                    break;
                case MonitoredExceptionKind.Fatal:
                    verbosity = TraceVerbosity.Fatal;
                    break;
            }

            return verbosity;
        }

        private static LogLevel FinalizeLogLevel<T>(T monitoredException) where T : MonitoredException
        {
            LogLevel logLevel;
            switch (monitoredException.Kind)
            {
                case MonitoredExceptionKind.Benign:
                    logLevel = LogLevel.Warning;
                    break;
                case MonitoredExceptionKind.Remote:
                case MonitoredExceptionKind.Service:
                case MonitoredExceptionKind.Error:
                default:
                    logLevel = LogLevel.Error;
                    break;
                case MonitoredExceptionKind.Fatal:
                    logLevel = LogLevel.Critical;
                    break;
            }

            return logLevel;
        }
    }
}
