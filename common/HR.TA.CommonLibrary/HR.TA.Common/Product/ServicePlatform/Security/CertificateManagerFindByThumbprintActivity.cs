//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using HR.TA.ServicePlatform.Context;

namespace HR.TA.ServicePlatform.Security
{
    internal sealed class CertificateManagerFindByThumbprintActivity : SingletonActivityType<CertificateManagerFindByThumbprintActivity>
    {
        public CertificateManagerFindByThumbprintActivity()
            : base("SP.CertMgr.FindByThumbprint")
        {
        }
    }
}
