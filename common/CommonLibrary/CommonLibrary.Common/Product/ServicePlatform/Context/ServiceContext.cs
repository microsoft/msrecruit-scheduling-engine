//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Flighting;
using CommonLibrary.ServicePlatform.Security;
using CommonLibrary.ServicePlatform.Tracing;

// TODO - 0000: [anbencic] Implement identity context
namespace CommonLibrary.ServicePlatform.Context
{
    using System.Threading;

    /// <summary>
    /// An immutable object that holds the execution context for the currently
    /// running logical thread/activity. The object provides a number of "well-known" slots
    /// (for activity, tracing, etc.), plus an arbitrary number of user-defined slots.
    /// The guarantee is that everything that preserves the logical call context, including
    /// all of the Service Platform's utility classes, will correctly transfer the context
    /// between threads.
    /// </summary>
    /// <remarks>
    /// This class inherits MarshalByRefObject. This solves the problem of
    /// libraries that make cross-AppDomain calls to self-created AppDomains,
    /// such as System.Configuration, System.Data, and the WindowsAzure Runtime.
    /// (Such calls are problematic because they propagate the logical call context
    /// to the target AppDomain and they try to propagate it back to the caller; the
    /// latter usually fails because it can't find the right type information.)
    /// Note that such "foreign" AppDomains do not make use of the our call context,
    /// so their context's operations are not generally invoked from that AppDomain.
    /// 
    /// This class is a data contract which makes it possible for .NET
    /// to send it across the wire easily.
    /// </remarks>
    [ImmutableObject(true)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed partial class ServiceContext : MarshalByRefObject, IActivityContext, ICorrelationContext
    {
        private static AsyncLocal<ServiceContext> CurrentServiceContext = new AsyncLocal<ServiceContext>();

        private readonly ServiceContext previousContext;
        private readonly Activity activityContext;
        private readonly Flights flightsContext;
        private readonly IServiceContextPrincipal principal;
        private readonly Tracing tracingContext;
        private readonly IEnvironmentContext environmentContext;

        private ServiceContext(ServiceContext previous, Activity activityContext)
            : this(previous)
        {
            Contract.AssertValue(activityContext, nameof(activityContext));

            this.activityContext = activityContext;
        }

        private ServiceContext(ServiceContext previous, Flights flightsContext)
            : this(previous)
        {
            Contract.AssertValue(flightsContext, nameof(flightsContext));

            this.flightsContext = flightsContext;
        }

        private ServiceContext(ServiceContext previous, IServiceContextPrincipal principal)
            : this(previous)
        {
            Contract.AssertValue(principal, nameof(principal));

            this.principal = principal;
        }

        private ServiceContext(ServiceContext previous, Tracing tracingContext)
            : this(previous)
        {
            Contract.AssertValue(tracingContext, nameof(tracingContext));

            this.tracingContext = tracingContext;
        }

        private ServiceContext(ServiceContext previous, IEnvironmentContext environmentContext, IServiceContextPrincipal principal, Flights flightsContext, Activity activityContext, Tracing tracingContext)
            : this(previous)
        {
            Contract.AssertValueOrNull(environmentContext, nameof(environmentContext));
            Contract.AssertValueOrNull(principal, nameof(principal));
            Contract.AssertValueOrNull(flightsContext, nameof(flightsContext));
            Contract.AssertValueOrNull(activityContext, nameof(activityContext));
            Contract.AssertValueOrNull(tracingContext, nameof(tracingContext));

            this.environmentContext = environmentContext;
            this.principal = principal;
            this.flightsContext = flightsContext;
            this.activityContext = activityContext;
            this.tracingContext = tracingContext;
        }

        private ServiceContext(ServiceContext previous)
        {
            Contract.AssertValueOrNull(previous, nameof(previous));

            previousContext = previous;
            principal = previous?.principal;
            flightsContext = previous?.flightsContext;
            activityContext = previous?.activityContext;
            tracingContext = previous?.tracingContext;
            environmentContext = previous?.environmentContext;
        }

        private static ServiceContext CurrentContext
        {
            get { return CurrentServiceContext.Value; }
        }

        /// <inheritdoc />
        string IActivityContext.SessionId
        {
            get { return activityContext?.SessionId.ToString("D"); }
        }

        /// <inheritdoc />
        string IActivityContext.RootActivityId
        {
            get { return activityContext?.RootActivityId.ToString("D"); }
        }

        /// <inheritdoc />
        string IActivityContext.ActivityVector
        {
            get { return activityContext?.ActivityVector; }
        }

        string ICorrelationContext.SessionId
        {
            get { return activityContext?.SessionId.ToString(); }
        }

        string ICorrelationContext.RootActivityId
        {
            get { return activityContext?.RootActivityId.ToString(); }
        }

        string ICorrelationContext.ActivityVector
        {
            get { return activityContext?.ActivityVector; }
        }

        string ICorrelationContext.Application
        {
            get { return environmentContext?.Application; }
        }

        string ICorrelationContext.Service
        {
            get { return environmentContext?.Service; }
        }

        string ICorrelationContext.CodePackageVersion
        {
            get { return environmentContext?.CodePackageVersion; }
        }

        /// <summary>
        /// Captures the current context, so that it may be restored later
        /// </summary>
        public static ServiceContext Capture()
        {
            return CurrentContext;
        }

        /// <summary>
        /// Gets the serializable <see cref="RootExecutionContext"/> capture reflecting the 
        /// current <see cref="ServiceContext"/> state.
        /// </summary>
        public static RootExecutionContext CaptureRoot()
        {
            var activityContext = Activity.Current;
            var flightsContext = Flights.Current;
            var tracingContext = Tracing.Current;
            var environmentContext = Environment.Current;

            return new RootExecutionContext
            {
                SessionId = activityContext != null ? activityContext.SessionId : Guid.NewGuid(),
                RootActivityId = activityContext != null ? activityContext.RootActivityId : Guid.NewGuid(),
                ActivityVector = activityContext?.ActivityVector,
                EnabledFlightNames = Flight.ConvertFlightsToFlightNames(flightsContext?.EnabledFlights),
                Principal = Principal.Current,
                SuppressedTraceVebosity = tracingContext != null ? (TraceVerbosity?)tracingContext.SuppressedTraceVerbosity : null,
                ForcedTraceVerbosity = tracingContext != null ? (TraceVerbosity?)tracingContext.ForcedTraceVerbosity : null,
                EnvironmentContext = environmentContext
            };
        }

        /// <summary>
        /// Restores a captured context.
        /// </summary>
        internal static IDisposable Restore(ServiceContext context)
        {
            return Push(context);
        }

        /// <summary>
        /// Restores the captured context as long as the current context is null.
        /// </summary>
        internal static IDisposable RestoreIfNoContextPresent(ServiceContext context)
        {
            if (CurrentContext != null || context == null)
            {
                return VoidDisposable.Instance;
            }

            return Push(context);
        }

        /// <summary>
        ///  Nullifies the current context until disposed.
        /// </summary>
        internal static IDisposable Suppress()
        {
            return Push((ServiceContext)null);
        }

        /// <summary>
        /// Push a new activity context leaving all else intact.
        /// </summary>
        internal static IDisposable Push(Activity activityContext)
        {
            var currentContext = CurrentContext;
            var newContext = new ServiceContext(currentContext, activityContext);
            return Push(newContext);
        }

        /// <summary>
        /// Push a new flights context leaving all else intact.
        /// </summary>
        internal static IDisposable Push(Flights flights)
        {
            var currentContext = CurrentContext;
            var newContext = new ServiceContext(currentContext, flights);
            return Push(newContext);
        }

        /// <summary>
        /// Push a new principal onto the context leaving all else intact.
        /// </summary>
        internal static IDisposable Push<T>(T principal) where T : class, IServiceContextPrincipal
        {
            var currentContext = CurrentContext;
            var newContext = new ServiceContext(currentContext, principal);
            return Push(newContext);
        }

        /// <summary>
        /// Push a new tracing context leaving all else intact.
        /// </summary>
        internal static IDisposable Push(Tracing tracingContext)
        {
            var currentContext = CurrentContext;
            var newContext = new ServiceContext(currentContext, tracingContext);
            return Push(newContext);
        }

        internal static IDisposable Push(
            IEnvironmentContext environmentContext,
            IServiceContextPrincipal principal,
            Flights flights,
            Activity activityContext,
            Tracing tracingContext)
        {
            var currentContext = CurrentContext;
            var newContext = new ServiceContext(currentContext, environmentContext, principal, flights, activityContext, tracingContext);
            return Push(newContext);
        }

        public static void Clear()
        {
            CurrentServiceContext.Value = null;
        }

        private static IDisposable Push(ServiceContext context)
        {
            var currentContext = CurrentContext;
            if (object.ReferenceEquals(context, currentContext))
                return VoidDisposable.Instance;

            CurrentServiceContext.Value = context;

            return new ActionDisposable(() =>
            {
                CurrentServiceContext.Value = currentContext;
            });
        }
    }

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public sealed class RootExecutionContext
    {
        public static RootExecutionContext Empty { get; } = new RootExecutionContext();

        [DataMember(Name = "sessionId", IsRequired = false, EmitDefaultValue = false)]
        public Guid SessionId { get; set; }

        [DataMember(Name = "rootActivityId", IsRequired = false, EmitDefaultValue = false)]
        public Guid RootActivityId { get; set; }

        [DataMember(Name = "activityVector", IsRequired = false, EmitDefaultValue = false)]
        public string ActivityVector { get; set; }

        [DataMember(Name = "enabledFlightNames", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> EnabledFlightNames { get; set; }

        [DataMember(Name = "principal", IsRequired = false, EmitDefaultValue = false)]
        public IServiceContextPrincipal Principal { get; set; }

        [DataMember(Name = "suppressedTraceVerbosity", IsRequired = false, EmitDefaultValue = false)]
        public TraceVerbosity? SuppressedTraceVebosity { get; set; }

        [DataMember(Name = "forcedTraceVerbosity", IsRequired = false, EmitDefaultValue = false)]
        public TraceVerbosity? ForcedTraceVerbosity { get; set; }

        [IgnoreDataMember]
        public IEnvironmentContext EnvironmentContext { get; set; }
    }
}
