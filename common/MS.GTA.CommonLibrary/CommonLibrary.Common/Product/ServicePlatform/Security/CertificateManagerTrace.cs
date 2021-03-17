//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.ServicePlatform.Tracing;

namespace CommonLibrary.ServicePlatform.Security
{
    internal sealed class CertificateManagerTrace : TraceSourceBase<CertificateManagerTrace>
    {
        public override string Name
        {
            get { return "GTA.SP.CertMngr"; }
        }

        public override TraceVerbosity Verbosity
        {
            get { return TraceVerbosity.Info; }
        }
    }
}
