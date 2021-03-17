//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Security
{
    internal sealed class CertificateManagerFindByThumbprintActivity : SingletonActivityType<CertificateManagerFindByThumbprintActivity>
    {
        public CertificateManagerFindByThumbprintActivity()
            : base("SP.CertMgr.FindByThumbprint")
        {
        }
    }
}
