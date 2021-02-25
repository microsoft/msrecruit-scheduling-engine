//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Base.Security.V2
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>The HCM principal retriever extension.</summary>
    public static class HCMPrincipalRetrieverExtension
    {
        /// <summary>The add principal retriever.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddPrincipalRetriever(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IHCMPrincipalRetriever, HCMPrincipalRetriever>();
        }
    }
}