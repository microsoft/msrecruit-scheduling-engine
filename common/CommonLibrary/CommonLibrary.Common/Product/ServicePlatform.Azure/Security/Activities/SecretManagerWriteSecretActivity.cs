//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerWriteSecretActivity : SingletonActivityType<SecretManagerWriteSecretActivity>
    {
        public SecretManagerWriteSecretActivity()
            : base("SP.SecretManager.WriteSecret")
        {
        }
    }
}
