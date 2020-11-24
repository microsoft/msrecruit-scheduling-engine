//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerReadSecretActivity : SingletonActivityType<SecretManagerReadSecretActivity>
    {
        public SecretManagerReadSecretActivity()
            : base("SP.SecretManager.ReadSecret")
        {
        }
    }
}
