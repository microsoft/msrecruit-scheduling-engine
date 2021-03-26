//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.ServicePlatform.Context;

namespace TA.CommonLibrary.ServicePlatform.Azure.Security.Activities
{
    internal class SecretManagerWriteSecretActivity : SingletonActivityType<SecretManagerWriteSecretActivity>
    {
        public SecretManagerWriteSecretActivity()
            : base("SP.SecretManager.WriteSecret")
        {
        }
    }
}
