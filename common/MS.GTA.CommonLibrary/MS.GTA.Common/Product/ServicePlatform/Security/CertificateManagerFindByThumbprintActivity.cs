//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

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
