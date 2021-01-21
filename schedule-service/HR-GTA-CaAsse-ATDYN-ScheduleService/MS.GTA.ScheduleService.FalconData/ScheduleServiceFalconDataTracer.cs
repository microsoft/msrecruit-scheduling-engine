//----------------------------------------------------------------------------
// <copyright file="ScheduleServiceFalconDataTracer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData
{
    using MS.GTA.ServicePlatform.Tracing;

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
