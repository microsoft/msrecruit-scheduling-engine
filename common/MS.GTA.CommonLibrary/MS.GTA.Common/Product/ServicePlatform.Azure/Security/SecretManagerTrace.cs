//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Tracing;

namespace MS.GTA.ServicePlatform.Azure.Security
{
    internal sealed class SecretManagerTrace : TraceSourceBase<SecretManagerTrace>
    {
        public override string Name
        {
            get { return "GTA.SP.SecretManager"; }
        }

        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
