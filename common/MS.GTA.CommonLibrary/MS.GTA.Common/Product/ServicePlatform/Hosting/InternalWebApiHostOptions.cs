//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Security;

namespace MS.GTA.ServicePlatform.Hosting
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
