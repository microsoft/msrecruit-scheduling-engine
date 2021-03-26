//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.ServicePlatform.Tracing;

namespace HR.TA.ServicePlatform.Communication.Http.Internal
{
    /// <summary>
    /// Internal trace source for the communication stack.
    /// </summary>
    internal sealed class CommunicationTraceSource : TraceSourceBase<CommunicationTraceSource>
    {
        /// <inheritdoc />
        public override string Name
        {
            get { return "GTA.SP.Communication"; }
        }

        /// <inheritdoc />
        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
