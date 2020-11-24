//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerReadSecretsActivity : SingletonActivityType<SecretManagerReadSecretsActivity>
    {
        public SecretManagerReadSecretsActivity()
            : base("SP.SecretManager.ReadSecrets")
        {
        }
    }
}
