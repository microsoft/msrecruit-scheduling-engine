//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Hosting.Security;
using Newtonsoft.Json;

namespace CommonLibrary.ServicePlatform.Hosting
{
    internal static class ContextSerializerFactory
    {
        internal static JsonSerializer CreateForInternalHost(InternalWebApiHostOptions options)
        {
            Contract.AssertValue(options, nameof(options));
            Contract.AssertValue(options.ServiceContextPrincipalType, nameof(options.ServiceContextPrincipalType));

            var jsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                ContractResolver = new PrincipalContractResolver(options.ServiceContextPrincipalType),
            });

            return jsonSerializer;
        }
    }
}
