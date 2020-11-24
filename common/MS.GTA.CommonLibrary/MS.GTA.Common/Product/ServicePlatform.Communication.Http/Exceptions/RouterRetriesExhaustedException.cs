// ----------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Thrown when retriable router retries have been exhausted
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class RouterRetriesExhaustedException : MonitoredException
    {
        internal RouterRetriesExhaustedException(HttpCommunicationException lastException)
            : base("Router retries have been exhausted", lastException)
        {
        }
    }
}
