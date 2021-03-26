//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TA.CommonLibrary.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// A contract for <see cref="RetriableRouter"/> requests.
    /// </summary>
    public interface IRetriableRouterRequest : IDisposable
    {
        /// <summary>
        /// Implementing class should return true if the response indicates a routing failure and the request
        /// should be retried.
        /// </summary>
        bool ShouldRetry(HttpRequestMessage request, HttpResponseMessage lastResponse);

        /// <summary>
        /// Implementing class should return the next endpoint or null if no (more) endpoints are available. An 
        /// instance of <see cref="NoRouterEndpointsAvailableException"/> if null is returned.
        /// </summary>
        Task<Uri> GetNextEndpointAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
