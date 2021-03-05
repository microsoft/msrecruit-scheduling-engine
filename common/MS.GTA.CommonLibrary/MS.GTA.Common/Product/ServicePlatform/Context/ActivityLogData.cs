//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using CommonDataService.Common.Internal;
using ServicePlatform.Exceptions;

namespace ServicePlatform.Context
{
    public enum ActivityLogKind
    {
        Start,
        End
    }

    public sealed class ActivityLogData
    {
        public ActivityLogKind LogKind { get; }

        public ActivityType ActivityType { get; }

        public Stopwatch Stopwatch { get; }

        public Type[] ExceptionTypesToIgnore { get; }

        private ActivityLogData(ActivityLogKind logKind, ActivityType activityType, Stopwatch stopwatch = null, Type[] exceptionsTypesToIgnore = null)
        {
            LogKind = logKind;
            ActivityType = activityType;
            Stopwatch = stopwatch;
            ExceptionTypesToIgnore = exceptionsTypesToIgnore;
        }

        public static string Start(ActivityType activityType)
        {
            Contract.AssertValue(activityType, nameof(activityType));

            string message = $"ActivityStarted: {activityType.Name}";

            return message;
        }

        public static string End(ActivityType activityType, Stopwatch stopwatch, Type[] exceptionsTypesToIgnore = null, Exception exception = null)
        {
            Contract.AssertValue(activityType, nameof(activityType));
            Contract.AssertValue(stopwatch, nameof(stopwatch));
            string message = $"ActivityCompleted: {activityType.Name}, ";
            string completionState = string.Empty;
            LogLevel logLevel = LogLevel.Information;
            string exceptionCustomPropertiesString = string.Empty;
            string exceptionTypeName = string.Empty;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (exception == null)
            {
                completionState = "Success:";
                logLevel = LogLevel.Information;
            }
            else
            {
                if (exception != null)
                {
                    message += $"\nHiding activity provided exception '{exception.GetType().Name}' because another exception was thrown later";
                }

                completionState = "Failure:";
                logLevel = LogLevel.Error;
                exceptionTypeName = exception.GetType().Name;

                if (exception is MonitoredException monitoredException)
                {
                    var mexCustomProperties = monitoredException.GetCustomData().Select(cd => $"{cd.Name}={cd.Value}");
                    exceptionCustomPropertiesString = mexCustomProperties != null ? string.Join(", ", mexCustomProperties) : null;

                    if (monitoredException?.RemoteServiceError?.PropagateError == true)
                    {
                        logLevel = LogLevel.Warning;
                        completionState = "SuccessDespiteError";
                    }
                    else
                    {
                        switch (monitoredException.Kind)
                        {
                            case MonitoredExceptionKind.Benign:
                                logLevel = LogLevel.Warning;
                                completionState = "SuccessDespiteError";
                                break;

                            case MonitoredExceptionKind.Remote:
                                logLevel = LogLevel.Error;
                                completionState = "RemoteError";
                                break;

                            case MonitoredExceptionKind.Service:
                                logLevel = LogLevel.Error;
                                //// Failure
                                break;

                            case MonitoredExceptionKind.Fatal:
                                logLevel = LogLevel.Critical;
                                //// Failure
                                break;
                        }
                    }
                }
                else
                {
                    message += $"Activity failing with unmonitored exception: {exception.ToString()}";
                }
            }

            if (!string.IsNullOrEmpty(exceptionTypeName))
            {
                message += $"\nHowEnded={completionState}, LogLevel={logLevel}, Duration={elapsedMilliseconds} [ms], Exception: {exceptionTypeName}";
            }
            else
            {
                message += $"\nHowEnded={completionState}, LogLevel={logLevel}, Duration={elapsedMilliseconds} [ms]";
            }

            return message;
        }
    }
}
