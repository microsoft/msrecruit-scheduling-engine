//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication.Http.Internal;

namespace MS.GTA.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// A simple router implementation with a single static URI and no retry behavior.
    /// </summary>
    public sealed class SingleUriRouter : RetriableRouter
    {
        private readonly Uri location;

        /// <summary>
        /// Creates a new instance of <see cref="SingleUriRouter"/> with the provided <paramref name="location"/> 
        /// and default <see cref="RetriableRouter"/> request retry configuration.
        /// </summary>
        public SingleUriRouter(Uri location)
        {
            CheckParams(location);
            this.location = location;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SingleUriRouter"/> with the provided <paramref name="location"/>
        /// and <paramref name="requestRetryOptions"/>.
        /// </summary>
        public SingleUriRouter(Uri location, RequestRetryOptions requestRetryOptions)
            : base(requestRetryOptions)
        {
            CheckParams(location);
            this.location = location;
        }

        private static void CheckParams(Uri location)
        {
            Contract.CheckValue(location, nameof(location));
            Contract.CheckParam(location.IsAbsoluteUri, nameof(location), "SingleUriRouter requires an absolute URI");
        }

        /// <inheritdoc />
        protected override IRetriableRouterRequest CreateRetriableRouterRequest()
        {
            return new SingleUriRouterRequest(location);
        }

        /// <summary>
        /// <see cref="IHttpRouterRequest"/> implementation for <see cref="SingleUriRouter"/>. The implementation 
        /// </summary>
        private sealed class SingleUriRouterRequest : IRetriableRouterRequest
        {
            private readonly Uri location;

            /// <summary>
            /// Creates a new instance of <see cref="SingleUriRouterRequest"/> with the provided <paramref name="location"/>.
            /// </summary>
            internal SingleUriRouterRequest(Uri location)
            {
                Contract.AssertValue(location, nameof(location));

                this.location = location;
            }

            /// <inheritdoc />
            public bool ShouldRetry(HttpRequestMessage request, HttpResponseMessage lastResponse)
            {
                // No retries when we get a response
                return false;
            }

            /// <inheritdoc />
            public Task<Uri> GetNextEndpointAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CommunicationTraceSource.Instance.TraceInformation($"SingleUriRouter returning its static location: '{location}'.");
                return Task.FromResult(location);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                // Nothing needed
            }
        }
    }
}
