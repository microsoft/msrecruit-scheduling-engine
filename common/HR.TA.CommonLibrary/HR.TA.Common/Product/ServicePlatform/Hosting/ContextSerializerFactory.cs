//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Hosting.Security;
using Newtonsoft.Json;

namespace HR.TA.ServicePlatform.Hosting
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
