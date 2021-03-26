//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net;
using TA.CommonLibrary.ServicePlatform.Exceptions;

namespace TA.CommonLibrary.ServicePlatform.Communication.Http
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
