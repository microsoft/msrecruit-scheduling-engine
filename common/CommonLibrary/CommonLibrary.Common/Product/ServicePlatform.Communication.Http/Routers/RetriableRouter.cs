//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Threading;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Communication.Http.Routers.Internal;
using CommonLibrary.ServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// Base class for router implementations with inbuilt request retry.
    /// </summary>
    public abstract partial class RetriableRouter : IHttpRouter
    {
        private static readonly RequestRetryOptions DefaultRequestRetryOptions = new RequestRetryOptions
        {
            MaxRetryAttempts = 3,
            DelayFunction = new CompositeDelay(
                    initialDelays: new[] { TimeSpan.Zero },
                    nextDelays: new ExponentialBackoffDelay(TimeSpan.FromMilliseconds(100), TimeSpan.FromMinutes(1)),
                    spread: 0.5),
            EndpointUnreachableRetryBehavior = RequestRetryBehavior.Always,
            TransportFailureRetryBehavior = RequestRetryBehavior.SafeRequests,
        };

        private readonly RequestRetryOptions requestRetryOptions;
        private string name;

        /// <summary>
        /// Initializes an instance of <see cref="RetriableRouter"/> with default request retry configuration:
        /// 
        ///   - <see cref="MaxRetryAttempts"/> set to 3, resulting in 4 total attempts
        ///   - <see cref="RequestRetryOptions.EndpointUnreachableRetryBehavior"/> set to <see cref="RequestRetryBehavior.Always"/>
        ///   - <see cref="RequestRetryOptions.TransportFailureRetryBehavior"/> set to <see cref="RequestRetryBehavior.SafeRequests"/>
        ///   - <see cref="RequestRetryOptions.DelayFunction"/>:
        /// 
        ///       Retry 1: Immediate
        ///       Retry 2: After 100 milliseconds (with 50% spread)
        ///       Retry 3: After 200 milliseconds (with 50% spread)
        /// </summary>
        public RetriableRouter()
            : this(DefaultRequestRetryOptions)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="RetriableRouter"/> with the provided <see cref="RequestRetryOptions"/>.
        /// </summary>
        public RetriableRouter(RequestRetryOptions requestRetryOptions)
        {
            Contract.CheckValue(requestRetryOptions, nameof(requestRetryOptions));

            this.requestRetryOptions = new RequestRetryOptions(requestRetryOptions);
        }

        /// <inheritdoc />
        public virtual string Name
        {
            get
            {
                if (name == null)
                {
                    var newName = GetType().FullName;
                    Interlocked.CompareExchange(ref name, newName, null);
                }

                return name;
            }
        }

        /// <summary>
        /// Implementing class provides an instance of <see cref="IRetriableRouterRequest"/>.
        /// </summary>
        protected abstract IRetriableRouterRequest CreateRetriableRouterRequest();

        /// <inheritdoc />
        public IHttpRouterRequest CreateRouterRequest()
        {
            return new RetriableRouterRequest(CreateRetriableRouterRequest(), requestRetryOptions);
        }
        
        /// <inheritdoc />
        public IHttpRouterRequest CreateRouterRequest(ILoggerFactory loggerFactory)
        {
            return new RetriableRouterRequest(CreateRetriableRouterRequest(), requestRetryOptions, loggerFactory);
        }
    }
}
