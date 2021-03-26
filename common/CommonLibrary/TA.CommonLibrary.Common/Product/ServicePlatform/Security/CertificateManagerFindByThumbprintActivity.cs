//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using TA.CommonLibrary.ServicePlatform.Context;

namespace TA.CommonLibrary.ServicePlatform.Security
{
    internal sealed class CertificateManagerFindByThumbprintActivity : SingletonActivityType<CertificateManagerFindByThumbprintActivity>
    {
        public CertificateManagerFindByThumbprintActivity()
            : base("SP.CertMgr.FindByThumbprint")
        {
        }
    }
}
