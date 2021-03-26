//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Security
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
