//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.CommonDataService.Common.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace HR.TA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Convenience extension methods over <see cref="IServiceCollection"/>.
    /// </summary>
    public static class HttpCommunicationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="HttpCommunicationClientFactory"/> as a singleton to the provided <paramref name="serviceCollection"/>.
        /// </summary>
        public static IServiceCollection AddHttpCommunicationClientFactory(this IServiceCollection serviceCollection)
        {
            Contract.CheckValue(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddSingleton<IHttpCommunicationClientFactory>(new HttpCommunicationClientFactory());
        }
    }
}
