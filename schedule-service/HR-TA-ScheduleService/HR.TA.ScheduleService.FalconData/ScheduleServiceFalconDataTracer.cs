//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.FalconData
{
    using HR.TA.ServicePlatform.Tracing;

    /// <summary>
    /// The Attract data tracer.
    /// </summary>
    public sealed class ScheduleServiceFalconDataTracer : TraceSourceBase<ScheduleServiceFalconDataTracer>
    {
        /// <summary>Gets the tracer name.</summary>
        public override string Name => "GTA.ScheduleServiceFalconData";

        /// <summary>Gets the trace verbosity.</summary>
        public override TraceVerbosity Verbosity => TraceVerbosity.Info;
    }
}
