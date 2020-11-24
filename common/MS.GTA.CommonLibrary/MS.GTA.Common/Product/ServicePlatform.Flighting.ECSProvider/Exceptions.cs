//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Flighting.ECSProvider
{
    /// <summary>
    /// Thrown when the ECS provider initialization exceeds the configured timeout.
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Service)]
    public sealed class ECSProviderInitializationTimedOutException : MonitoredException
    {
        internal ECSProviderInitializationTimedOutException(TimeSpan initializationTimeout)
        {
            InitializationTimeout = initializationTimeout;
        }

        /// <summary>
        /// The configured initialization timeout.
        /// </summary>
        [ExceptionCustomData]
        public TimeSpan InitializationTimeout { get; private set; }
    }
}
