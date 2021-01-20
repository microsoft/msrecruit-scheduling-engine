//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerDeleteSecretActivity : SingletonActivityType<SecretManagerDeleteSecretActivity>
    {
        public SecretManagerDeleteSecretActivity()
            : base("SP.SecretManager.DeleteSecret")
        {
        }
    }
}
