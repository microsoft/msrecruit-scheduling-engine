//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerReadSecretsActivity : SingletonActivityType<SecretManagerReadSecretsActivity>
    {
        public SecretManagerReadSecretsActivity()
            : base("SP.SecretManager.ReadSecrets")
        {
        }
    }
}
