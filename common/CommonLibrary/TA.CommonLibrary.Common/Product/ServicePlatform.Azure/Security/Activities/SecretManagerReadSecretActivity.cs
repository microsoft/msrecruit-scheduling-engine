//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.ServicePlatform.Context;

namespace TA.CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerReadSecretActivity : SingletonActivityType<SecretManagerReadSecretActivity>
    {
        public SecretManagerReadSecretActivity()
            : base("SP.SecretManager.ReadSecret")
        {
        }
    }
}
