//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MS.GTA.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// Represents an abstraction over a single request made on behalf of the owning <see cref="IHttpRouter"/>.
    /// </summary>
    public interface IHttpRouterRequest : IDisposable
    {
        /// <summary>
        /// Controls the request execution loop. The router should do one of the following:
        /// 
        /// 1) Return an absolute base URL:
        ///        - Means that next request (attempt) should be made by the message handler
        /// 2) Return null:
        ///        - Means that the request should not be retried again, or that no endpoints are available (if this is the first attempt)
        ///        - The message handler will return the last result, re-throw the last exception or throw <see cref="NoRouterEndpointsAvailableException"/> if this is the first attempt.
        ///        - Breaks the retry loop with one of the above
        /// 3) Throw a custom exception:
        ///        - A router exception or no more endpoints available
        ///        - Breaks the retry loop with the thrown exception
        /// </summary>
        /// <param name="request">
        /// The request message object being sent. There is no guarantee that the request object instance 
        /// will remain the same across retries. Handler implementations can choose to recreate the request 
        /// in certain situations. In such a case all request data (including properties) will be copied
        /// to the new request instance. Routers are therefore free to use request properties to carry
        /// custom data between retries.
        /// </param>
        /// <param name="lastResponse">
        /// The endpoint from the most recent failed request attempt. 
        /// Will be null on first attempt or when an exception was thrown 
        /// instead of receiving a non-success response from the server.
        /// </param>
        /// <param name="lastException">
        /// The exception that was thrown for the most recent attempt. 
        /// Will be null on first attempt or when a response was received from the server.
        /// </param>
        /// <param name="cancellationToken">
        /// The execution cancellation token.
        /// </param>
        Task<Uri> GetNextEndpointAsync(
            HttpRequestMessage request, 
            HttpResponseMessage lastResponse, 
            HttpCommunicationException lastException,
            CancellationToken cancellationToken);
    }
}
