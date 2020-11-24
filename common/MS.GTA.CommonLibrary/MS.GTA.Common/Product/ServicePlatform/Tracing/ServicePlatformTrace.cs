//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.Tracing
{
    internal sealed class ServicePlatformTrace : TraceSourceBase<ServicePlatformTrace>
    {
        public override string Name
        {
            get { return "GTA.SP"; }
        }

        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
