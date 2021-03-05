//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using CommonDataService.Common.Internal;
using ServicePlatform.Hosting.Security;
using Newtonsoft.Json;

namespace ServicePlatform.Hosting
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
