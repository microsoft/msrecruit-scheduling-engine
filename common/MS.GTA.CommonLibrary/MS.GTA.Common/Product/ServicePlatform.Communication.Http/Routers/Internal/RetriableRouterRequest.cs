//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication.Http.Internal;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Fabric;
using MS.GTA.ServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.Communication.Http.Routers.Internal
{
    /// <summary>
    /// Base class for retriable router requests.
    /// </summary>
    internal sealed class RetriableRouterRequest : IHttpRouterRequest
    {
        private static readonly HashSet<string> safeRequestMethods = new HashSet<string> { "GET", "HEAD", "TRACE", "OPTIONS" };
        private static readonly HashSet<string> idempotentRequestMethods = new HashSet<string> { "GET", "PUT", "DELETE", "HEAD", "TRACE", "OPTIONS" };
        private static readonly IAsyncDelay sharedTaskAsyncDelay = new TaskAsyncDelay();

        private readonly IAsyncDelay asyncDelay;
        private readonly RequestRetryOptions requestRetryOptions;
        private readonly IRetriableRouterRequest retriableRouterRequest;
        private readonly ILogger logger;

        private int resolutionCallCount = 0;
        private bool disposed = false;

        /// <summary>
        /// Initializes <see cref="RetriableRouterRequest"/> with the provided <paramref name="requestRetryOptions"/>
        /// </summary>
        internal RetriableRouterRequest(IRetriableRouterRequest retriableRouterRequest, RequestRetryOptions requestRetryOptions, ILoggerFactory loggerFactory = null)
            : this(retriableRouterRequest, requestRetryOptions, sharedTaskAsyncDelay, loggerFactory)
        {
        }

        // Test-only constructor
        internal RetriableRouterRequest(IRetriableRouterRequest retriableRouterRequest, RequestRetryOptions requestRetryOptions, IAsyncDelay asyncDelay, ILoggerFactory loggerFactory = null)
        {
            Contract.AssertValue(requestRetryOptions, nameof(requestRetryOptions));
            Contract.AssertValue(retriableRouterRequest, nameof(retriableRouterRequest));
            Contract.AssertValue(asyncDelay, nameof(asyncDelay));

            this.asyncDelay = asyncDelay;
            this.retriableRouterRequest = retriableRouterRequest;
            this.requestRetryOptions = requestRetryOptions;
            this.logger = loggerFactory?.CreateLogger<RetriableRouterRequest>();
        }

        // Internal test hook
        internal RequestRetryOptions RetryOptions
        {
            get { return new RequestRetryOptions(requestRetryOptions); }
        }

        /// <inheritdoc />
        public Task<Uri> GetNextEndpointAsync(HttpRequestMessage request, HttpResponseMessage lastResponse, HttpCommunicationException lastException, CancellationToken cancellationToken)
        {
            Contract.CheckValue(request, nameof(request));

            resolutionCallCount++;

            if (this.logger == null)
            {
                return ServiceContext.Activity.ExecuteAsync(
                    RetriableRouterActivityType.Instance,
                    async () => await this.GetNextEndpointAsyncInternal(request, lastResponse, lastException, cancellationToken));                
            }

            return this.logger.ExecuteAsync(
                RetriableRouterActivityType.Instance,
                async () => await this.GetNextEndpointAsyncInternal(request, lastResponse, lastException, cancellationToken),
                new [] { typeof(MonitoredFabricException) });
        }

        public async Task<Uri> GetNextEndpointAsyncInternal(HttpRequestMessage request, HttpResponseMessage lastResponse, HttpCommunicationException lastException, CancellationToken cancellationToken)
        {
            if (resolutionCallCount == 1)
            {
                Contract.Check(lastResponse == null && lastException == null, "Initial call to GetNextEndpointAsync should contain neither a lastRequest, nor a lastException");
                return await retriableRouterRequest.GetNextEndpointAsync(request, cancellationToken);
            }

            Contract.Check(
                (lastResponse != null && lastException == null) || (lastResponse == null && lastException != null), 
                "Retry calls to GetNextEndpointAsync should contain either a lastRequest, or a lastException");

            bool shouldRetry = false;
            if (lastResponse != null)
            {
                // Ask the retriable router implementation to retry or not
                shouldRetry = retriableRouterRequest.ShouldRetry(request, lastResponse);
            }
            else
            {
                shouldRetry = ShouldRetry(request, lastException);
            }

            if (shouldRetry)
            {
                var retryAttempt = resolutionCallCount - 1;
                if (retryAttempt > requestRetryOptions.MaxRetryAttempts)
                {
                    throw new RouterRetriesExhaustedException(lastException);
                }
                else
                {
                    await asyncDelay.Delay(requestRetryOptions.DelayFunction.GetDelay(retryAttempt), cancellationToken);
                    var nextEndpoint = await retriableRouterRequest.GetNextEndpointAsync(request, cancellationToken);

                    if (nextEndpoint == null)
                    {
                        throw new NoRouterEndpointsAvailableException(lastException);
                    }

                    return nextEndpoint;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns true for communication exceptions based on the <see cref="RetriableRouterOptions"/> provided during construction. 
        /// For all other cases, including non-success responses received from the server, false will be returned and the request will
        /// not be retried.
        /// </summary>
        private bool ShouldRetry(HttpRequestMessage request, HttpCommunicationException lastException)
        {
            bool shouldRetry = false;
            string retryBehaviorTarget = null;
            RequestRetryBehavior retryBehavior;

            if (lastException is HttpEndpointUnreachableException)
            {
                retryBehaviorTarget = "EndpointUnreachableRetryBehavior";
                retryBehavior = requestRetryOptions.EndpointUnreachableRetryBehavior;

                shouldRetry = ShouldRetry(request.Method, retryBehavior);
            }
            else if (lastException is HttpTransportFailureException)
            {
                retryBehaviorTarget = "TransportFailureRetryBehavior";
                retryBehavior = requestRetryOptions.TransportFailureRetryBehavior;

                shouldRetry = ShouldRetry(request.Method, retryBehavior);
            }
            else
            {
                this.Log($"{request.Method.Method} request will NOT be retried because it completed with a non-retriable exception: Details - {lastException} and detals - {lastException.InnerException} and {lastException.Message} and {lastException.Kind}");
                return false;
            }

            if (shouldRetry)
            {
                this.Log(
                    $"{request.Method.Method} request will be retried because the {retryBehaviorTarget} is set to {retryBehavior}");

                return true;
            }
            else
            {
                this.Log(
                    $"{request.Method.Method} request will NOT be retried because the {retryBehaviorTarget} is set to {retryBehavior}");

                return false;
            }
        }

        private void Log(string msg)
        {
            if (this.logger == null)
            {
                CommunicationTraceSource.Instance.TraceInformation(msg);
            }
            else
            {
                this.logger.LogInformation(msg);
            }
        }

        /// <summary>
        /// Helper method to determine whether an <see cref="HttpRequestMessage"/> is retriable given a <see cref="RequestRetryBehavior"/>
        /// </summary>
        private static bool ShouldRetry(HttpMethod method, RequestRetryBehavior retryBehavior)
        {
            Contract.AssertValue(method, nameof(method));

            if (retryBehavior == RequestRetryBehavior.Never)
            {
                return false;
            }
            else if (retryBehavior == RequestRetryBehavior.Always)
            {
                return true;
            }

            if ((retryBehavior & RequestRetryBehavior.IdempotentRequests) == RequestRetryBehavior.IdempotentRequests)
            {
                if (idempotentRequestMethods.Contains(method.Method))
                {
                    return true;
                }
            }

            if ((retryBehavior & RequestRetryBehavior.SafeRequests) == RequestRetryBehavior.SafeRequests)
            {
                if (safeRequestMethods.Contains(method.Method))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!disposed)
            {
                retriableRouterRequest.Dispose();
                disposed = true;
            }
        }

        private sealed class RetriableRouterActivityType : SingletonActivityType<RetriableRouterActivityType>
        {
            public RetriableRouterActivityType()
                : base("SP.RetriableHttpRouter.GetNextEndpoint")
            {
            }
        }
    }
}
