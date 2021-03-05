//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonDataService.Common.Internal;
using ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePlatform.AspNetCore.Security
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
