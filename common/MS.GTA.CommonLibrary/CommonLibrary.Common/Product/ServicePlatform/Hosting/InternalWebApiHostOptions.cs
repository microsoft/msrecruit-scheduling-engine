//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Security;

namespace CommonLibrary.ServicePlatform.Hosting
{
    public sealed class InternalWebApiHostOptions
    {
        private Type serviceContextPrincipalType;

        public Type ServiceContextPrincipalType
        {
            get { return serviceContextPrincipalType; }
            set
            {
                Contract.CheckValue(value, nameof(value));

                // TODO - 0000: Custom exception below
                var interfaceType = typeof(IServiceContextPrincipal);
                Contract.Check(value != interfaceType, "The provided type must not be directly IServiceContextPrincipal");
                Contract.Check(interfaceType.IsAssignableFrom(value), "The provided type must inherit from IServiceContextPrincipal");

                serviceContextPrincipalType = value;
            }
        }
    }
}
