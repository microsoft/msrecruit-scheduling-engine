//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerReadSecretActivity : SingletonActivityType<SecretManagerReadSecretActivity>
    {
        public SecretManagerReadSecretActivity()
            : base("SP.SecretManager.ReadSecret")
        {
        }
    }
}
