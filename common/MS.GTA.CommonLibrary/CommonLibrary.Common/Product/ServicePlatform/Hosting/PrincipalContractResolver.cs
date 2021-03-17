//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Security;
using Newtonsoft.Json.Serialization;

namespace CommonLibrary.ServicePlatform.Hosting.Security
{
    internal sealed class PrincipalContractResolver : DefaultContractResolver
    {
        private readonly Type principalType;

        public PrincipalContractResolver(Type principalType)
        {
            Contract.AssertValue(principalType, nameof(principalType));

            this.principalType = principalType;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType == typeof(IServiceContextPrincipal))
                objectType = principalType;

            return base.CreateContract(objectType);
        }
    }
}
