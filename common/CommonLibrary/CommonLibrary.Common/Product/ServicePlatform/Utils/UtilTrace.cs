//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.ServicePlatform.Tracing;

namespace CommonLibrary.ServicePlatform.Utils
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
