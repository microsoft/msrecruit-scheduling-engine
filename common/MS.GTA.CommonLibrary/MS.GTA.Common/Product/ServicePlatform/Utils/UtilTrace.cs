//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ServicePlatform.Utils
{
    internal sealed class UtilTrace : TraceSourceBase<UtilTrace>
    {
        public override string Name
        {
            get { return "SP.Util"; }
        }

        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
