//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.ServicePlatform.Tracing;

namespace TA.CommonLibrary.ServicePlatform.Azure.Security
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
