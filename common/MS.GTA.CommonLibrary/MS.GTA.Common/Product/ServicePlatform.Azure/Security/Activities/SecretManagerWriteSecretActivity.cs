//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerWriteSecretActivity : SingletonActivityType<SecretManagerWriteSecretActivity>
    {
        public SecretManagerWriteSecretActivity()
            : base("SP.SecretManager.WriteSecret")
        {
        }
    }
}
