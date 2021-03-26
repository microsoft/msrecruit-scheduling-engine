//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using HR.TA.ServicePlatform.Tracing;

namespace HR.TA.ServicePlatform.Context
{
    public sealed partial class ServiceContext
    {
        // Static context APIs
        public sealed partial class Tracing
        {
            // Keep internal for platform and tests, there shouldn't be a need to use this one otherwise
            internal static Tracing Current
            {
                get
                {
                    var currentContext = CurrentContext;
                    if (currentContext == null)
                        return null;

                    return currentContext.tracingContext;
                }
            }

            public static IDisposable Suppress(TraceVerbosity newVerbosity)
            {
                var currentTracingContext = Current;
                var tracingContext = new Tracing(
                    newVerbosity,
                    currentTracingContext != null ? currentTracingContext.ForcedTraceVerbosity : TraceVerbosity.ForceNothing);
                return ServiceContext.Push(tracingContext);
            }

            public static IDisposable Force(TraceVerbosity newVerbosity)
            {
                var currentTracingContext = Current;
                var tracingContext = new Tracing(
                    currentTracingContext != null ? currentTracingContext.SuppressedTraceVerbosity : TraceVerbosity.SuppressNothing,
                    newVerbosity);
                return ServiceContext.Push(tracingContext);
            }

            internal static bool ShouldTrace(TraceVerbosity level)
            {
                var currentTracingContext = CurrentContext?.tracingContext;
                if (currentTracingContext == null)
                    return true;

                return level <= currentTracingContext.SuppressedTraceVerbosity || level >= currentTracingContext.ForcedTraceVerbosity;
            }
        }

        // Instance implementation
        [ImmutableObject(true)]
        public sealed partial class Tracing
        {
            // Keep these constructors internal. Consumers need to go through the static APIs provided on ServiceContext
            internal Tracing(
                TraceVerbosity suppressedTraceVerbosity,
                TraceVerbosity forcedTraceVerbosity)
            {
                SuppressedTraceVerbosity = suppressedTraceVerbosity;
                ForcedTraceVerbosity = forcedTraceVerbosity;
            }

            /// <summary>
            /// Suppression verbosity -- everything which is more verbose than this
            /// will not be traced.
            /// </summary>
            internal TraceVerbosity SuppressedTraceVerbosity { get; }

            /// <summary>
            /// Forced verbosity -- everything which is higher than this
            /// will be traced.
            /// </summary>
            internal TraceVerbosity ForcedTraceVerbosity { get; }
        }
    }
}
