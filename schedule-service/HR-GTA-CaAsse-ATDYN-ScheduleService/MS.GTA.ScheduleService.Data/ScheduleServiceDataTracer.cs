﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ScheduleServiceDataTracer.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ScheduleService.Data
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
