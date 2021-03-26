//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.Extensions.Logging;

namespace TA.CommonLibrary.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// An abstraction over components responsible for routing requests among endpoints. The specific
    /// router implementations can be stateless as the retry policy is handled by an instance of 
    /// <see cref="IHttpRouterRequest"/> which gets created for every request.
    /// </summary>
    public interface IHttpRouter : IIdentifiable
    {
        /// <summary>
        /// Implementing class provides an implementation of <see cref="IHttpRouterRequest"/> 
        /// which holds the state for a single request execution and controls the retry loop.
        /// </summary>
        IHttpRouterRequest CreateRouterRequest();
        
        /// <summary>
        /// Implementing class provides an implementation of <see cref="IHttpRouterRequest"/> 
        /// which holds the state for a single request execution and controls the retry loop.
        /// </summary>
        IHttpRouterRequest CreateRouterRequest(ILoggerFactory loggerFactory);
    }
}
