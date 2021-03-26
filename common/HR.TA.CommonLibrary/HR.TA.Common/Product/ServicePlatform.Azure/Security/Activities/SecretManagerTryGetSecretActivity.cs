//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.ServicePlatform.Context;

namespace HR.TA.ServicePlatform.Azure.Security.Activities
{
    internal sealed class SecretManagerTryGetSecretActivity : SingletonActivityType<SecretManagerTryGetSecretActivity>
    {
        public SecretManagerTryGetSecretActivity()
            : base("SP.SecretManager.TryGetSecret")
        {
        }
    }
}
