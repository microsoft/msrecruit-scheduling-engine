//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using ServicePlatform.Context;

namespace ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerDeleteSecretActivity : SingletonActivityType<SecretManagerDeleteSecretActivity>
    {
        public SecretManagerDeleteSecretActivity()
            : base("SP.SecretManager.DeleteSecret")
        {
        }
    }
}
