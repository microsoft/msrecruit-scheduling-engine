//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.BusinessLibrary
{
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>
    /// The user directory service tracer.
    /// </summary>
    public sealed class ScheduleServiceBusinessLibraryTracer : TraceSourceBase<ScheduleServiceBusinessLibraryTracer>
    {
        /// <summary>Gets the tracer name.</summary>
        public override string Name => "GTA.ScheduleServiceBusinessLayer";

        /// <summary>Gets the trace verbosity.</summary>
        public override TraceVerbosity Verbosity => TraceVerbosity.Info;
    }
}
