//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace HR.TA.ServicePlatform.AspNetCore.Security
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
