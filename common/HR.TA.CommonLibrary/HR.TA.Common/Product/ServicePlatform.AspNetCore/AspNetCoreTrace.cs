//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ServicePlatform.Tracing
{
    internal sealed class AspNetCoreTrace : TraceSourceBase<AspNetCoreTrace>
    {
        public override string Name
        {
            get { return "GTA.SP.AspNetCore"; }
        }

        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
