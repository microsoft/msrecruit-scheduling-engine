//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.ServicePlatform.AspNetCore.Security
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
