//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using ServicePlatform.Context;

namespace ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerWriteSecretActivity : SingletonActivityType<SecretManagerWriteSecretActivity>
    {
        public SecretManagerWriteSecretActivity()
            : base("SP.SecretManager.WriteSecret")
        {
        }
    }
}
