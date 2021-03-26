//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net.Http;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Utils;

namespace HR.TA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Contains options for modifying <see cref="IHttpCommunicationClient"/> behavior.
    /// </summary>
    public sealed class HttpCommunicationClientOptions
    {
        // The default timeout is 100 seconds to be consistent with out of box HttpClient behavior
        // The max timeout is derived from CancellationTokenSource limitations
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(100.0);
        private static readonly TimeSpan maxTimeout = TimeUtil.MaxCancellationTokenTimeSpan;

        private TimeSpan timeout = defaultTimeout;

        /// <summary>
        /// Gets or sets whether an exceptions should be thrown for all non-successful responses. A response
        /// is only considered successful if the status code is in the 2xx range.
        /// 
        /// The default value is true.
        /// </summary>
        public bool ThrowOnNonSuccessResponse { get; set; } = true;

        /// <summary>
        /// Gets or sets the request timeout for the created <see cref="IHttpCommunicationClient"/> instance. The value must
        /// be either <see cref="Timeout.InfiniteTimeSpan"/> or a <see cref="TimeSpan"/> greater than zero and less or equal
        /// to <see cref="TimeSpan.FromMilliseconds(int.MaxValue)"/>.
        /// 
        /// The default timeout is 100 seconds.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return timeout; }
            set
            {
                Contract.CheckRange(
                    value == System.Threading.Timeout.InfiniteTimeSpan || (value > TimeSpan.Zero && value <= maxTimeout),
                    nameof(value));

                timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom request execution handlers. The handlers will be composed in the order 
        /// that they appear in the list with the transport handler being appended at the end by the 
        /// implementing client. 
        /// 
        /// All delegating handlers in the collection must not have their InnerHandler set. This
        /// means that delegating handlers cannot be reused across calls to the client factory Create() 
        /// method.
        /// 
        /// Empty collection and null are valid values meaning that the consumer does not want 
        /// any request behavior customization.
        /// </summary>
        public IReadOnlyList<DelegatingHandler> CustomHandlers { get; set; } = null;
    }
}
