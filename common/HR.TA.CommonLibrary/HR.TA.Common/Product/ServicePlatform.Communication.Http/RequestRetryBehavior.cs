//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;

namespace HR.TA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Defines a router retry behavior for
    /// </summary>
    [Flags]
    public enum RequestRetryBehavior
    {
        /// <summary>
        /// Routers should never attempt to retry.
        /// </summary>
        Never = 0x0,

        /// <summary>
        /// Routers should retry only safe requests.
        /// 
        /// Safe requests include the following verbs:
        /// 
        /// - GET
        /// - HEAD
        /// - OPTIONS
        /// - TRACE
        /// </summary>
        SafeRequests = 0x1,

        /// <summary>
        /// Routers should retry only idempotent requests.
        /// 
        /// Idempotent requests include the following verbs:
        /// 
        /// - GET
        /// - HEAD
        /// - DELETE
        /// - OPTIONS
        /// - PUT
        /// - TRACE
        /// </summary>
        IdempotentRequests = 0x2,

        /// <summary>
        /// Routers should always attempt to retry.
        /// </summary>
        Always = int.MaxValue,
    }
}
