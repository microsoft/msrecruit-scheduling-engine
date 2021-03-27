//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using HR.TA.ServicePlatform.Tracing;

namespace HR.TA.ScheduleService.Data
{
    /// <summary>
    /// The scheduling service tracer.
    /// </summary>
    public sealed class ScheduleServiceDataTracer : TraceSourceBase<ScheduleServiceDataTracer>
    {
        /// <summary>Gets the tracer name.</summary>
        public override string Name => "GTA.ScheduleService.Data";

        /// <summary>Gets the trace verbosity.</summary>
        public override TraceVerbosity Verbosity => TraceVerbosity.Info;
    }
}
