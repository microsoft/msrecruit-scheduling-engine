//----------------------------------------------------------------------------
// <copyright file="ScheduleServiceTracer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService
{
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>
    /// The user directory service tracer.
    /// </summary>
    public sealed class ScheduleServiceTracer : TraceSourceBase<ScheduleServiceTracer>
    {
        /// <summary>Gets the tracer name.</summary>
        public override string Name => "GTA.ScheduleService";

        /// <summary>Gets the trace verbosity.</summary>
        public override TraceVerbosity Verbosity => TraceVerbosity.Info;
    }
}
