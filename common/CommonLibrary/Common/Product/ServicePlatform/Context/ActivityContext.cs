//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonDataService.Common.Internal;
using ServicePlatform.Exceptions;
using ServicePlatform.Flighting;

using ServicePlatform.Tracing;
using Microsoft.Extensions.Logging;

namespace ServicePlatform.Context
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed partial class ServiceContext
    {
        // Static context APIs
        public sealed partial class Activity
        {
            public const int DEFAULT_EVENTID = 300;

            private const int AvailableLiterals = '9' - '0' + 'Z' - 'A' + 2;
            private const int MaxSequenceId = AvailableLiterals * AvailableLiterals;
            private static readonly Activity empty = new Activity(
                rootActivityContext: new RootActivityContext(Guid.Empty, Guid.Empty),
                activityType: EmptyActivityType.Instance,
                activityVector: string.Empty);

            internal static Activity Empty
            {
                get { return empty; }
            }

            public static Activity Current
            {
                get
                {
                    var currentContext = CurrentContext;
                    if (currentContext == null)
                        return null;

                    return currentContext.activityContext;
                }
            }

            [Obsolete("Instead use logger.Execute in ActivityLoggerExtensions.")]
            public static void Execute<TActivityType>(TActivityType activityType, Action action)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                logger.Execute(activityType, action);
            }

            [Obsolete("Instead use logger.Execute in ActivityLoggerExtensions.")]
            public static TResult Execute<TActivityType, TResult>(TActivityType activityType, Func<TResult> func)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                return logger.Execute<TResult>(activityType, func);
            }

            [Obsolete("Instead use logger.ExecuteAsync in ActivityLoggerExtensions.")]
            public static async Task ExecuteAsync<TActivityType>(TActivityType activityType, Func<Task> action, string rootActivityId = null, string sessionId = null)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                await logger.ExecuteAsync(activityType, action, rootActivityId, sessionId);
            }

            [Obsolete("Instead use logger.ExecuteAsync in ActivityLoggerExtensions.")]
            public static async Task<TResult> ExecuteAsync<TActivityType, TResult>(TActivityType activityType, Func<Task<TResult>> func)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                return await logger.ExecuteAsync<TResult>(activityType, func);
            }

            [Obsolete("Instead use logger.ExecuteRoot in ActivityLoggerExtensions.")]
            public static void ExecuteRoot<TActivityType>(RootExecutionContext rootContext, TActivityType activityType, Action action)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                logger.ExecuteRoot(rootContext, activityType, action);
            }

            [Obsolete("Instead use logger.ExecuteRoot in ActivityLoggerExtensions.")]
            public static TResult ExecuteRoot<TActivityType, TResult>(RootExecutionContext rootContext, TActivityType activityType, Func<TResult> func)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                return logger.ExecuteRoot<TResult>(rootContext, activityType, func);
            }

            [Obsolete("Instead use logger.ExecuteRootAsync in ActivityLoggerExtensions.")]
            public static async Task ExecuteRootAsync<TActivityType>(RootExecutionContext rootContext, TActivityType activityType, Func<Task> action)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                await logger.ExecuteRootAsync(rootContext, activityType, action);
            }

            [Obsolete("Instead use logger.ExecuteRootAsync in ActivityLoggerExtensions.")]
            public static async Task<TResult> ExecuteRootAsync<TActivityType, TResult>(RootExecutionContext rootContext, TActivityType activityType, Func<Task<TResult>> func)
                where TActivityType : ActivityType
            {
                ILogger logger = TraceSourceMeta.LoggerFactory.CreateLogger(activityType.Name);
                return await logger.ExecuteRootAsync<TResult>(rootContext, activityType, func);
            }

            internal static IDisposable GetActivityContext(ActivityScope activityScope, ILogger logger, string rootActivityId = null, string sessionId = null)
            {
                var currentActivity = Current;

                if (activityScope.RootExecutionContext == null)
                {
                    var activityContext = new Activity(currentActivity, activityScope.ActivityType, logger, rootActivityId, sessionId);
                    return ServiceContext.Push(activityContext);
                }
                else
                {
                    var activityContext = new Activity(
                        new RootActivityContext(activityScope.RootExecutionContext.SessionId, activityScope.RootExecutionContext.RootActivityId),
                        activityScope.ActivityType,
                        currentActivity?.customProperties,
                        activityScope.RootExecutionContext.ActivityVector);

                    var suppressedVerbosity = activityScope.RootExecutionContext.SuppressedTraceVebosity;
                    var forcedVerbosity = activityScope.RootExecutionContext.ForcedTraceVerbosity;

                    Tracing tracingContext = null;
                    if (suppressedVerbosity.HasValue || forcedVerbosity.HasValue)
                    {
                        tracingContext = new Tracing(
                            suppressedVerbosity.HasValue ? suppressedVerbosity.Value : TraceVerbosity.SuppressNothing,
                            forcedVerbosity.HasValue ? forcedVerbosity.Value : TraceVerbosity.ForceNothing);
                    }

                    Flights flightsContext = null;
                    if (activityScope.RootExecutionContext.EnabledFlightNames != null)
                    {
                        var flights = Flight.ConvertFlightNamesToFlights(activityScope.RootExecutionContext.EnabledFlightNames);
                        flightsContext = new ServiceContext.Flights(flights);
                    }

                    return ServiceContext.Push(activityScope.RootExecutionContext.EnvironmentContext, activityScope.RootExecutionContext.Principal, flightsContext, activityContext, tracingContext);
                }
            }

            internal void GetCompletionState(
                ILogger logger,
                Exception exception,
                Type[] exceptionsTypesToIgnore,
                out LogLevel logLevel,
                out string completionState,
                out string activityCustomPropertiesString,
                out string exceptionTypeName,
                out string exceptionCustomPropertiesString,
                out string failingChildActivityType,
                out string rootActivityType)
            {
                activityCustomPropertiesString = customProperties != null ? string.Join(", ", customProperties.Select(p => string.Concat(p.Key, "=", p.Value))) : null;
                failingChildActivityType = this.FailingChildActivityType;
                rootActivityType = this.RootActivityType;

                exception = exception ?? this.exception;
                if (exception == null)
                {
                    logLevel = LogLevel.Information;
                    completionState = "Success";
                    exceptionTypeName = null;
                    exceptionCustomPropertiesString = null;
                }
                else
                {
                    if (this.exception != null && exception != this.exception)
                    {
                        logger.LogError(
                            "Hiding activity provided exception '{0}' because another exception was thrown later",
                            this.exception.GetType().Name);
                    }

                    logLevel = LogLevel.Error;
                    completionState = "Failure";

                    exceptionCustomPropertiesString = null;
                    exceptionTypeName = exception.GetType().Name;

                    if (exception is MonitoredException monitoredException)
                    {
                        monitoredException.EnsureTraced();

                        var mexCustomProperties = monitoredException.GetCustomData().Select(cd => $"{cd.Name}={cd.Value}");
                        exceptionCustomPropertiesString = mexCustomProperties != null ? string.Join(", ", mexCustomProperties) : null;

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
                    else
                    {
                        logger.LogError("Activity failing with unmonitored exception: {0}", exception.ToString());
                    }

                    if (completionState != "SuccessDespiteError"
                        && exceptionsTypesToIgnore != null)
                    {
                        var types = exceptionsTypesToIgnore.Where(exceptionTypeToIgnore => exceptionTypeToIgnore.IsAssignableFrom(exception?.GetType()));

                        if (types.Any())
                        {
                            logger.LogWarning($"Marking activity as SuccessDespiteError as Exception {exception.GetType().Name} is of type {string.Join(",", types.Select(t => t.Name).ToArray())}");
                            completionState = "SuccessDespiteError";
                        }
                    }
                }
            }

            private static string GetNextActivityVector(Activity activity)
            {
                Contract.AssertValue(activity, nameof(activity));

                int sequenceId = (Interlocked.Increment(ref activity.nextSequenceId) - 1) % MaxSequenceId;
                int sequenceIndexHigh = sequenceId / AvailableLiterals;
                int sequenceIndexLow = sequenceId - (sequenceIndexHigh * AvailableLiterals);
                int sequenceCharHigh = sequenceIndexHigh + '0';
                int sequenceCharLow = sequenceIndexLow + '0';
                sequenceCharHigh += (sequenceCharHigh > '9') ? 'A' - '9' - 1 : 0;
                sequenceCharLow += (sequenceCharLow > '9') ? 'A' - '9' - 1 : 0;

                var sequenceString = new char[3];
                sequenceString[0] = '.';
                sequenceString[1] = (char)sequenceCharHigh;
                sequenceString[2] = (char)sequenceCharLow;

                return activity.activityVector + new string(sequenceString);
            }
        }

        // Instance implementation
        public sealed partial class Activity
        {
            private readonly RootActivityContext rootActivityContext;
            private readonly string activityVector;
            private readonly string activityType;
            private readonly Activity parentActivity;

            private int nextSequenceId;
            private ConcurrentDictionary<string, string> customProperties;
            private Exception exception;
            private string failingChildActivityType;

            // Keep these constructors internal. Consumers need to go through the static APIs provided on ServiceContext
            internal Activity(RootActivityContext rootActivityContext, ActivityType activityType, string activityVector = null) : this(rootActivityContext, activityType, null, activityVector)
            {
            }

            // Keep these constructors internal. Consumers need to go through the static APIs provided on ServiceContext
            internal Activity(RootActivityContext rootActivityContext, ActivityType activityType, ConcurrentDictionary<string, string> customProperties, string activityVector = null)
            {
                Contract.AssertValue(rootActivityContext, nameof(rootActivityContext));
                Contract.AssertValue(activityType, nameof(activityType));
                Contract.AssertValueOrNull(customProperties, nameof(customProperties));
                Contract.AssertValueOrNull(activityVector, nameof(activityVector));

                this.rootActivityContext = rootActivityContext;
                this.activityVector = activityVector ?? "00";
                this.activityType = activityType.Name;

                if (customProperties != null && customProperties.Any())
                    this.customProperties = new ConcurrentDictionary<string, string>(customProperties.ToDictionary(a => a.Key, b => b.Value));
            }

            internal Activity(Activity parentActivity, ActivityType activityType, ILogger logger, string rootActivityId = null, string sessionId = null)
            {
                Contract.AssertValueOrNull(parentActivity, nameof(parentActivity));
                Contract.AssertValue(activityType, nameof(activityType));

                if (parentActivity != null)
                {
                    this.rootActivityContext = parentActivity.rootActivityContext;
                    this.parentActivity = parentActivity;
                    this.activityVector = GetNextActivityVector(parentActivity);
                    this.activityType = activityType.Name;

                    if (parentActivity.customProperties != null && parentActivity.customProperties.Any())
                        this.customProperties = new ConcurrentDictionary<string, string>(parentActivity.customProperties.ToDictionary(a => a.Key, b => b.Value));
                }
                else
                {
                    this.rootActivityContext = new RootActivityContext(String.IsNullOrEmpty(sessionId) ? Guid.NewGuid() : Guid.Parse(sessionId), String.IsNullOrEmpty(rootActivityId) ? Guid.NewGuid() : Guid.Parse(rootActivityId));
                    this.activityVector = string.Empty;
                    this.activityType = activityType.Name;
                    logger.LogInformation("Parent activity is null. Adding new RootActivityContext.");
                }
            }

            public Guid SessionId => rootActivityContext.SessionId;

            public Guid RootActivityId => rootActivityContext.RootActivityId;

            public string ActivityVector => activityVector;

            public string ActivityType => activityType;

            public string FailingChildActivityType
            {
                get => failingChildActivityType;
                set
                {
                    if (this.parentActivity != null)
                    {
                        this.parentActivity.FailingChildActivityType = value;
                    }

                    this.failingChildActivityType = value;
                }
            }

            public string RootActivityType
            {
                get
                {
                    if (this.parentActivity != null && this.parentActivity.ActivityType != "SP.EXTRQ")
                    {
                        return this.parentActivity.RootActivityType;
                    }

                    return this.activityType;
                }
            }

            public void AddCustomProperty(string name, string value)
            {
                Contract.CheckNonEmpty(name, nameof(name));
                Contract.CheckValueOrNull(value, nameof(value));

                if (customProperties == null)
                    Interlocked.CompareExchange(ref customProperties, new ConcurrentDictionary<string, string>(), null);

                customProperties.AddOrUpdate(name, key => value, (key, oldValue) => value);
            }

            internal void FailWith(Exception exception)
            {
                Contract.AssertValue(exception, nameof(exception));

                var previous = Interlocked.Exchange(ref this.exception, exception);
                if (previous != null)
                {
                    ServicePlatformTrace.Instance.TraceError(String.Format(
                        "Hiding previous activity provided exception '{0}' because another one was set-up",
                        exception.GetType().Name));
                }
            }

            internal ConcurrentDictionary<string, string> GetCustomProperties()
            {
                return customProperties;
            }

            [ImmutableObject(true)]
            internal sealed class RootActivityContext
            {
                private static readonly RootActivityContext empty = new RootActivityContext(
                    sessionId: Guid.Empty,
                    rootActivityId: Guid.Empty);

                internal RootActivityContext(Guid sessionId, Guid rootActivityId)
                {
                    SessionId = sessionId;
                    RootActivityId = rootActivityId;
                }

                internal RootActivityContext(Activity original)
                {
                    Contract.AssertValue(original, nameof(original));

                    SessionId = original != null ? original.SessionId : Guid.Empty;
                    RootActivityId = original != null ? original.RootActivityId : Guid.Empty;
                }

                internal static RootActivityContext Empty
                {
                    get { return empty; }
                }

                internal Guid SessionId { get; }

                internal Guid RootActivityId { get; }
            }
        }
    }

    /// <summary>
    /// Base class for all activity types, defines the activity name.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class ActivityType
    {
        // This should stay internal
        internal ActivityType(string name, ActivityKind kind = ActivityKind.InternalCall)
        {
            Contract.CheckNonEmpty(name, nameof(name));

            Name = name.Chop(64);
            Kind = kind;
        }

        public static ActivityType Empty
        {
            get { return EmptyActivityType.Instance; }
        }

        public string Name { get; }

        public ActivityKind Kind { get; }

        virtual public bool OptOutOfDurationMetric { get; } = false;

        virtual public bool OptOutOfConcurrentMetric { get; } = false;

        virtual public bool OptInPropertiesAsDimensions { get; } = false;
    }

    /// <summary>
    /// Runtime Activity, so you can instantiate activities at runtime.  For internal use how Activities are consumed.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal class RuntimeActivityType : ActivityType
    {
        public RuntimeActivityType(string name, ActivityKind kind = ActivityKind.InternalCall)
            : base(name, kind)
        {
        }
    }

    /// <summary>
    /// Base class for all singleton activity types.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class SingletonActivityType<T> : ActivityType
        where T : ActivityType, new()
    {
        private static T instance = new T();

        protected SingletonActivityType(string name, ActivityKind type = ActivityKind.InternalCall)
            : base(name, type)
        {
            Contract.Check(instance == null, "Only use of the singleton instance is allowed");
        }

        public static T Instance
        {
            get { return instance; }
        }
    }

    /// <summary>
    /// Represents an empty activity type
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class EmptyActivityType : SingletonActivityType<EmptyActivityType>
    {
        public EmptyActivityType()
            : base("[EMPTY]", ActivityKind.InternalCall)
        {
        }
    }

    /// <summary>
    /// Specifies the activity kind.
    /// </summary>
    public enum ActivityKind
    {
        InternalCall = 0,
        ServiceApi = 1,
        ClientProxy = 2,
    }
}
