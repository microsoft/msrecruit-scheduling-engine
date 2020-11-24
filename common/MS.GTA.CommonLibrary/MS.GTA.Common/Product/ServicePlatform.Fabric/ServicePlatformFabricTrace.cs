//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ServicePlatform.Fabric
{
    internal sealed class ServicePlatformFabricTrace : TraceSourceBase<ServicePlatformFabricTrace>
    {
        public override string Name
        {
            get { return "GTA.SP.Fabric"; }
        }

        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
