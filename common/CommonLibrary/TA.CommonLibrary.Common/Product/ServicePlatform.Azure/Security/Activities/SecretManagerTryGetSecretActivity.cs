//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.ServicePlatform.Context;

namespace TA.CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal sealed class SecretManagerTryGetSecretActivity : SingletonActivityType<SecretManagerTryGetSecretActivity>
    {
        public SecretManagerTryGetSecretActivity()
            : base("SP.SecretManager.TryGetSecret")
        {
        }
    }
}
