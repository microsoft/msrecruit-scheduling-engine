//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Security
{
    internal sealed class CertificateManagerFindByThumbprintActivity : SingletonActivityType<CertificateManagerFindByThumbprintActivity>
    {
        public CertificateManagerFindByThumbprintActivity()
            : base("SP.CertMgr.FindByThumbprint")
        {
        }
    }
}
