//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace MS.GTA.CommonDataService.Common
{
    /// <summary>
    /// Interface for tracer objects that can be consumed by the InfoNav Interpretation
    /// Service. Classes that implement this interface have the ability to fire traces
    /// at multiple levels of severity.
    /// </summary>
    /// <remarks>
    /// This interface is adapted from <see cref="ITraceSource"/> in the Cloud Platform assembly
    /// but can also be implemented using standard .NET tracing for standalone operations
    /// </remarks>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    [SuppressMessage("Microsoft.InfoNav.StyleCop.CSharp.InfoNavAnalyzer", "IN0001:InfoNavPublicObjectsMustBeInInternalNamespace", Justification = "Not worth introducing namespace reference everywhere for this common class.")]
    internal interface ITracer
    {
        /// <summary>
        /// Trace a fatal message that consists of a single string.
        /// </summary>
        /// <param name="message">The message of the trace line</param>
        void TraceFatal(string message);

        /// <summary>
        /// Trace a fatal message that consists of a format string with a single parameter.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        void TraceFatal(string format, object arg0);

        /// <summary>
        /// Trace a fatal message that consists of a format string with two parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The first argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        void TraceFatal(string format, object arg0, object arg1);

        /// <summary>
        /// Trace a fatal message that consists of a format string with three parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        void TraceFatal(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Trace a fatal message that consists of a format string with four parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        /// <param name="arg3">The forth argument for the message</param>
        void TraceFatal(string format, object arg0, object arg1, object arg2, object arg3);

        /// <summary>
        /// Trace an error message that consists of a single string.
        /// </summary>
        /// <param name="message">The message of the trace line</param>
        void TraceError(string message);

        /// <summary>
        /// Trace a error message that consists of a format string with a single parameter.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        void TraceError(string format, object arg0);

        /// <summary>
        /// Trace a error message that consists of a format string with two parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The first argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        void TraceError(string format, object arg0, object arg1);

        /// <summary>
        /// Trace a error message that consists of a format string with three parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        void TraceError(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Trace a error message that consists of a format string with four parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        /// <param name="arg3">The forth argument for the message</param>
        void TraceError(string format, object arg0, object arg1, object arg2, object arg3);

        /// <summary>
        /// Trace a warning message that consists of a single string.
        /// </summary>
        /// <param name="message">The message of the trace line</param>
        void TraceWarning(string message);

        /// <summary>
        /// Trace a warning message that consists of a format string with a single parameter.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        void TraceWarning(string format, object arg0);

        /// <summary>
        /// Trace a warning message that consists of a format string with two parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The first argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        void TraceWarning(string format, object arg0, object arg1);

        /// <summary>
        /// Trace a warning message that consists of a format string with three parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        void TraceWarning(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Trace a warning message that consists of a format string with four parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        /// <param name="arg3">The forth argument for the message</param>
        void TraceWarning(string format, object arg0, object arg1, object arg2, object arg3);

        /// <summary>
        /// Trace an informational message that consists of a single string.
        /// </summary>
        /// <param name="message">The message of the trace line</param>
        void TraceInformation(string message);

        /// <summary>
        /// Trace a information message that consists of a format string with a single parameter.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        void TraceInformation(string format, object arg0);

        /// <summary>
        /// Trace a information message that consists of a format string with two parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The first argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        void TraceInformation(string format, object arg0, object arg1);

        /// <summary>
        /// Trace a information message that consists of a format string with three parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        void TraceInformation(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Trace a information message that consists of a format string with four parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        /// <param name="arg3">The forth argument for the message</param>
        void TraceInformation(string format, object arg0, object arg1, object arg2, object arg3);

        /// <summary>
        /// Trace a verbose message that consists of a single string.
        /// </summary>
        /// <param name="message">The message of the trace line</param>
        void TraceVerbose(string message);

        /// <summary>
        /// Trace a verbose message that consists of a format string with a single parameter.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        void TraceVerbose(string format, object arg0);

        /// <summary>
        /// Trace a verbose message that consists of a format string with two parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The first argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        void TraceVerbose(string format, object arg0, object arg1);

        /// <summary>
        /// Trace a verbose message that consists of a format string with three parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        void TraceVerbose(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Trace a verbose message that consists of a format string with four parameters.
        /// </summary>
        /// <param name="format">The format of the message</param>
        /// <param name="arg0">The argument for the message</param>
        /// <param name="arg1">The second argument for the message</param>
        /// <param name="arg2">The third argument for the message</param>
        /// <param name="arg3">The forth argument for the message</param>
        void TraceVerbose(string format, object arg0, object arg1, object arg2, object arg3);
    }
}
