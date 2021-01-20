﻿//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ServicePlatform.Communication.Http.Internal
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
