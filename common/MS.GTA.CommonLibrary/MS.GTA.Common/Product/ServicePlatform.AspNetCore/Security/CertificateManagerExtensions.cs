//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace MS.GTA.ServicePlatform.AspNetCore.Security
{
    public static class CertificateManagerExtensions
    {
        public static void AddCertificateManager(this IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddSingleton<ICertificateManager, CertificateManager>();
        }
    }
}
