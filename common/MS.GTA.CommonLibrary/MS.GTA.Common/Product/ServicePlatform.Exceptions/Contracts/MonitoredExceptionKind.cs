//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ServicePlatform.Exceptions
{
    /// <summary>
    /// Exception Kind.
    /// </summary>
    public enum MonitoredExceptionKind
    {
        /// <summary>
        /// Ignorable error? User-related?
        /// </summary>
        Benign = 1,

        /// <summary>
        /// Service-Level Error
        /// </summary>
        Service = 2,

        /// <summary>
        /// Remote Service Failure
        /// </summary>
        Remote = 3,

        /// <summary>
        /// Halt and Catch Fire.  Terminate Service Process and Recycle.
        /// </summary>
        Fatal = 4,

        /// <summary>
        /// It's not obvious that Service == Error in the telemetry.  This explicitly maps, so callers can understand
        /// </summary>
        Error = 5,
    }
}
