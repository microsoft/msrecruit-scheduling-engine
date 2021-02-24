//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Azure.Security.Activities
{
    internal sealed class SecretManagerTryGetSecretActivity : SingletonActivityType<SecretManagerTryGetSecretActivity>
    {
        public SecretManagerTryGetSecretActivity()
            : base("SP.SecretManager.TryGetSecret")
        {
        }
    }
}
