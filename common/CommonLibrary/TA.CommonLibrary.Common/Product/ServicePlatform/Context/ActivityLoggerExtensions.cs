//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Activity = TA.CommonLibrary.ServicePlatform.Context.ServiceContext.Activity;

namespace TA.CommonLibrary.ServicePlatform.Context
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using TA.CommonLibrary.Common.Base.Configuration;
    using TA.CommonLibrary.Common.Common.Common.Base.Configuration;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions to ILogger for executing and logging activities.
    /// </summary>
    public static class ActivityLoggerExtensions
    {
        private static TelemetryClient _telemetryClient = new TelemetryClient(
            new TelemetryConfiguration(FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<LoggingConfiguration>().InstrumentationKey));

        public static void Execute(this ILogger logger, ActivityType activityType, Action action)
        {
            logger.Execute(Activity.DEFAULT_EVENTID, activityType, action, null);
        }

        public static TResult Execute<TResult>(this ILogger logger, ActivityType activityType, Func<TResult> func)
        {
            return logger.Execute(Activity.DEFAULT_EVENTID, activityType, func, null);
        }

        public static async Task ExecuteAsync(this ILogger logger, ActivityType activityType, Func<Task> action, string rootActivityId = null, string sessionId = null)
        {
            await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, activityType, action, null, rootActivityId, sessionId);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, ActivityType activityType, Func<Task<TResult>> func)
        {
            return await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, activityType, func, null);
        }

        public static void ExecuteRoot(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Action action)
        {
            logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, activityType, action, null);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Func<TResult> func)
        {
            return logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, activityType, func, null);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Func<Task> action)
        {
            await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, activityType, action, null);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Func<Task<TResult>> func)
        {
            return await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, activityType, func, null);
        }

        public static void Execute(this ILogger logger, EventId eventId, ActivityType activityType, Action action)
        {
            logger.Execute(eventId, activityType, action, null);
        }

        public static TResult Execute<TResult>(this ILogger logger, EventId eventId, ActivityType activityType, Func<TResult> func)
        {
            return logger.Execute(eventId, activityType, func, null);
        }

        public static async Task ExecuteAsync(this ILogger logger, EventId eventId, ActivityType activityType, Func<Task> action)
        {
            await logger.ExecuteAsync(eventId, activityType, action, null);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, EventId eventId, ActivityType activityType, Func<Task<TResult>> action)
        {
            return await logger.ExecuteAsync(eventId, activityType, action, null);
        }

        public static void ExecuteRoot(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Action action)
        {
            logger.ExecuteRoot(eventId, rootContext, activityType, action, null);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Func<TResult> func)
        {
            return logger.ExecuteRoot(eventId, rootContext, activityType, func, null);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Func<Task> action)
        {
            await logger.ExecuteRootAsync(eventId, rootContext, activityType, action, null);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Func<Task<TResult>> action)
        {
            return await logger.ExecuteRootAsync(eventId, rootContext, activityType, action, null);
        }

        public static void Execute(this ILogger logger, ActivityType activityType, Action action, Type[] exceptionTypesToIgnore)
        {
            logger.Execute(Activity.DEFAULT_EVENTID, activityType, action, exceptionTypesToIgnore);
        }

        public static TResult Execute<TResult>(this ILogger logger, ActivityType activityType, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            return logger.Execute(Activity.DEFAULT_EVENTID, activityType, func, exceptionTypesToIgnore);
        }

        public static async Task ExecuteAsync(this ILogger logger, ActivityType activityType, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, activityType, action, exceptionTypesToIgnore);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, ActivityType activityType, Func<Task<TResult>> func, Type[] exceptionTypesToIgnore)
        {
            return await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, activityType, func, exceptionTypesToIgnore);
        }

        public static void ExecuteRoot(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Action action, Type[] exceptionTypesToIgnore)
        {
            logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, activityType, action, exceptionTypesToIgnore);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            return logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, activityType, func, exceptionTypesToIgnore);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, activityType, action, exceptionTypesToIgnore);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, RootExecutionContext rootContext, ActivityType activityType, Func<Task<TResult>> func, Type[] exceptionTypesToIgnore)
        {
            return await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, activityType, func, exceptionTypesToIgnore);
        }

        public static void Execute(this ILogger logger, EventId eventId, ActivityType activityType, Action action, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(action, nameof(action));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, eventId, ActivityLogData.Start(activityType));
                    action();
                    logger.Log(LogLevel.Information, eventId, ActivityLogData.End(activityType, stopwatch));
                }
                catch (Exception ex)
                {
                    SetFailingChildActivityType(exceptionTypesToIgnore, activityType, ex);
                    Dictionary<string, string> properties = new Dictionary<string, string>();
                    properties.Add("RootActivityId", ServiceContext.Activity.Current?.RootActivityId.ToString());
                    properties.Add("SessionId", ServiceContext.Activity.Current?.SessionId.ToString());

                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static TResult Execute<TResult>(this ILogger logger, EventId eventId, ActivityType activityType, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(func, nameof(func));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    var res = func();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));

                    return res;
                }
                catch (Exception ex)
                {
                    SetFailingChildActivityType(exceptionTypesToIgnore, activityType, ex);
                    Dictionary<string, string> properties = new Dictionary<string, string>();
                    properties.Add("RootActivityId", ServiceContext.Activity.Current?.RootActivityId.ToString());
                    properties.Add("SessionId", ServiceContext.Activity.Current?.SessionId.ToString());
                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static async Task ExecuteAsync(this ILogger logger, EventId eventId, ActivityType activityType, Func<Task> action, Type[] exceptionTypesToIgnore, string rootActivityId = null, string sessionId = null)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(action, nameof(action));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(activityType), logger, rootActivityId, sessionId))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    await action();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));
                }
                catch (Exception ex)
                {
                    SetFailingChildActivityType(exceptionTypesToIgnore, activityType, ex);

                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, EventId eventId, ActivityType activityType, Func<Task<TResult>> action, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(action, nameof(action));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    var res = await action();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));
                    return res;
                }
                catch (Exception ex)
                {
                    SetFailingChildActivityType(exceptionTypesToIgnore, activityType, ex);

                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static void ExecuteRoot(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Action action, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(rootContext, nameof(rootContext));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(action, nameof(action));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(rootContext, activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    action();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(rootContext, nameof(rootContext));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(func, nameof(func));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(rootContext, activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    var res = func();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));

                    return res;
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static async Task ExecuteRootAsync(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(rootContext, nameof(rootContext));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(action, nameof(action));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(rootContext, activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    await action();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));

                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, ActivityType activityType, Func<Task<TResult>> action, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(rootContext, nameof(rootContext));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(action, nameof(action));

            using (ServiceContext.Activity.GetActivityContext(new ActivityScope(rootContext, activityType), logger))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    logger.Log(LogLevel.Information, ActivityLogData.Start(activityType));
                    var res = await action();
                    logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));

                    return res;
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                    throw;
                }
            }
        }

        public static Stopwatch LogActivityStart(this ILogger logger, ActivityType activityType)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            var stopwatch = Stopwatch.StartNew();
            try
            {                
                logger.Log(LogLevel.Information,ActivityLogData.Start(activityType));
                return stopwatch;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                throw;
            }
        }

        public static void LogActivityEnd(this ILogger logger, ActivityType activityType, Stopwatch stopwatch)
        {
            logger.LogActivityEnd(activityType, stopwatch, null);
        }

        public static void LogActivityEnd(this ILogger logger, ActivityType activityType, Stopwatch stopwatch, Type[] exceptionTypesToIgnore)
        {
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(activityType, nameof(activityType));
            TA.CommonLibrary.CommonDataService.Common.Internal.Contract.CheckValue(stopwatch, nameof(stopwatch));
            try
            {
                logger.Log(LogLevel.Information, ActivityLogData.End(activityType, stopwatch));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ActivityLogData.End(activityType, stopwatch, null, ex));
                throw;
            }
        }

        #region String versions of Execute, ExecuteAsync, and ExecuteRoot
        public static void Execute(this ILogger logger, string activityName, Action action)
        {
            logger.Execute(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), action, null);
        }

        public static TResult Execute<TResult>(this ILogger logger, string activityName, Func<TResult> func)
        {
            return logger.Execute(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), func, null);
        }

        public static async Task ExecuteAsync(this ILogger logger, string activityName, Func<Task> action)
        {
            await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), action, null);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, string activityName, Func<Task<TResult>> func)
        {
            return await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), func, null);
        }

        public static void ExecuteRoot(this ILogger logger, RootExecutionContext rootContext, string activityName, Action action)
        {
            logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), action, null);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, RootExecutionContext rootContext, string activityName, Func<TResult> func)
        {
            return logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), func, null);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, RootExecutionContext rootContext, string activityName, Func<Task> action)
        {
            await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), action, null);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, RootExecutionContext rootContext, string activityName, Func<Task<TResult>> func)
        {
            return await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), func, null);
        }

        public static void Execute(this ILogger logger, EventId eventId, string activityName, Action action)
        {
            logger.Execute(eventId, new RuntimeActivityType(activityName), action, null);
        }

        public static TResult Execute<TResult>(this ILogger logger, EventId eventId, string activityName, Func<TResult> func)
        {
            return logger.Execute(eventId, new RuntimeActivityType(activityName), func, null);
        }

        public static async Task ExecuteAsync(this ILogger logger, EventId eventId, string activityName, Func<Task> action)
        {
            await logger.ExecuteAsync(eventId, new RuntimeActivityType(activityName), action, null);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, EventId eventId, string activityName, Func<Task<TResult>> action)
        {
            return await logger.ExecuteAsync(eventId, new RuntimeActivityType(activityName), action, null);
        }

        public static void ExecuteRoot(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Action action)
        {
            logger.ExecuteRoot(eventId, rootContext, new RuntimeActivityType(activityName), action, null);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Func<TResult> func)
        {
            return logger.ExecuteRoot(eventId, rootContext, new RuntimeActivityType(activityName), func, null);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Func<Task> action)
        {
            await logger.ExecuteRootAsync(eventId, rootContext, new RuntimeActivityType(activityName), action, null);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Func<Task<TResult>> func)
        {
            return await logger.ExecuteRootAsync(eventId, rootContext, new RuntimeActivityType(activityName), func, null);
        }

        public static void Execute(this ILogger logger, string activityName, Action action, Type[] exceptionTypesToIgnore)
        {
            logger.Execute(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static TResult Execute<TResult>(this ILogger logger, string activityName, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            return logger.Execute(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }

        public static async Task ExecuteAsync(this ILogger logger, string activityName, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, string activityName, Func<Task<TResult>> func, Type[] exceptionTypesToIgnore)
        {
            return await logger.ExecuteAsync(Activity.DEFAULT_EVENTID, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }

        public static void ExecuteRoot(this ILogger logger, RootExecutionContext rootContext, string activityName, Action action, Type[] exceptionTypesToIgnore)
        {
            logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, RootExecutionContext rootContext, string activityName, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            return logger.ExecuteRoot(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, RootExecutionContext rootContext, string activityName, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, RootExecutionContext rootContext, string activityName, Func<Task<TResult>> func, Type[] exceptionTypesToIgnore)
        {
            return await logger.ExecuteRootAsync(Activity.DEFAULT_EVENTID, rootContext, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }

        public static void Execute(this ILogger logger, EventId eventId, string activityName, Action action, Type[] exceptionTypesToIgnore)
        {
            logger.Execute(eventId, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static TResult Execute<TResult>(this ILogger logger, EventId eventId, string activityName, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            return logger.Execute(eventId, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }

        public static async Task ExecuteAsync(this ILogger logger, EventId eventId, string activityName, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            await logger.ExecuteAsync(eventId, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this ILogger logger, EventId eventId, string activityName, Func<Task<TResult>> action, Type[] exceptionTypesToIgnore)
        {
            return await logger.ExecuteAsync(eventId, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static void ExecuteRoot(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Action action, Type[] exceptionTypesToIgnore)
        {
            logger.ExecuteRoot(eventId, rootContext, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static TResult ExecuteRoot<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Func<TResult> func, Type[] exceptionTypesToIgnore)
        {
            return logger.ExecuteRoot(eventId, rootContext, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }

        public static async Task ExecuteRootAsync(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Func<Task> action, Type[] exceptionTypesToIgnore)
        {
            await logger.ExecuteRootAsync(eventId, rootContext, new RuntimeActivityType(activityName), action, exceptionTypesToIgnore);
        }

        public static async Task<TResult> ExecuteRootAsync<TResult>(this ILogger logger, EventId eventId, RootExecutionContext rootContext, string activityName, Func<Task<TResult>> func, Type[] exceptionTypesToIgnore)
        {
            return await logger.ExecuteRootAsync(eventId, rootContext, new RuntimeActivityType(activityName), func, exceptionTypesToIgnore);
        }
        #endregion

        private static void SetFailingChildActivityType(Type[] exceptionTypesToIgnore, ActivityType activityType, Exception ex)
        {
            if (ServiceContext.Activity.Current.FailingChildActivityType == null)
            {
                if (exceptionTypesToIgnore != null)
                {
                    var types = exceptionTypesToIgnore.Where(exceptionTypeToIgnore => exceptionTypeToIgnore.IsAssignableFrom(ex?.GetType()));

                    if (!types.Any())
                    {
                        ServiceContext.Activity.Current.FailingChildActivityType = activityType.Name;
                    }
                }
                else
                {
                    ServiceContext.Activity.Current.FailingChildActivityType = activityType.Name;
                }
            }
        }

        internal static string DefaultFormatter(ActivityLogData activityLogData, Exception exception)
        {
            var stringBuilder = new StringBuilder();
            switch (activityLogData.LogKind)
            {
                case ActivityLogKind.Start:
                    stringBuilder.AppendLine($"ActivityStarted: {activityLogData.ActivityType.Name}");
                    break;
                case ActivityLogKind.End:
                    stringBuilder.AppendLine($"ActivityCompleted: {activityLogData.ActivityType.Name}, Duration={activityLogData.Stopwatch.ElapsedMilliseconds} [ms]");
                    break;
            }

            // Don't need to log exception, it is already included in logs by ConsoleLogger and DebugLogger outside of formatter.
            // Also, the extension methods don't take in Exception as parameter to log.
            // See: https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/ConsoleLogger.cs#L157
            //      https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Debug/DebugLogger.cs#L77

            return stringBuilder.ToString();
        }
    }
}
