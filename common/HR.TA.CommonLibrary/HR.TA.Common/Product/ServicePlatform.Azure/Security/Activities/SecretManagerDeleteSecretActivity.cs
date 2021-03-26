//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.ServicePlatform.Context;

namespace HR.TA.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerDeleteSecretActivity : SingletonActivityType<SecretManagerDeleteSecretActivity>
    {
        public SecretManagerDeleteSecretActivity()
            : base("SP.SecretManager.DeleteSecret")
        {
        }
    }
}
