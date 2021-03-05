//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ServicePlatform.Tracing
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
