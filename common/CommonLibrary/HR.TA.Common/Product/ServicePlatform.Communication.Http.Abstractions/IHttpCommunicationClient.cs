//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HR.TA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// An abstraction over components responsible for routing requests among endpoints. The specific
    /// router implementations can be stateless as the retry policy is handled by an instance of 
    /// <see cref="IHttpRouterRequest"/> which gets created for every request.
    /// </summary>
    public interface IHttpCommunicationClient : IDisposable
    {
        /// <summary>
        /// Sends the provided <paramref name="request"/> message with the given <paramref name="cancellationToken"/>
        /// </summary>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
