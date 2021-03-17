//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal sealed class SecretManagerTryGetSecretActivity : SingletonActivityType<SecretManagerTryGetSecretActivity>
    {
        public SecretManagerTryGetSecretActivity()
            : base("SP.SecretManager.TryGetSecret")
        {
        }
    }
}
