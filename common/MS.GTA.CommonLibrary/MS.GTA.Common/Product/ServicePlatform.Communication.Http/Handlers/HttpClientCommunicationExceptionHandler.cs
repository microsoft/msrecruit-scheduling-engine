//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.Communication.Http.Handlers
{

    /// <summary>
    /// Wraps HttpRequestExceptions thrown from the underlying transport handler in a monitored exception 
    /// derived from <see cref="HttpCommunicationException"/> for monitoring and easier consumption. 
    /// 
    /// The exception handler should be put as the last handler in the request pipeline so that other
    /// handlers can work with a consistent stack of monitored exceptions.
    /// 
    /// See <see cref="HttpCommunicationException"/> for more details on the behavior.
    /// </summary>
    public class HttpClientCommunicationExceptionHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public HttpClientCommunicationExceptionHandler(ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory != null)
            {
                this.logger = loggerFactory.CreateLogger<HttpClientCommunicationExceptionHandler>();                
            }
        }
        
        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                throw HttpCommunicationException.Create(ex, this.logger);
            }
        }
    }
}
