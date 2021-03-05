//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using ServicePlatform.Context;

namespace ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerReadSecretsActivity : SingletonActivityType<SecretManagerReadSecretsActivity>
    {
        public SecretManagerReadSecretsActivity()
            : base("SP.SecretManager.ReadSecrets")
        {
        }
    }
}
