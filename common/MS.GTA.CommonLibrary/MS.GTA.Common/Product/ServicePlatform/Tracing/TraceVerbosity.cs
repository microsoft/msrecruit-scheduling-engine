//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace MS.GTA.ServicePlatform.Tracing
{
    /// <summary>
    /// The different trace verbosities
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Intended")]
    public enum TraceVerbosity
    {
        /// <summary>
        /// Fatal verbosity
        /// </summary>
        Fatal = 1,

        /// <summary>
        /// Error verbosity
        /// </summary>
        Error = 2,

        /// <summary>
        /// Warning verbosity
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Info verbosity
        /// </summary>
        Info = 4,

        /// <summary>
        /// Verbose verbosity
        /// </summary>
        Verbose = 5,

        // For TracingContext - not an actual verbosities
        SuppressAll = 0,
        SuppressNothing = 6,
        ForceAll = 6,
        ForceNothing = 0,
    }
}
