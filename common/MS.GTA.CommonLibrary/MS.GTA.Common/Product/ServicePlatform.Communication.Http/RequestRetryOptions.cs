//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ServicePlatform.Communication.Http
{
    using System;
    using CommonDataService.Common.Internal;
    using Utils;

    public sealed class RequestRetryOptions
    {
        private int maxRetryAttempts;
        private DelayFunction delayFunction = DelayFunction.Zero;

        /// <summary>
        /// Creates a new instance of <see cref="RequestRetryOptions"/> with default values.
        /// </summary>
        public RequestRetryOptions()
        {
            MaxRetryAttempts = 3;
            DelayFunction = new CompositeDelay(
                initialDelays: new[] { TimeSpan.Zero },
                nextDelays: new ExponentialBackoffDelay(TimeSpan.FromMilliseconds(100), TimeSpan.FromMinutes(1)),
                spread: 0.5);
            EndpointUnreachableRetryBehavior = RequestRetryBehavior.Always;
            TransportFailureRetryBehavior = RequestRetryBehavior.SafeRequests;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RequestRetryOptions"/> with values copied from <paramref name="original"/>.
        /// </summary>
        public RequestRetryOptions(RequestRetryOptions original)
        {
            Contract.CheckValue(original, nameof(original));

            MaxRetryAttempts = original.MaxRetryAttempts;
            DelayFunction = original.DelayFunction;
            EndpointUnreachableRetryBehavior = original.EndpointUnreachableRetryBehavior;
            TransportFailureRetryBehavior = original.TransportFailureRetryBehavior;
        }

        /// <summary>
        /// Gets or sets the maximum request retry attempts to perform. The value must be a positive number or zero. 
        /// 
        /// The default value is zero.
        /// </summary>
        public int MaxRetryAttempts
        {
            get { return maxRetryAttempts; }
            set
            {
                Contract.CheckRange(value >= 0, nameof(value));
                maxRetryAttempts = value;
            }
        }

        /// <summary>
        /// The retry back off function.
        /// 
        /// The default is <see cref="DelayFunction.Zero"/>.
        /// </summary>
        public DelayFunction DelayFunction
        {
            get { return delayFunction; }
            set
            {
                Contract.CheckValue(value, nameof(value));
                delayFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets the retry behavior when the target endpoint is unreachable - i.e. a connection
        /// fails to be established. In this case we can guarantee that the request has not been received
        /// by the remote server so it is normally always safe to retry. 
        /// 
        /// Endpoint unreachable failures include:
        /// 
        /// - ConnectFailure
        /// - NameResolutionFailure
        /// - ProxyNameResolutionFailure
        /// 
        /// The default value is <see cref="RequestRetryBehavior.Never"/>.
        /// </summary>
        public RequestRetryBehavior EndpointUnreachableRetryBehavior { get; set; }

        /// <summary>
        /// Gets or sets the retry behavior when the communication fails for any other reason than an unreachable
        /// endpoint. In this case we don't know whether the target server received the request so it is normally
        /// only safe to retry when the request is idempotent. 
        /// 
        /// Transport failures include:
        /// 
        /// - ReceiveFailure
        /// - SendFailure
        /// - PipelineFailure
        /// - ConnectionClosed
        /// - KeepAliveFailure
        /// 
        /// The default value is to retry <see cref="RequestRetryBehavior.Never"/>.
        /// </summary>
        public RequestRetryBehavior TransportFailureRetryBehavior { get; set; }
    }
}
