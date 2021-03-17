//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerDeleteSecretActivity : SingletonActivityType<SecretManagerDeleteSecretActivity>
    {
        public SecretManagerDeleteSecretActivity()
            : base("SP.SecretManager.DeleteSecret")
        {
        }
    }
}
